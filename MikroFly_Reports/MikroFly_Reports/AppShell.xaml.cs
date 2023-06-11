
using DevExpress.XamarinForms.CollectionView;
using MikroFly_Reports.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MikroFly_Reports
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            DevExpress.XamarinForms.Editors.Initializer.Init();
            InitializeComponent();
            Routing.RegisterRoute(nameof(SummaryPage), typeof(SummaryPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(UpdatePasswordPage), typeof(UpdatePasswordPage));
            Routing.RegisterRoute(nameof(SalableInventoryPage), typeof(SalableInventoryPage));
            Routing.RegisterRoute(nameof(ListofLotNumbersPage), typeof(ListofLotNumbersPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        //private async void OnMenuItemClicked(object sender, EventArgs e)
        //{
        //    await Shell.Current.GoToAsync("//LoginPage");
        //}

        private async void ButtonExit_Clicked(object sender, EventArgs e)
        {
            FlyoutIsPresented = false;
            await Shell.Current.GoToAsync("///LoginPage");
        }
    }
}
