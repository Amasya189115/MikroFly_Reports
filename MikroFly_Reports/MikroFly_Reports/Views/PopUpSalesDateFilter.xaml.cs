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
    public partial class PopUpSalesDateFilter : Rg.Plugins.Popup.Pages.PopupPage
    {
        public EventHandler<string> DateEventHandler;
        public PopUpSalesDateFilter()
        {
            InitializeComponent();
            FillComboBox();
        }
        private void FillComboBox()
        {
            List<string> itemlist = new List<string>();
            int loop = 0;
            int year = 2021;
            loop = DateTime.Now.Year - 2021;
            for (int i = 0; i <= loop; i++)
            {
                itemlist.Add(year.ToString());
                year++;
            }
            itemlist.Add("All Years");
            ComboBoxPeriod.ItemsSource = itemlist;
            ComboBoxPeriod.SelectedItem = "All Years";
        }

        private void ComboBoxPeriod_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private async void ButtonPopUpFilter_Clicked(object sender, EventArgs e)
        {
            if (ComboBoxPeriod.SelectedValue is null)
            { }
            else
            { DateEventHandler?.Invoke(this, ComboBoxPeriod.SelectedValue.ToString()); }

            await Navigation.PopPopupAsync();
        }

        private void ButtonPopUpExit_Clicked(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }
    }
}