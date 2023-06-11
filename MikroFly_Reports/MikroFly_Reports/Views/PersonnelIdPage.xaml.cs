using MikroFly_Reports.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonnelIdPage : ContentPage
    {
        public byte[] Image { get; set; }
        string GetId;
        public PersonnelIdPage(string Id)
        {
            InitializeComponent();
            GetId = Id;
            GetVisual();
            GetPersonnelInfo();
        }
        private void GetVisual()
        {
            try
            {               
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcomM1 = new SqlCommand("SELECT TOP 1 Data ImgData FROM MikroDB_V16_PERSONEL.dbo.PERSONELLER WITH (NOLOCK) LEFT OUTER JOIN MikroDB_V16_PERSONEL.dbo.mye_ImageData ON TableID =71 AND Record_uid =per_Guid where per_kod='"+GetId+"'", sqlcon);
                Image = (byte[])sqlcomM1.ExecuteScalar();
                sqlcon.Close();
                var stream = new MemoryStream(Image);
                ImagePerson.Source = ImageSource.FromStream(() => stream);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.ToString(), "Tamam");
            }
        }
        private void GetPersonnelInfo()
        {
            foreach(var item in HumanResourcesPage.PersonelInfos)
            {
                if(item.Code==GetId)
                {
                    LabelName.Text = item.Name;
                    LabelTitle.Text = item.Title;
                    LabelDepartment.Text=item.DepartmentName;
                    LabelEntryDate.Text = "Entry Date: " + item.EntryDate;
                    LabelAnnualLeave.Text = "Remained Leaves: " + item.RemainedLeaves;
                }
            }
        }
    }
}