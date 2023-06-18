using DevExpress.XamarinForms.DataGrid;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using MikroFly_Reports.Models;
using Rg.Plugins.Popup.Services;
using System;
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
    public partial class SalesDetailFromReneuePage : ContentPage
    {
        public List<SalesRecordsFiltered> SalesRecord { get; set; }
        public List<string> ColumnList;
        public string Value = string.Empty;
        public string Month = string.Empty;
        public string Year = string.Empty;
        public string Group = string.Empty;
        public string IlkTarih = "'20180101'";
        public string SonTarih = "GetDate()";
        public string Country = string.Empty;
        public string CustomerName = string.Empty;
        public string ProductName = string.Empty;
        public string FilteredColumn = string.Empty;
        public string GroupBy = "Customer";
        public SalesDetailFromReneuePage(string value, string month, string year, string group, string country, string customer, string productname, string ilktarih, string sontarih,string filteredcolumn)
        {
            InitializeComponent();
            FilteredColumn = filteredcolumn;
            Value = value;
            Month = month;
            Year = year;
            Group = group;
            Country = country;
            CustomerName = customer;
            ProductName = productname;
            IlkTarih = ilktarih;
            SonTarih = sontarih;
            SalesRecord = new List<SalesRecordsFiltered>();
            ColumnList = new List<string>();
            ArrangeFilterValues();    

        }
        private void ArrangeFilterValues()
        {
            switch (FilteredColumn)
            {
                case "Customer":
                    CustomerName = "%"+Value+"%";
                    break;
                case "Country":
                    Country = "%" + Value + "%";
                    break;
                case "Type":
                    ProductName = "%" + Value + "%";
                    break;
                case "Group":
                    Group = "%" + Value + "%";
                    break;
            }
            switch (Value)
            {
                case "Jan":
                    Month = "1";
                    break;
                case "Feb":
                    Month = "2";
                    break;
                case "Mar":
                    Month = "3";
                    break;
                case "Apr":
                    Month = "4";
                    break;
                case "May":
                    Month = "5";
                    break;
                case "Jun":
                    Month = "6";
                    break;
                case "Jul":
                    Month = "7";
                    break;
                case "Aug":
                    Month = "8";
                    break;
                case "Sep":
                    Month = "9";
                    break;
                case "Oct":
                    Month = "10";
                    break;
                case "Nov":
                    Month = "11";
                    break;
                case "Dec":
                    Month = "12";
                    break;

                default:
                    Month = "%%";
                    break;
            }

            FillDataGrid();
        }
        private void DataGridSalesList_PullToRefresh(object sender, EventArgs e)
        {
            FillDataGrid();
            DataGridSalesList.IsRefreshing = false;
        }

        private void FillDataGrid()
        {
            
            try
            {
                SalesRecord.Clear();
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select * from [dbo].[FX_MOBILAPP_SALES_SALESDETAILS] ( "+IlkTarih+"  , "+SonTarih+", '"+Month+"', '"+Year+"', '"+Group+"', '"+Country+"', '"+CustomerName+"', '"+ProductName+"' )", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    SalesRecord.Add(new SalesRecordsFiltered
                    {
                        Country = sdr[0].ToString(),
                        Customer = sdr[1].ToString(),
                        Group = sdr[2].ToString(),
                        Product = sdr[3].ToString(),
                        ShipmentDate = Convert.ToDateTime(sdr[4]),
                        Amount = float.Parse(sdr[5].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                        EuroValue = float.Parse(sdr[6].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                        Currency = sdr[7].ToString(),
                    });
                }
                DataGridSalesList.ItemsSource = null;
                DataGridSalesList.ItemsSource = SalesRecord;
                ColumnList.Clear();
                
                foreach (var column in DataGridSalesList.Columns)
                {
                    ColumnList.Add(column.FieldName);
                    column.HeaderFontAttributes = FontAttributes.Bold;
                    column.HeaderFontSize = 15;
                    column.Width = 100;
                    column.AutoFilterCondition = DevExpress.Data.AutoFilterCondition.Contains;

                    if (column.FieldName == "Product")
                    { column.FixedStyle = DevExpress.XamarinForms.DataGrid.FixedStyle.Start; }
                    if (column.FieldName == GroupBy)
                    { column.IsGrouped = true; 
                    }
                    
                }
                sdr.Close();
                sqlcon.Close();
            }
            catch (Exception ex)
            {

                Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Ok");
            }
        }

        private async void ToolBartoFilter_Clicked(object sender, EventArgs e)
        {
            try
            {
                var pageInfo = new PopUpGroupByColumns(ColumnList);
                pageInfo.DataEventHandler += async (popupsender, userdata) =>
                {
                    GroupBy = userdata;
                    FillDataGrid();

                };



                await PopupNavigation.Instance.PushAsync(pageInfo);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}