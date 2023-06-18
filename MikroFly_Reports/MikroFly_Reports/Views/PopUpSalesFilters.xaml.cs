using DocumentFormat.OpenXml;
using Rg.Plugins.Popup.Extensions;
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
    public partial class PopUpSalesFilters : Rg.Plugins.Popup.Pages.PopupPage
    {
        public List<string> Country;
        public List<string> Customer;
        public List<string> ProductType;
        public List<string> ColumnList;
        public EventHandler<string> MarketEventHandler;
        public EventHandler<string> CustomerEventHandler;
        public EventHandler<string> ProductTypeEventHandler;
        public EventHandler<string> GroupByEventHandler;
        public EventHandler<string> CountryEventHandler;
        public EventHandler<DateTime> StartDateEventHandler;
        public EventHandler<DateTime> EndDateEventHandler;
        public PopUpSalesFilters(List<string> CustomerFilter, List<string> CountryFilter, List<string> ProductTypeFilter, List<string> ColumnGroupList)
        {
            InitializeComponent();
            ColumnList = new List<string>();
            Country = new List<string>();
            Customer = new List<string>();
            ProductType = new List<string>();
            Customer = CustomerFilter;
            ColumnList = ColumnGroupList;
            Country=CountryFilter;
            ProductType=ProductTypeFilter;
            FillComboboxes();
        }
        private void FillComboboxes()
        {
            ComboBoxCountry.ItemsSource=Country;
            ComboBoxCustomer.ItemsSource=Customer;
            ComboBoxGroupBy.ItemsSource = ColumnList;
            ComboBoxProduct.ItemsSource = ProductType;
        }
        private void ButtonPopUpCLose_Clicked(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }

        private async void ButtonPopUpFilter_Clicked(object sender, EventArgs e)
        {

            if (ComboBoxCountry.SelectedValue is null)
            { }
            else
            { CountryEventHandler?.Invoke(this, ComboBoxCountry.SelectedValue.ToString()); }

            if (ComboBoxCustomer.SelectedValue is null)
            { }
            else
            { CustomerEventHandler?.Invoke(this, ComboBoxCustomer.SelectedValue.ToString()); }

            if (ComboBoxProduct.SelectedValue is null)
            { }
            else
            { ProductTypeEventHandler?.Invoke(this, ComboBoxProduct.SelectedValue.ToString()); }

            if (ComboBoxGroupBy.SelectedValue is null)
            { }
            else
            { GroupByEventHandler?.Invoke(this, ComboBoxGroupBy.SelectedValue.ToString()); }        
            
            StartDateEventHandler?.Invoke(this, DateTimePickerStartDate.Date);
            EndDateEventHandler?.Invoke(this, DateTimePickerEndDate.Date);
            await Navigation.PopPopupAsync();
        }
    }
}