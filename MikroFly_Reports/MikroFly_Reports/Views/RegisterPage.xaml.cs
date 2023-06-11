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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void ButtonUpdate_Clicked(object sender, EventArgs e)
        {
            //Navigation.PopAsync();
            //await Task.Delay(1);
            await Shell.Current.GoToAsync(nameof(UpdatePasswordPage));
        }
    }
}