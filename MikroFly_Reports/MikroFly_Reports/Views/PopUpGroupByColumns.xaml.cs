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
    public partial class PopUpGroupByColumns : Rg.Plugins.Popup.Pages.PopupPage
    {
        public EventHandler<string> DataEventHandler;
        public List<string> columns;
        public PopUpGroupByColumns(List<string> columns)
        {
            this.columns = columns;
            InitializeComponent();
            FillComboBoxColumns();
            
        }

        private void ComboBoxGroups_SelectionChanged(object sender, EventArgs e)
        {
            ButtonPopUpFilter.IsEnabled = true;
        }
        private void FillComboBoxColumns()
        {
            ComboBoxGroups.ItemsSource = columns;
        }
        private async void ButtonPopUpFilter_Clicked(object sender, EventArgs e)
        {
            DataEventHandler?.Invoke(this, ComboBoxGroups.SelectedValue.ToString());
            await Navigation.PopPopupAsync();
        }
        private async void ButtonPopUpCLose_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}