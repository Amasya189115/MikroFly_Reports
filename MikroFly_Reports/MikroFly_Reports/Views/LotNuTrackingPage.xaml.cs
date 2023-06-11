using DocumentFormat.OpenXml.Spreadsheet;
using Microcharts;
using MikroFly_Reports.Models;
using MikroFly_Reports.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LotNuTrackingPage : ContentPage
    {
        private ExcelServices excelService;
        public List<LotNumbersTracking> trackings { get; set; }
        public LotNuTrackingPage()
        {
            InitializeComponent();

            excelService = new ExcelServices();
            trackings = new List<LotNumbersTracking>();
            GetLotNumbersTracking();
        }

        private void GetLotNumbersTracking()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[RIDVAN_TRACKING_LOT_NUMBERS]", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    trackings.Add(new LotNumbersTracking
                    {
                        Code = sdr[0].ToString(),
                        Desc = sdr[1].ToString(),
                        LotNumber = sdr[2].ToString(),
                        Module1 = Convert.ToDouble(sdr[3].ToString()),
                        Module2 = Convert.ToDouble(sdr[4].ToString()),
                        Module3 = Convert.ToDouble(sdr[5].ToString()),
                        Module4 = Convert.ToDouble(sdr[6].ToString()),
                        Module5 = Convert.ToDouble(sdr[7].ToString()),
                        Packed = Convert.ToDouble(sdr[8].ToString()),
                        Sterilized = Convert.ToDouble(sdr[9].ToString()),
                        Released = Convert.ToDouble(sdr[10].ToString()),
                        Sold = Convert.ToDouble(sdr[11].ToString()),
                    });
                }
                DataGridLotNuTracking.ItemsSource = trackings;
                foreach (var column in DataGridLotNuTracking.Columns)
                {
                    column.HeaderFontAttributes = FontAttributes.Bold;
                    column.HeaderFontSize = 15;
                    column.Width = 100;

                    if (column.FieldName == "LotNumber")
                    { column.FixedStyle = DevExpress.XamarinForms.DataGrid.FixedStyle.Start;
                        column.Width = 100;
                    }
                    if (column.FieldName == "Desc")
                    { column.IsGrouped = true; }
                    if (column.FieldName == "Code")
                    { column.IsVisible = false; }
                }
                sdr.Close();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.Message.ToString(), "Tamam");
            }
        }

        private void DataGridLotNuTracking_DoubleTap(object sender, DevExpress.XamarinForms.DataGrid.DataGridGestureEventArgs e)
        {

        }

        private void ToolBarToExcel_Clicked(object sender, EventArgs e)
        {
            //try
            //{
            //    var fileName = $"Contacts-{Guid.NewGuid()}.xlsx";
            //    string filepath = excelService.GenerateExcel(fileName);

            //    var data = new ExcelStructure
            //    {
            //        Headers = new List<string>() { "Name", "DepartmentName", "BirthDate", "BirthDate", "BirthDate", "Education",
            //    "EducationLevel", "EntryDate", "Gender", "Group", "MilitaryService", "RemainedLeaves", "SectionName", "Title"}
            //    };

            //    foreach (var item in HumanResourcesPage.PersonelInfos)
            //    {
            //        data.Values.Add(new List<string>() { item.Name, item.DepartmentName, item.BirthDate, item.BirthDate,
            //    item.BirthDate,item.Education,item.EducationLevel,item.EntryDate,item.Gender,item.Group,item.MilitaryService,
            //    item.RemainedLeaves,item.SectionName,item.Title});
            //    }

            //    excelService.InsertDataIntoSheet(filepath, "PersonnelInfo", data);

            //    await Launcher.OpenAsync(new OpenFileRequest()
            //    {
            //        File = new ReadOnlyFile(filepath)
            //    });
            //}
            //catch (Exception ex)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Uyarı", ex.Message, "Tamam");
            //}
        }
    }
}