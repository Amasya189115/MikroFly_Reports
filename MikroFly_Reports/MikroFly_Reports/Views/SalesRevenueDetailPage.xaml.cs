using DevExpress.XamarinForms.Core.ConditionalFormatting;
using DevExpress.XamarinForms.DataGrid;
using MikroFly_Reports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalesRevenueDetailPage : ContentPage
    {
        private List<GeneralTwoColumnRevenue> GridData;
        public string Month = string.Empty;
        public string Year = string.Empty;
        public string Group = string.Empty;
        public string IlkTarih = "'20180101'";
        public string SonTarih = "GetDate()";
        public string Country = string.Empty;
        public string CustomerName = string.Empty;
        public string ProductName = string.Empty;
        public string FilteredColumn=string.Empty;
        public SalesRevenueDetailPage(List<GeneralTwoColumnRevenue> GridDataSource,string month, string year, string group, string country, string customer, string productname, string ilktarih, string sontarih,string filteredcolumn)
        {
            InitializeComponent();
            FilteredColumn = filteredcolumn;
            Month = month;
            Year = year;
            Group = group;
            Country = country;
            CustomerName = customer;
            ProductName = productname;
            IlkTarih = ilktarih;
            SonTarih = sontarih;
            GridData = GridDataSource;
            FillDataGrid();
        }
        private void FillDataGrid()
        {
            DataGridSalesRevenueDetail.ItemsSource = GridData;
            foreach (var column in DataGridSalesRevenueDetail.Columns)
            {               
                column.HeaderFontAttributes = FontAttributes.Bold;
                column.HeaderFontSize = 18;
                column.AutoFilterCondition = DevExpress.Data.AutoFilterCondition.Contains;
                column.VerticalContentAlignment = (TextAlignment)VerticalAlignment.Start;
                if (column.FieldName == "Revenue_Euro" && SalesRevenuePage.CurrencyOrPcs==string.Empty)
                    column.Caption = "Quantity";
            }
        }

        private void DataGridSalesRevenueDetail_DoubleTap(object sender, DevExpress.XamarinForms.DataGrid.DataGridGestureEventArgs e)
        {
            var selectedItem = e.Item as GeneralTwoColumnRevenue;
            if (FilteredColumn == "Year")
            {
                IlkTarih = "'" + selectedItem.Name.ToString() + "0101'";
                SonTarih = "'" + selectedItem.Name.ToString() + "1231'";
            }
            else
            {

            }
            Navigation.PushAsync(new SalesDetailFromReneuePage(selectedItem.Name.ToString(), Month, Year, Group, Country, CustomerName, ProductName, IlkTarih, SonTarih,FilteredColumn));
        }
    }
}