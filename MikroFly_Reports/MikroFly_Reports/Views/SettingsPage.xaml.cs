using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            GetPreferences();
        }

        private void ButtonSave_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("IP", EntryIP.Text);
            Preferences.Set("Database", EntryDatabase.Text);
            Preferences.Set("User", EntryUser.Text);
            Preferences.Set("Password", EntryPassword.Text);
        }

        private void ButtonCancel_Clicked(object sender, EventArgs e)
        {

        }
        private void GetPreferences()
        {
            if (Preferences.Get("IP", string.Empty) != string.Empty)
            {
                EntryIP.Text = Preferences.Get("IP", string.Empty);
            }
            if (Preferences.Get("Database", string.Empty) != string.Empty)
            {
                EntryDatabase.Text = Preferences.Get("Database", string.Empty);

            }
            if (Preferences.Get("User", string.Empty) != string.Empty)
            {
                EntryUser.Text = Preferences.Get("User", string.Empty);

            }
            if (Preferences.Get("Password", string.Empty) != string.Empty)
            {
                EntryPassword.Text = Preferences.Get("Password", string.Empty);

            }
        }
    }
}