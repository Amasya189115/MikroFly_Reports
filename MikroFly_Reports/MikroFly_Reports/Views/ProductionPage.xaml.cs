using MikroFly_Reports.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
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
            Colorize();
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

            if (LabelM6.Text != string.Empty)
                FrameM6.BackgroundColor = Color.Red;



        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(3000);
            GetProductionQuantities();
            Colorize();
            RefreshView.IsRefreshing = false;
        }

        private void FrameTap(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var classId=frame.ClassId;
            Navigation.PushAsync(new DiaProdDetailsPage(classId.ToString()));
        }
        private void Colorize()
        {
            string data = "NotActive";
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();

                SqlCommand sqlcomM1 = new SqlCommand("select OpT_IsEmriKodu 'JobOrder',sto_isim 'Code',per_adi+' '+per_soyadi 'Operator' from URETIM_OPERASYON_HAREKETLERI" +
                    " inner join PERSONELLER on URETIM_OPERASYON_HAREKETLERI.OpT_PersonelKodu=PERSONELLER.per_kod" +
                    " inner join STOKLAR on STOKLAR.sto_kod=URETIM_OPERASYON_HAREKETLERI.OpT_UrunKodu WHERE OpT_bitis_tarihi<'19000101' and OpT_OperasyonKodu='" + "3-M10" + "'", sqlcon);
                SqlDataReader sdr = sqlcomM1.ExecuteReader();
                data= "NotActive";
                while (sdr.Read())
                {
                    data = sdr[0].ToString();
                }
                if(data!= "NotActive")
                FrameM1.BackgroundColor = Color.SpringGreen;
                else FrameM1.BackgroundColor = Color.White;
                sdr.Close();

                SqlCommand sqlcomM2 = new SqlCommand("select OpT_IsEmriKodu 'JobOrder',sto_isim 'Code',per_adi+' '+per_soyadi 'Operator' from URETIM_OPERASYON_HAREKETLERI" +
    " inner join PERSONELLER on URETIM_OPERASYON_HAREKETLERI.OpT_PersonelKodu=PERSONELLER.per_kod" +
    " inner join STOKLAR on STOKLAR.sto_kod=URETIM_OPERASYON_HAREKETLERI.OpT_UrunKodu WHERE OpT_bitis_tarihi<'19000101' and OpT_OperasyonKodu='" + "3-M20" + "'", sqlcon);
                SqlDataReader sdr2 = sqlcomM2.ExecuteReader();
                data = "NotActive";
                while (sdr2.Read())
                {
                    data = sdr2[0].ToString();
                }
                if (data != "NotActive")
                    FrameM2.BackgroundColor = Color.SpringGreen;
                else FrameM2.BackgroundColor = Color.White;
                sdr2.Close();

                SqlCommand sqlcomM3 = new SqlCommand("select OpT_IsEmriKodu 'JobOrder',sto_isim 'Code',per_adi+' '+per_soyadi 'Operator' from URETIM_OPERASYON_HAREKETLERI" +
" inner join PERSONELLER on URETIM_OPERASYON_HAREKETLERI.OpT_PersonelKodu=PERSONELLER.per_kod" +
" inner join STOKLAR on STOKLAR.sto_kod=URETIM_OPERASYON_HAREKETLERI.OpT_UrunKodu WHERE OpT_bitis_tarihi<'19000101' and OpT_OperasyonKodu='" + "3-M30" + "'", sqlcon);
                SqlDataReader sdr3 = sqlcomM3.ExecuteReader();
                data = "NotActive";
                while (sdr3.Read())
                {
                    data = sdr3[0].ToString();
                }
                if (data != "NotActive")
                    FrameM3.BackgroundColor = Color.SpringGreen;
                else FrameM3.BackgroundColor = Color.White;
                sdr3.Close();

                SqlCommand sqlcomM4 = new SqlCommand("select OpT_IsEmriKodu 'JobOrder',sto_isim 'Code',per_adi+' '+per_soyadi 'Operator' from URETIM_OPERASYON_HAREKETLERI" +
" inner join PERSONELLER on URETIM_OPERASYON_HAREKETLERI.OpT_PersonelKodu=PERSONELLER.per_kod" +
" inner join STOKLAR on STOKLAR.sto_kod=URETIM_OPERASYON_HAREKETLERI.OpT_UrunKodu WHERE OpT_bitis_tarihi<'19000101' and OpT_OperasyonKodu='" + "3-M40" + "'", sqlcon);
                SqlDataReader sdr4 = sqlcomM4.ExecuteReader();
                data = "NotActive";
                while (sdr4.Read())
                {
                    data = sdr4[0].ToString();
                }
                if (data != "NotActive")
                    FrameM4.BackgroundColor = Color.SpringGreen;
                else FrameM4.BackgroundColor = Color.White;
                sdr4.Close();

                SqlCommand sqlcomM5 = new SqlCommand("select OpT_IsEmriKodu 'JobOrder',sto_isim 'Code',per_adi+' '+per_soyadi 'Operator' from URETIM_OPERASYON_HAREKETLERI" +
" inner join PERSONELLER on URETIM_OPERASYON_HAREKETLERI.OpT_PersonelKodu=PERSONELLER.per_kod" +
" inner join STOKLAR on STOKLAR.sto_kod=URETIM_OPERASYON_HAREKETLERI.OpT_UrunKodu WHERE OpT_bitis_tarihi<'19000101' and OpT_OperasyonKodu='" + "3-M50" + "'", sqlcon);
                SqlDataReader sdr5 = sqlcomM5.ExecuteReader();
                data = "NotActive";
                while (sdr5.Read())
                {
                    data = sdr5[0].ToString();
                }
                if (data != "NotActive")
                    FrameM5.BackgroundColor = Color.SpringGreen;
                else FrameM5.BackgroundColor = Color.White;
                sdr5.Close();

                SqlCommand sqlcomM6 = new SqlCommand("select OpT_IsEmriKodu 'JobOrder',sto_isim 'Code',per_adi+' '+per_soyadi 'Operator' from URETIM_OPERASYON_HAREKETLERI" +
" inner join PERSONELLER on URETIM_OPERASYON_HAREKETLERI.OpT_PersonelKodu=PERSONELLER.per_kod" +
" inner join STOKLAR on STOKLAR.sto_kod=URETIM_OPERASYON_HAREKETLERI.OpT_UrunKodu WHERE OpT_bitis_tarihi<'19000101' and OpT_OperasyonKodu='" + "3-M60" + "'", sqlcon);
                SqlDataReader sdr6 = sqlcomM6.ExecuteReader();
                data = "NotActive";
                while (sdr6.Read())
                {
                    data = sdr6[0].ToString();
                }
                if (data != "NotActive")
                    FrameM6.BackgroundColor = Color.SpringGreen;
                else FrameM6.BackgroundColor = Color.White;

                sdr6.Close();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.ToString(), "Tamam");
            }

        }
    }
}