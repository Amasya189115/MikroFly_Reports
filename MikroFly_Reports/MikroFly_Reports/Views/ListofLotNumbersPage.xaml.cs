using DevExpress.XamarinForms.DataGrid;
using DocumentFormat.OpenXml.Wordprocessing;
using Microcharts;
using MikroFly_Reports.Models;
using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("Warehouse", "warehouse")]
    [QueryProperty("Desc", "desc")]
    [QueryProperty("Includepack", "includepack")]
    public partial class ListofLotNumbersPage : ContentPage
    {
        public List<LotNumbers> ListofLots;
        int count;
        public ListofLotNumbersPage()
        {
            InitializeComponent();
            
            Title = SummaryPage.pagetitles;
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                Command = new Command(() =>
                {
                    Navigation.RemovePage(this);
                })
            });

            FillDataGrid();
            
        }
        protected override bool OnBackButtonPressed()
        {
            Navigation.RemovePage(this);
            return true;
        }
        private void FillDataGrid()
        {
            ListofLots = new List<LotNumbers>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select [msg_S_0002],[msg_S_0161],CASE when [msg_S_0162]<10 then '-' else [msg_S_0162] end as 'Lot',pl_son_kullanim_tar,\r\nSUM(Round([msg_S_1563\\T],0)) FROM dbo.StokEnvanterYonetimi('20220101',getdate(),0,N'" + SalableInventoryPage.Warehouse +"',0,1)" +
                    " inner join PARTILOT on pl_stokkodu=msg_S_0001 and pl_partikodu=msg_S_0161 and pl_lotno=msg_S_0162 WHERE (([msg_S_0002] LIKE N'%" + SalableInventoryPage.Desc+"') and (([msg_S_0001] LIKE N'4%') or (([msg_S_0001] LIKE N'%"+ SalableInventoryPage.IncludePack +"%') ))) and ([msg_S_1563\\T])>0" +
                    " GROUP BY [msg_S_0002],[msg_S_0161],[msg_S_0162],pl_son_kullanim_tar\r\nORDER BY SUM([msg_S_1563\\T]) desc", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    ListofLots.Add(new LotNumbers
                    {
                        Desc = sdr[0].ToString(),
                        Lot = sdr[1].ToString(),
                        Cycle = sdr[2].ToString(),
                        ExpDate = Convert.ToDateTime(sdr[3].ToString()),
                        Quantity = float.Parse(sdr[4].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });
                }
                sdr.Close();
                sqlcon.Close();
                GridListofLotNumbers.ItemsSource = ListofLots;
                foreach (var column in GridListofLotNumbers.Columns)
                {
                    column.HeaderFontAttributes = FontAttributes.Bold;
                    column.HeaderFontSize = 15;
                    column.Width = 100;

                    //if (column.FieldName == "Lot")
                    //{ column.FixedStyle = DevExpress.XamarinForms.DataGrid.FixedStyle.Start; }
                    if (column.FieldName == "Desc")
                    { column.IsGrouped = true; }
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.Message.ToString(), "Tamam");
            }
        }

        private void GridListofLotNumbers_CalculateCustomSummary(object sender, DevExpress.XamarinForms.DataGrid.CustomSummaryEventArgs e)
        {
            if (e.FieldName.ToString() == "Quantity")
                if (e.IsTotalSummary)
                {
                    if (e.SummaryProcess == CustomSummaryProcess.Start)
                    {
                        count = 0;
                    }
                    if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                    {
                        if (!(bool)e.FieldValue)
                            count++;
                        e.TotalValue = count;
                    }
                }
        }
    }
}