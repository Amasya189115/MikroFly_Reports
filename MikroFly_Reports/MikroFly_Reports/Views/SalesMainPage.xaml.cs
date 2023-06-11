using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using Microcharts;
using MikroFly_Reports.Models;
using Rg.Plugins.Popup.Extensions;
using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalesMainPage : ContentPage
    {
        public List<SalableInventory> Sales;
        public string DateFilter = string.Empty;
        public SalesMainPage()
        {
            InitializeComponent();
            FillReportPeriod();
            FillDiaChart();
            FillBloChart();
            FillNeedleChart();
            FillPowChart();
            FillLabels();
        }

        private void FillLabels()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select sum(sth_miktar) 'Quantity'\r\nfrom CARI_HESAP_HAREKETLERI \r\ninner join STOK_HAREKETLERI on sth_fat_uid=cha_Guid\r\ninner join CARI_HESAPLAR on cha_kod=cari_kod\r\ninner join STOKLAR on sth_stok_kod=sto_kod\r\nleft outer join CARI_HESAP_ADRESLERI on adr_cari_kod=cari_kod and adr_adres_no=1\r\nwhere"+DateFilter+" sto_anagrup_kod = 'PR1' and  sto_isim Like '%Neph%' and cha_evrak_tip=63 and \r\ncari_unvan1 not in ('MEDICALPARK EOOD','ERDEMLER GERİ DÖNÜŞÜM METAL SANAYİ VE TİCARET LİMİTED ŞİRKETİ','TURKA GROUP MEDİKAL TİCARET ANONİM ŞİRKETİ','TURKAGROUP MEDİKAL TİCARET LTD.ŞTİ.')\r\n\r\n", sqlcon);
                LabelSoldDialyzers.Text = sqlcom.ExecuteScalar().ToString();
                sqlcon.Close();
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom1 = new SqlCommand("Select sum(sth_miktar) 'Quantity'\r\nfrom CARI_HESAP_HAREKETLERI \r\ninner join STOK_HAREKETLERI on sth_fat_uid=cha_Guid\r\ninner join CARI_HESAPLAR on cha_kod=cari_kod\r\ninner join STOKLAR on sth_stok_kod=sto_kod\r\nleft outer join CARI_HESAP_ADRESLERI on adr_cari_kod=cari_kod and adr_adres_no=1\r\nwhere"+DateFilter+" sto_anagrup_kod = 'PR2' and  sto_isim Like '%Line%' and cha_evrak_tip=63 and \r\ncari_unvan1 not in ('MEDICALPARK EOOD','ERDEMLER GERİ DÖNÜŞÜM METAL SANAYİ VE TİCARET LİMİTED ŞİRKETİ','TURKA GROUP MEDİKAL TİCARET ANONİM ŞİRKETİ','TURKAGROUP MEDİKAL TİCARET LTD.ŞTİ.')\r\n\r\n", sqlcon);
                LabelSoldBloodlines.Text = sqlcom1.ExecuteScalar().ToString();
                sqlcon.Close();
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom2 = new SqlCommand("Select sum(sth_miktar) 'Quantity'\r\nfrom CARI_HESAP_HAREKETLERI \r\ninner join STOK_HAREKETLERI on sth_fat_uid=cha_Guid\r\ninner join CARI_HESAPLAR on cha_kod=cari_kod\r\ninner join STOKLAR on sth_stok_kod=sto_kod\r\nleft outer join CARI_HESAP_ADRESLERI on adr_cari_kod=cari_kod and adr_adres_no=1\r\nwhere"+DateFilter+" sto_anagrup_kod = 'PR2' and  sto_isim Like '%Need%' and cha_evrak_tip=63 and \r\ncari_unvan1 not in ('MEDICALPARK EOOD','ERDEMLER GERİ DÖNÜŞÜM METAL SANAYİ VE TİCARET LİMİTED ŞİRKETİ','TURKA GROUP MEDİKAL TİCARET ANONİM ŞİRKETİ','TURKAGROUP MEDİKAL TİCARET LTD.ŞTİ.')\r\n\r\n", sqlcon);
                LabelSoldNeedles.Text = sqlcom2.ExecuteScalar().ToString();
                sqlcon.Close();
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom3 = new SqlCommand("Select sum(sth_miktar) 'Quantity'\r\nfrom CARI_HESAP_HAREKETLERI \r\ninner join STOK_HAREKETLERI on sth_fat_uid=cha_Guid\r\ninner join CARI_HESAPLAR on cha_kod=cari_kod\r\ninner join STOKLAR on sth_stok_kod=sto_kod\r\nleft outer join CARI_HESAP_ADRESLERI on adr_cari_kod=cari_kod and adr_adres_no=1\r\nwhere"+DateFilter+" sto_anagrup_kod = 'PR3' and  sto_isim Like '%%' and cha_evrak_tip=63 and \r\ncari_unvan1 not in ('MEDICALPARK EOOD','ERDEMLER GERİ DÖNÜŞÜM METAL SANAYİ VE TİCARET LİMİTED ŞİRKETİ','TURKA GROUP MEDİKAL TİCARET ANONİM ŞİRKETİ','TURKAGROUP MEDİKAL TİCARET LTD.ŞTİ.')\r\n\r\n", sqlcon);
                LabelSoldPowders.Text = sqlcom3.ExecuteScalar().ToString();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
            
        }
        private void FillReportPeriod()
        {
            List<string> itemlist=new List<string>();   
            int loop = 0;
            int year = 2020;
            loop=DateTime.Now.Year-2020;
            for(int i=0;i<=loop;i++)
            {
                itemlist.Add(year.ToString());
                year++;
            }
            itemlist.Add("All Years");
            ComboBoxPeriod.ItemsSource=itemlist;
            ComboBoxPeriod.SelectedItem="All Years";
        }
        private void FillDiaChart()
        {
            try
            {
                var entries = new List<ChartEntry>();
                Sales = new List<SalableInventory>();
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select substring(sto_isim,7,20), sum(sth_miktar) 'Quantity'\r\nfrom CARI_HESAP_HAREKETLERI \r\ninner join STOK_HAREKETLERI on sth_fat_uid=cha_Guid\r\ninner join CARI_HESAPLAR on cha_kod=cari_kod\r\ninner join STOKLAR on sth_stok_kod=sto_kod\r\nleft outer join CARI_HESAP_ADRESLERI on adr_cari_kod=cari_kod and adr_adres_no=1\r\nwhere"+DateFilter+" sto_anagrup_kod = 'PR1' and  sto_isim Like '%Neph%' and cha_evrak_tip=63 and \r\ncari_unvan1 not in ('MEDICALPARK EOOD','ERDEMLER GERİ DÖNÜŞÜM METAL SANAYİ VE TİCARET LİMİTED ŞİRKETİ','TURKA GROUP MEDİKAL TİCARET ANONİM ŞİRKETİ','TURKAGROUP MEDİKAL TİCARET LTD.ŞTİ.') Group By sto_isim order by sum(sth_miktar) desc\r\n\r\n", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    Sales.Add(new SalableInventory
                    {
                        Title = sdr[0].ToString(),
                        QTY = float.Parse(sdr[1].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });

                }

                sdr.Close();
                sqlcon.Close();
                foreach (var data in Sales)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(231, 65, 71),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries.Add(entry);
                };


                var chartBar = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };

                this.ChartView.Chart = chartBar;

            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }          
        }
        private void FillBloChart()
        {
            try
            {
                var entries = new List<ChartEntry>();
                Sales = new List<SalableInventory>();
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select substring(sto_isim,7,20), sum(sth_miktar) 'Quantity'\r\nfrom CARI_HESAP_HAREKETLERI \r\ninner join STOK_HAREKETLERI on sth_fat_uid=cha_Guid\r\ninner join CARI_HESAPLAR on cha_kod=cari_kod\r\ninner join STOKLAR on sth_stok_kod=sto_kod\r\nleft outer join CARI_HESAP_ADRESLERI on adr_cari_kod=cari_kod and adr_adres_no=1\r\nwhere"+DateFilter+" sto_anagrup_kod = 'PR2' and  sto_isim Like '%Line%' and cha_evrak_tip=63 and \r\ncari_unvan1 not in ('MEDICALPARK EOOD','ERDEMLER GERİ DÖNÜŞÜM METAL SANAYİ VE TİCARET LİMİTED ŞİRKETİ','TURKA GROUP MEDİKAL TİCARET ANONİM ŞİRKETİ','TURKAGROUP MEDİKAL TİCARET LTD.ŞTİ.') Group By sto_isim order by sum(sth_miktar) desc\r\n\r\n", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    Sales.Add(new SalableInventory
                    {
                        Title = sdr[0].ToString(),
                        QTY = float.Parse(sdr[1].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });

                }

                sdr.Close();
                sqlcon.Close();
                foreach (var data in Sales)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(231, 65, 71),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries.Add(entry);
                };


                var chartBar = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };

                this.ChartView1.Chart = chartBar;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        private void FillNeedleChart()
        {
            try
            {
                var entries = new List<ChartEntry>();
                Sales = new List<SalableInventory>();
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select substring(sto_isim,7,20), sum(sth_miktar) 'Quantity'\r\nfrom CARI_HESAP_HAREKETLERI \r\ninner join STOK_HAREKETLERI on sth_fat_uid=cha_Guid\r\ninner join CARI_HESAPLAR on cha_kod=cari_kod\r\ninner join STOKLAR on sth_stok_kod=sto_kod\r\nleft outer join CARI_HESAP_ADRESLERI on adr_cari_kod=cari_kod and adr_adres_no=1\r\nwhere"+DateFilter+" sto_anagrup_kod = 'PR2' and  sto_isim Like '%Need%' and cha_evrak_tip=63 and \r\ncari_unvan1 not in ('MEDICALPARK EOOD','ERDEMLER GERİ DÖNÜŞÜM METAL SANAYİ VE TİCARET LİMİTED ŞİRKETİ','TURKA GROUP MEDİKAL TİCARET ANONİM ŞİRKETİ','TURKAGROUP MEDİKAL TİCARET LTD.ŞTİ.') Group By sto_isim order by sum(sth_miktar) desc\r\n\r\n", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    Sales.Add(new SalableInventory
                    {
                        Title = sdr[0].ToString(),
                        QTY = float.Parse(sdr[1].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });

                }

                sdr.Close();
                sqlcon.Close();
                foreach (var data in Sales)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(231, 65, 71),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries.Add(entry);
                };


                var chartBar = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };
                this.ChartView2.Chart = chartBar;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        private void FillPowChart()
        {
            try
            {
                var entries = new List<ChartEntry>();
                Sales = new List<SalableInventory>();
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select substring(sto_isim,7,20), sum(sth_miktar) 'Quantity'\r\nfrom CARI_HESAP_HAREKETLERI \r\ninner join STOK_HAREKETLERI on sth_fat_uid=cha_Guid\r\ninner join CARI_HESAPLAR on cha_kod=cari_kod\r\ninner join STOKLAR on sth_stok_kod=sto_kod\r\nleft outer join CARI_HESAP_ADRESLERI on adr_cari_kod=cari_kod and adr_adres_no=1\r\nwhere"+DateFilter+" sto_anagrup_kod = 'PR3' and  sto_isim Like '%%' and cha_evrak_tip=63 and \r\ncari_unvan1 not in ('MEDICALPARK EOOD','ERDEMLER GERİ DÖNÜŞÜM METAL SANAYİ VE TİCARET LİMİTED ŞİRKETİ','TURKA GROUP MEDİKAL TİCARET ANONİM ŞİRKETİ','TURKAGROUP MEDİKAL TİCARET LTD.ŞTİ.') Group By sto_isim order by sum(sth_miktar) desc\r\n\r\n", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    Sales.Add(new SalableInventory
                    {
                        Title = sdr[0].ToString(),
                        QTY = float.Parse(sdr[1].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });

                }

                sdr.Close();
                sqlcon.Close();
                foreach (var data in Sales)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(231, 65, 71),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries.Add(entry);
                    
                };


                var chartBar = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };
                this.ChartView3.Chart = chartBar;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private void ComboBoxPeriod_SelectionChanged(object sender, EventArgs e)
        {
            if(ComboBoxPeriod.SelectedValue.ToString()=="All Years")
            {
                DateFilter = string.Empty;
            }
            else
            {
                DateFilter= " YEAR(cha_tarihi)='"+ ComboBoxPeriod.SelectedValue.ToString() + "' and ";
            }
            FillDiaChart();
            FillBloChart();
            FillNeedleChart();
            FillPowChart();
            FillLabels();
        }
    }
}