using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpPackedInvFilter : Rg.Plugins.Popup.Pages.PopupPage
    {
        public List<string> status;
        public EventHandler<string> DataEventHandler ;
        
        public PopUpPackedInvFilter()
        {
            InitializeComponent();
            status=new List<string>();
            FillComboBoxItems();            
        }
        private void FillComboBoxItems()
        {           
            status.Add("Salable");
            status.Add("Waiting For Release");
            status.Add("Sterilization");
            status.Add("Waiting For Sterilization");
            status.Add("All");
            ComboBoxFilter.ItemsSource = status;
        }
        private void SelectComboBoxItem()
        {
            ComboBoxFilter.SelectedValue = string.Empty;
        }
        private async void ButtonPopUpCLose_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        private void ComboBoxFilter_SelectionChanged(object sender, EventArgs e)
        {
            ButtonPopUpFilter.IsEnabled = true;
        }

        private async void ButtonPopUpFilter_Clicked(object sender, EventArgs e)
        {
            DataEventHandler?.Invoke(this, ComboBoxFilter.SelectedValue.ToString());
            await Navigation.PopPopupAsync();
        }
    }
}