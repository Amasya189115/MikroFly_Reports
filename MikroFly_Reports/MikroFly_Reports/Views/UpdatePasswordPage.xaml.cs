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
    public partial class UpdatePasswordPage : ContentPage
    {
        public UpdatePasswordPage()
        {
            InitializeComponent();
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                Command = new Command(() =>
                {
                    Navigation.RemovePage(this);
                })
            });
        }

        private void ButtonSave_Clicked(object sender, EventArgs e)
        {

        }
        protected override bool OnBackButtonPressed()
        {
            Navigation.RemovePage(this);
            return true;
        }

    }
}