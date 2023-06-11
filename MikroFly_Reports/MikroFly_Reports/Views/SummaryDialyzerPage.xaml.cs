using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microcharts;
using MikroFly_Reports.Models;
using Rg.Plugins.Popup.Services;
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
    public partial class SummaryDialyzerPage : ContentPage
    {
        public string chartheaderdynamic = "Salable";
        double TotalDia;
        public List<SalableInventory> InventoryDia;
        public List<Sqm> sizes;
        public string stat = "90";
        public SummaryDialyzerPage()
        {
            InitializeComponent();
            DisplayChart();

        }
        private async Task DisplayChart()
        {
            if (stat != "All")
            {
                if (stat == "90" || stat == "80" || stat == "70")
                {
                    DisplayDiaChart(stat);
                }
                else
                {
                    DisplayDiaChartNonSteril(stat);
                }
            }
            else
            {
                DisplayDiaChartAll();
            }
            DisplayFluxChart();
            DisplaySizeChart();
            DisplayTypeChart();
        }
        private void DisplayDiaChart(string warehouse)
        {
            TotalDia = 0;
            InventoryDia = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT TOP 100 PERCENT sto_kod AS 'Code',substring(sto_isim,7,20) AS 'Title', Round(dbo.fn_DepodakiMiktar(sto_kod," + warehouse + " ,getdate()),0) 'QTY' " +
                "FROM dbo.STOKLAR WITH (NOLOCK) where sto_anagrup_kod in ('PR1') and (sto_kod like '4%' or sto_kod like '%PACK') AND dbo.fn_DepodakiMiktar(sto_kod," + warehouse + " ,getdate())>0 ORDER BY Round(dbo.fn_DepodakiMiktar(sto_kod," + warehouse + " ,getdate()),0)  desc", sqlcon);

                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryDia.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });
                    TotalDia = TotalDia + Convert.ToDouble(sdr[2].ToString());
                }
                LabelDia.Text = chartheaderdynamic + " Dialyzers: " + TotalDia.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryDia)
                {

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(0, 0, 255),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                        TextColor = SKColor.FromHsv(0, 0, 255),
                    };
                    entries.Add(entry);
                };


                var chartBar = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                };

                this.chartViewDia.Chart = chartBar;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.Message.ToString(), "Tamam");
            }
        }
        private void DisplayDiaChartNonSteril(string warehouse)
        {
            TotalDia = 0;
            InventoryDia = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT TOP 100 PERCENT sto_kod AS 'Code',substring(sto_isim,7,20) AS 'Title', Round((dbo.fn_DepodakiMiktar(sto_kod," + warehouse + " ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,30 ,getdate())),0) 'QTY' " +
                "FROM dbo.STOKLAR WITH (NOLOCK) where sto_anagrup_kod in ('PR1') and (sto_kod like '4%' or sto_kod like '%PACK') AND Round((dbo.fn_DepodakiMiktar(sto_kod," + warehouse + " ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,30 ,getdate())),0)>0 ORDER BY Round((dbo.fn_DepodakiMiktar(sto_kod," + warehouse + " ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,30 ,getdate())),0)  desc", sqlcon);

                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryDia.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });
                    TotalDia = TotalDia + Convert.ToDouble(sdr[2].ToString());
                }
                LabelDia.Text = "Total " + chartheaderdynamic + " Dialyzers: " + TotalDia.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryDia)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(0, 0, 255),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                        TextColor = SKColor.FromHsv(0, 0, 255),
                    };
                    entries.Add(entry);
                };


                var chartBar = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                };

                this.chartViewDia.Chart = chartBar;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.Message.ToString(), "Tamam");
            }
        }

        private void DisplayFluxChart()
        {
            float CountHF = 0;
            float CountLF = 0;
            foreach (var item in InventoryDia)
            {
                if (item.Title.Substring(item.Title.IndexOf("S")).Remove(item.Title.Substring(item.Title.IndexOf("S")).Length-1).Length == 3)
                {
                    CountHF = CountHF + item.QTY;
                }
                else
                {
                    CountLF=CountLF+item.QTY;
                }                                       
            }
            var entries = new List<ChartEntry>();

            var entry = new ChartEntry(CountHF)
            {

                Label = "Male",
                ValueLabel = (CountHF.ToString()),
                Color = SKColor.FromHsv(347, 89, 78),
                ValueLabelColor = SKColor.FromHsv(347, 89, 78),
                TextColor = SKColor.FromHsv(347, 89, 78),
            };
            var entry1 = new ChartEntry(CountLF)
            {
                Label = "Female",
                ValueLabel = (CountLF).ToString(),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            entries.Add(entry);
            entries.Add(entry1);

            var chartdonut = new DonutChart()
            {
                Entries = entries,
                LabelTextSize = 25,
                LabelMode = LabelMode.None
            };
            this.ChartFluxRatio.Chart = chartdonut;
            LabelHF.Text = "HF: %" + Math.Round((CountHF / (CountLF + CountHF) * 100), 0).ToString();
            LabelLF.Text = "LF: %" + Math.Round((CountLF / (CountLF + CountHF) * 100), 0).ToString();
            //LabelHF.TextColor = "#C7163C";
            //LabelObjective.Text = Achieved.ToString();
        }
        private void DisplayTypeChart()
        {
            float CountPS = 0;
            float CountPES = 0;
            foreach (var item in InventoryDia)
            {
                if (item.Title.Contains("PS"))
                {
                    CountPS = CountPS + item.QTY;
                }
                else
                {
                    CountPES=CountPES + item.QTY;
                }
            }
            var entries = new List<ChartEntry>();

            var entry = new ChartEntry(CountPS)
            {

                Label = "Male",
                ValueLabel = (CountPS.ToString()),
                Color = SKColor.FromHsv(347, 89, 78),
                ValueLabelColor = SKColor.FromHsv(347, 89, 78),
                TextColor = SKColor.FromHsv(347, 89, 78),
            };
            var entry1 = new ChartEntry(CountPES)
            {
                Label = "Female",
                ValueLabel = (CountPES).ToString(),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            entries.Add(entry);
            entries.Add(entry1);

            var chartdonut = new DonutChart()
            {
                Entries = entries,
                LabelTextSize = 25,
                LabelMode = LabelMode.None
            };
            this.ChartTypeRatio.Chart = chartdonut;
            LabelPS.Text = "PS: %" + Math.Round((CountPS / (CountPES + CountPS) * 100), 0).ToString();
            LabelPES.Text = "PES: %" + Math.Round((CountPES / (CountPES + CountPS) * 100), 0).ToString();
            //LabelObjective.Text = Achieved.ToString();
        }
        private void DisplaySizeChart()
        {
            bool check = false;
            sizes = new List<Sqm>();
            var entries = new List<ChartEntry>();
            try
            {
                foreach (var item in InventoryDia)
                {
                    foreach(var data in sizes)
                    {
                        if(item.Title.Substring(item.Title.IndexOf("S")+1,2) == data.Dimens)
                        {
                            check = true;
                            data.Quantity = data.Quantity + item.QTY;
                        }

                    }
                    if(check)
                    {

                    }
                    else
                    {
                        sizes.Add(new Sqm
                        {
                            Dimens= item.Title.Substring(item.Title.IndexOf("S")+1, 2),
                            Quantity=item.QTY
                        });
                    }
                    check = false;
                };
                foreach (var data in sizes)
                {
                    {
                        Random ran = new Random();
                        SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                        var entry = new ChartEntry(data.Quantity)
                        {
                            Label = data.Dimens,
                            ValueLabel ="% " + Math.Round((data.Quantity/TotalDia)*100,0).ToString(),
                            Color = SKColor.FromHsv(0, 0, 255),
                            ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                            TextColor = SKColor.FromHsv(0, 0, 255),
                        };
                        entries.Add(entry);
                    }

                };


                var chartBar = new LineChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                };

                this.ChartSize.Chart = chartBar;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.Message.ToString(), "Tamam");
            }
        }

        private void TapGestureRecognizerDia(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SalableInventoryPage(InventoryDia));
        }
        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(3000);
            await DisplayChart();
            RefreshView.IsRefreshing = false;
        }

        private async void ToolBarItemFilter_Clicked(object sender, EventArgs e)
        {
            var pageInfo = new PopUpPackedInvFilter();
            pageInfo.DataEventHandler += async (popupsender, userdata) =>
            {
                switch (userdata)
                {
                    case "Salable":
                        stat = "90";
                        break;
                    case "Waiting For Release":
                        stat = "80";
                        break;
                    case "Sterilization":
                        stat = "70";
                        break;
                    case "Waiting For Sterilization":
                        stat = "60";
                        break;
                    default:
                        stat = "All";
                        break;
                }
                chartheaderdynamic=userdata;
                if (userdata != string.Empty)
                    PageDiaSummary.Title = userdata + " Inventory";               
                await DisplayChart();
            };

            await PopupNavigation.Instance.PushAsync(pageInfo);
            //Navigation.PushPopupAsync(new PopUpPackedInvFilter());
        }
        public void DisplayDiaChartAll()
        {
            TotalDia = 0;
            InventoryDia = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("select top 9999 'Code',substring([msg_S_0002], 7, 20) 'Title',Round(sum([msg_S_1563\\T]),0) 'QTY' from dbo.StokEnvanterYonetimi('20210101',getdate(),0,'',0,0)" +
                    " where [msg_S_1563\\T]>0 and [#msg_S_0873] in (30,40,50,60,70,80,90) and ([msg_S_0001] like '4%' or [msg_S_0001] like '%PACK') and [#msg_S_0013]='PR1' Group by [msg_S_0002] Order by Round(sum([msg_S_1563\\T]),0) desc", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryDia.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });
                    TotalDia = TotalDia + Convert.ToDouble(sdr[2].ToString());
                }
                LabelDia.Text = "Total " + chartheaderdynamic + " Dialyzers: " + TotalDia.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryDia)
                {
                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(0, 0, 255),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                        TextColor = SKColor.FromHsv(0, 0, 255),
                    };
                    entries.Add(entry);
                };


                var chartBar = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                };

                this.chartViewDia.Chart = chartBar;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.Message.ToString(), "Tamam");
            }
        }
    }
}