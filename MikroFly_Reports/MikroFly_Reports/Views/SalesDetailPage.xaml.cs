using DevExpress.Data.Native;
using DocumentFormat.OpenXml.Drawing.Charts;
using MikroFly_Reports.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
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
    public partial class SalesDetailPage : ContentPage
    {
        public List<string> Market;
        public List<string> Country;
        public List<string> Customer;
        public List<string> ProductType;
        public List<string> ColumnList;
        public string Group=string.Empty;
        public string FilterByMarket=string.Empty;
        public string FilterByCountry = string.Empty;
        public string FilterByCustomer = string.Empty;
        public string FilterByProductType = string.Empty;
        public string FilterByGroupBy = string.Empty;
        public string FilterByStockName = string.Empty;
        public string FilterByStartDate = string.Empty;
        public string FilterByEndDate = string.Empty;
        public List<SalesRecords> SalesRecord { get; set; }
        public List<SalesRecords> FilteredSalesRecord { get; set; }
        public SalesDetailPage()
        {
            InitializeComponent();
            SalesRecord = new List<SalesRecords>();
            FilteredSalesRecord = new List<SalesRecords>();
            Market = new List<string>();
            ColumnList = new List<string>();
            Country = new List<string>();
            Customer = new List<string>();  
            ProductType = new List<string>();
            Group = "Customer";
            FillDataGrid();
            GroupSaleColumns();
        }

        private async void ToolBarItemFilter_Clicked(object sender, EventArgs e)
        {
                try
                {
                       Group = "Customer";
                       FilterByCountry = string.Empty;
                       FilterByCustomer = string.Empty;
                       FilterByProductType = string.Empty;
                       FilterByGroupBy = string.Empty;
                       FilterByStockName = string.Empty;
                var pageInfo = new PopUpSalesFilters(Customer, Country, ProductType, ColumnList);
                pageInfo.CountryEventHandler += async (popupsender, userdata) =>
                {
                    FilterByCountry = userdata;
                };
                pageInfo.CustomerEventHandler += async (popupsender, userdata) =>
                {
                    FilterByCustomer = userdata;
                };
                pageInfo.ProductTypeEventHandler += async (popupsender, userdata) =>
                {
                    FilterByStockName= userdata;
                };
                pageInfo.GroupByEventHandler += async (popupsender, userdata) =>
                {
                    FilterByGroupBy = userdata;
                };
                pageInfo.StartDateEventHandler += async (popupsender, userdata) =>
                {
                    string ay = string.Empty;
                    if (userdata.Month < 10)
                        ay = "0";
                    string gun = string.Empty;
                    if (userdata.Month < 10)
                        gun = "0";
                    FilterByStartDate = "'"+userdata.Year.ToString() + ay + userdata.Month.ToString() + gun + userdata.Day.ToString()+"'";
                };
                pageInfo.EndDateEventHandler += async (popupsender, userdata) =>
                {
                    string ay=string.Empty;
                    if (userdata.Month < 10)
                        ay = "0";
                    string gun = string.Empty;
                    if (userdata.Day < 10)
                        gun = "0";
                    FilterByEndDate = "'"+userdata.Year.ToString()+ ay+userdata.Month.ToString()+ gun+userdata.Day.ToString()+"'";
                    FillDataGridFiltered();
                };

                await PopupNavigation.Instance.PushAsync(pageInfo);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",ex.Message,"Ok");
            }
            
        }

        private void GroupSaleColumns()
        {
            try
            {
                var groupByCustomer =
                from student in SalesRecord
                group student by student.Customer into groupedCustomer
                select groupedCustomer.Key;
                foreach (var item in groupByCustomer)
                {
                    Customer.Add(item.ToString());
                }
                var groupByCountry =
                from student in SalesRecord
                group student by student.Country into groupedCustomer
                select groupedCustomer.Key;
                foreach (var item in groupByCountry)
                {
                    Country.Add(item.ToString());
                }
                var groupByProductType =
                from student in SalesRecord
                group student by student.Product into groupedCustomer
                select groupedCustomer.Key;
                foreach (var item in groupByProductType)
                {
                    ProductType.Add(item.ToString());
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error",ex.Message,"Ok");
            }

            
        }
        private void FillDataGrid()
        {
            try
            {
                SalesRecord.Clear();
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select * from [dbo].[FX_MOBILAPP_SALES_SALESDETAILS_WITHLOTNUMBERS] ( '20200101'  , Getdate(), '%%', '', '%%', '%%', '%%', '%%' )", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    SalesRecord.Add(new SalesRecords
                    {
                        Country = sdr[0].ToString(),
                        Customer = sdr[1].ToString(),
                        Group = sdr[2].ToString(),
                        Product = sdr[3].ToString(),
                        LotNu = sdr[4].ToString(),
                        SterNu = sdr[5].ToString(),
                        Date = Convert.ToDateTime(sdr[6]),
                        Quantity = float.Parse(sdr[7].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                        EuroValue = float.Parse(sdr[8].ToString(), CultureInfo.InvariantCulture.NumberFormat),                      
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
                    if (column.FieldName == "Customer")
                    { column.IsGrouped = true; }
                }
                sdr.Close();
                sqlcon.Close();
            }
            catch (Exception ex)
            {

                Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Ok");
            }

        }
        private async void FillDataGridFiltered()
        {
            try
            {
                FilteredSalesRecord.Clear();
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select * from [dbo].[FX_MOBILAPP_SALES_SALESDETAILS_WITHLOTNUMBERS] ( "+FilterByStartDate+"  , "+FilterByEndDate+", '%%', '', '%%', '%"+FilterByCountry+"%', '%"+FilterByCustomer+"%', '%"+FilterByStockName+"%' )", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    FilteredSalesRecord.Add(new SalesRecords
                    {
                        Country = sdr[0].ToString(),
                        Customer = sdr[1].ToString(),
                        Group = sdr[2].ToString(),
                        Product = sdr[3].ToString(),
                        LotNu = sdr[4].ToString(),
                        SterNu = sdr[5].ToString(),
                        Date = Convert.ToDateTime(sdr[6]),
                        Quantity = float.Parse(sdr[7].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                        EuroValue = float.Parse(sdr[8].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });
                }
                DataGridSalesList.ItemsSource=null;
                DataGridSalesList.ItemsSource = FilteredSalesRecord;
                ColumnList.Clear();
                if (FilterByGroupBy != String.Empty)
                    Group = FilterByGroupBy;

                foreach (var column in DataGridSalesList.Columns)
                {
                    ColumnList.Add(column.FieldName);
                    column.HeaderFontAttributes = FontAttributes.Bold;
                    column.HeaderFontSize = 15;
                    column.Width = 100;
                    column.AutoFilterCondition = DevExpress.Data.AutoFilterCondition.Contains;

                    if (column.FieldName == "Product")
                    { column.FixedStyle = DevExpress.XamarinForms.DataGrid.FixedStyle.Start; }
                    if (column.FieldName == Group)
                    { column.IsGrouped = true; }
                }
                sdr.Close();
                sqlcon.Close();
            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Ok");
            }
        }
        private async void DataGridSalesList_PullToRefresh(object sender, EventArgs e)
        {
            FillDataGridFiltered();
            await Task.Delay(2000);
            DataGridSalesList.IsRefreshing = false;
        }
    }
}