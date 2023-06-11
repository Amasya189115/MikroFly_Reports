
using MikroFly_Reports.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public static string ConnectionString { get; set; }
        public IReadOnlyList<SalableInventory> SalableDialyzers { get; set; }
        public IReadOnlyList<SalableInventory> SalableBloodlines { get; set; }
        public IReadOnlyList<SalableInventory> SalableNeedles { get; set; }
        public IReadOnlyList<SalableInventory> SalablePowders { get; set; }
        public LoginPage()
        {
            InitializeComponent();
            ConnectionString = @"Data Source=" + Preferences.Get("IP", string.Empty) + ";Initial Catalog=" + Preferences.Get("Database", string.Empty) +
                ";User ID=" + Preferences.Get("User", string.Empty) + ";Password=" + Preferences.Get("Password", string.Empty) + ";Connection Timeout=2";
           
        }
        private void PickerUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///SummaryPage");
        }

        private async void ButtonSettings_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new SettingsPage());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("ok", ex.ToString(), "ok");
            }

        }

        private async void ButtonRegister_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(RegisterPage));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("ok", ex.ToString(), "ok");
            }
        }
    }
}