using MikroFly_Reports.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductionPage : ContentPage
    {
        public ProductionPage()
        {
            InitializeComponent();
            GetProductionQuantities();
        }
        private void GetProductionQuantities()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcomM1 = new SqlCommand("select sum(sth_miktar) from STOK_HAREKETLERI inner join STOKLAR on sto_kod=sth_stok_kod"+
                    " where CAST(sth_create_date AS DATE) =CAST(GETDATE() AS DATE) and sth_stok_kod like '%M1' and sth_evraktip=7"+
                    " and sth_tip=0 and sth_cins=7 and sto_anagrup_kod='PR1'", sqlcon);
                LabelM1.Text = sqlcomM1.ExecuteScalar().ToString();
                SqlCommand sqlcomM2 = new SqlCommand("select sum(sth_miktar) from STOK_HAREKETLERI inner join STOKLAR on sto_kod=sth_stok_kod" +
    " where CAST(sth_create_date AS DATE) =CAST(GETDATE() AS DATE) and sth_stok_kod like '%M2' and sth_evraktip=7" +
    " and sth_tip=0 and sth_cins=7 and sto_anagrup_kod='PR1'", sqlcon);
                LabelM2.Text = sqlcomM2.ExecuteScalar().ToString();
                SqlCommand sqlcomM3 = new SqlCommand("select sum(sth_miktar) from STOK_HAREKETLERI inner join STOKLAR on sto_kod=sth_stok_kod" +
    " where CAST(sth_create_date AS DATE) =CAST(GETDATE() AS DATE) and sth_stok_kod like '%M3' and sth_evraktip=7" +
    " and sth_tip=0 and sth_cins=7 and sto_anagrup_kod='PR1'", sqlcon);
                LabelM3.Text = sqlcomM3.ExecuteScalar().ToString();
                SqlCommand sqlcomM4 = new SqlCommand("select sum(sth_miktar) from STOK_HAREKETLERI inner join STOKLAR on sto_kod=sth_stok_kod" +
    " where CAST(sth_create_date AS DATE) =CAST(GETDATE() AS DATE) and sth_stok_kod like '%M4' and sth_evraktip=7" +
    " and sth_tip=0 and sth_cins=7 and sto_anagrup_kod='PR1'", sqlcon);
                LabelM4.Text = sqlcomM4.ExecuteScalar().ToString();
                SqlCommand sqlcomM5 = new SqlCommand("select sum(sth_miktar) from STOK_HAREKETLERI inner join STOKLAR on sto_kod=sth_stok_kod" +
    " where CAST(sth_create_date AS DATE) =CAST(GETDATE() AS DATE) and sth_stok_kod like '%M5' and sth_evraktip=7" +
    " and sth_tip=0 and sth_cins=7 and sto_anagrup_kod='PR1'", sqlcon);
                LabelM5.Text = sqlcomM5.ExecuteScalar().ToString();
                SqlCommand sqlcomM6 = new SqlCommand("select sum(sth_miktar) from STOK_HAREKETLERI inner join STOKLAR on sto_kod=sth_stok_kod" +
    " where CAST(sth_create_date AS DATE) =CAST(GETDATE() AS DATE) and sth_stok_kod like '%PACK' and sth_evraktip=7" +
    " and sth_tip=0 and sth_cins=7 and sto_anagrup_kod='PR1'", sqlcon);
                LabelM6.Text = sqlcomM6.ExecuteScalar().ToString();

                sqlcon.Close();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error",ex.Message,"Ok");
            }
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(3000);
            GetProductionQuantities();
            RefreshView.IsRefreshing = false;
        }

        private void FrameTap(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var classId=frame.ClassId;
            Navigation.PushAsync(new DiaProdDetailsPage(classId.ToString()));
        }

    }
}