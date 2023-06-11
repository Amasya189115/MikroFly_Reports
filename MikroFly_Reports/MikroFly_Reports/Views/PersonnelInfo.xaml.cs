using MikroFly_Reports.Models;
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
    public partial class PersonnelInfo : ContentPage
    {
        public string GetCode;
        public string GetVisibleColumn;
        public PersonnelInfo(string visibleColumn)
        {
            InitializeComponent();
            GetVisibleColumn= visibleColumn;
            if (visibleColumn == null)
            {
                FillDataGridWithoutFilter();
            }
            else
            {
                FillDataGridWithFilter();
            }
            
        }
        private void FillDataGridWithFilter()
        {
            DataGridPersonnelInfo.ItemsSource = HumanResourcesPage.PersonelInfos;
            foreach (var column in DataGridPersonnelInfo.Columns)
            {
                column.HeaderFontAttributes = FontAttributes.Bold;
                column.HeaderFontSize = 15;
                column.IsVisible = false;

                if (column.FieldName == GetVisibleColumn || column.FieldName=="Name")
                { column.IsVisible = true; }
            }
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
            var id=(e.Item as PersonelInfo).Code;
            Navigation.PushAsync(new PersonnelIdPage(id.ToString()));
        }
    }
}