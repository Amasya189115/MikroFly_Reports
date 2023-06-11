using DevExpress.XamarinForms.DataGrid;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using MikroFly_Reports.Models;
using MikroFly_Reports.Services;
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

    public partial class SalableInventoryPage : ContentPage
    {
        public static string Desc;
        public static string Warehouse;
        public static string IncludePack;
        private ExcelServices excelService;
        private List<SalableInventory> salables;
        public SalableInventoryPage(List<SalableInventory> Inventory)
        {
            InitializeComponent();
            Title = SummaryPage.pagetitles;
            excelService = new ExcelServices();
            salables = Inventory;
            DataGridViewAll.ItemsSource = Inventory;
            foreach (var column in DataGridViewAll.Columns)
            {
                column.HeaderFontAttributes = FontAttributes.Bold;
                column.AutoFilterCondition=DevExpress.Data.AutoFilterCondition.Contains;
            }            
        }

        private async void ToolBarToExcel_Clicked(object sender, EventArgs e)
        {
            var fileName = $"Contacts-{Guid.NewGuid()}.xlsx";
            string filepath = excelService.GenerateExcel(fileName);

            var data = new ExcelStructure
            {
                Headers = new List<string>() { "Code", "Title", "Quantity" }
            };

            foreach (var item in salables)
            {
                data.Values.Add(new List<string>() { item.Code, item.Title, item.QTY.ToString() });
            }

            excelService.InsertDataIntoSheet(filepath, "SalableInventory", data);

            await Launcher.OpenAsync(new OpenFileRequest()
            {
                File = new ReadOnlyFile(filepath)
            });
        }

        private async void DataGridViewAll_DoubleTap(object sender, DataGridGestureEventArgs e)
        {
            Desc= (e.Item as SalableInventory).Title;
            switch (Title)
            {
                case "Salable Inventory":
                    Warehouse = "90";
                    break;
                case "Waiting For Release Inventory":
                    Warehouse = "80";
                    break;
                case "Sterilization Inventory":
                    Warehouse = "70";
                    break;
                case "Waiting For Sterilization Inventory":
                    Warehouse = "30,40,50,60";
                    break;
                default:
                    Warehouse = "30,40,50,60,70,80,90";
                    break;
            }
            if (Title=="Salable Inventory" || Title=="Waiting For Release Inventory")
            { IncludePack = "ZZZZ"; }            
            else
            { IncludePack = "PACK"; }
            await Shell.Current.GoToAsync($"ListofLotNumbersPage?Desc={Desc}&Warehouse={Warehouse}&Includepack={IncludePack}");
        }
    }
}