using DevExpress.XamarinForms.DataGrid;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BloProductionPage : ContentPage
    {
        public BloProductionPage()
        {
            InitializeComponent();
            GetProductionQuantities();
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(3000);
            GetProductionQuantities();
            RefreshView.IsRefreshing = false;
        }
        private void GetProductionQuantities()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcomM1 = new SqlCommand("select sum(sth_miktar) from STOK_HAREKETLERI inner join STOKLAR on sto_kod=sth_stok_kod" +
                    " where CAST(sth_create_date AS DATE) =CAST(GETDATE() AS DATE) and sto_isim like '%tub%' and sth_evraktip=7" +
                    " and sth_tip=0 and sth_cins=7 and sto_anagrup_kod='PR2'", sqlcon);
                LabelExtruder.Text = sqlcomM1.ExecuteScalar().ToString();
                SqlCommand sqlcomM2 = new SqlCommand("select sum(sth_miktar) from STOK_HAREKETLERI inner join STOKLAR on sto_kod=sth_stok_kod" +
    " where CAST(sth_create_date AS DATE) =CAST(GETDATE() AS DATE) and (sth_stok_kod like '%A' or sth_stok_kod like '%V') and sth_evraktip=7" +
    " and sth_tip=0 and sth_cins=7 and sto_anagrup_kod='PR2'", sqlcon);
                LabelAssembly.Text = sqlcomM2.ExecuteScalar().ToString();
                SqlCommand sqlcomM3 = new SqlCommand("select sum(sth_miktar) from STOK_HAREKETLERI inner join STOKLAR on sto_kod=sth_stok_kod" +
    " where CAST(sth_create_date AS DATE) =CAST(GETDATE() AS DATE) and sth_stok_kod like '%PACK' and sth_evraktip=7" +
    " and sth_tip=0 and sth_cins=7 and sto_anagrup_kod='PR2'", sqlcon);
                LabelPacking.Text = sqlcomM3.ExecuteScalar().ToString();

                sqlcon.Close();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private void FrameTap(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var classId = frame.ClassId;
            Navigation.PushAsync(new BloProdDetailsPage(classId.ToString()));
        }

    }
}