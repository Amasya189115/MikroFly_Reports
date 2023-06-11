using DevExpress.XamarinForms.DataGrid;
using Microcharts;
using MikroFly_Reports.Models;
using MikroFly_Reports.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
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
    public partial class PersonnelAllInfoPage : ContentPage
    {
        private ExcelServices excelService;
        public PersonnelAllInfoPage()
        {
            InitializeComponent();
            excelService = new ExcelServices();
            FillDataGridWithoutFilter();
        }
        private void FillDataGridWithoutFilter()
        {
            DataGridPersonnelInfo.ItemsSource = HumanResourcesPage.PersonelInfos;
            foreach (var column in DataGridPersonnelInfo.Columns)
            {
                column.HeaderFontAttributes = FontAttributes.Bold;
                column.HeaderFontSize = 15;
                column.Width = 100;

                if (column.FieldName == "Name")
                { column.FixedStyle = DevExpress.XamarinForms.DataGrid.FixedStyle.Start; }
                if (column.FieldName == "SectionName")
                { column.IsGrouped = true; }
                if (column.FieldName == "DepartmentName")
                { column.Width = 130; }
                if (column.FieldName == "Code" || column.FieldName == "DepartmentCode" || column.FieldName == "BirthDate" || column.FieldName == "MilitaryService")
                { column.IsVisible = false; }
            }
        }

        private void DataGridPersonnelInfo_DoubleTap(object sender, DevExpress.XamarinForms.DataGrid.DataGridGestureEventArgs e)
        {
            var id = (e.Item as PersonelInfo).Code;
            Navigation.PushAsync(new PersonnelIdPage(id.ToString()));
        }
        private async void ToolBarToExcel_Clicked(object sender, EventArgs e)
        {
            try
            {
                var fileName = $"Contacts-{Guid.NewGuid()}.xlsx";
                string filepath = excelService.GenerateExcel(fileName);

                var data = new ExcelStructure
                {
                    Headers = new List<string>() { "Name", "DepartmentName", "BirthDate", "BirthDate", "BirthDate", "Education",
                "EducationLevel", "EntryDate", "Gender", "Group", "MilitaryService", "RemainedLeaves", "SectionName", "Title"}
                };

                foreach (var item in HumanResourcesPage.PersonelInfos)
                {
                    data.Values.Add(new List<string>() { item.Name, item.DepartmentName, item.BirthDate, item.BirthDate,
                item.BirthDate,item.Education,item.EducationLevel,item.EntryDate,item.Gender,item.Group,item.MilitaryService,
                item.RemainedLeaves,item.SectionName,item.Title});
                }

                excelService.InsertDataIntoSheet(filepath, "PersonnelInfo", data);

                await Launcher.OpenAsync(new OpenFileRequest()
                {
                    File = new ReadOnlyFile(filepath)
                });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Uyarı",ex.Message,"Tamam");
            }

        }
    }
}