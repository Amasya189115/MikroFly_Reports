using DocumentFormat.OpenXml.Packaging;
using Microcharts;
using MikroFly_Reports.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Color = System.Drawing.Color;

namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SummaryPage : ContentPage
    {
        public string chartheadersdynamic="Salable";
        double TotalDia;
        double TotalBlo;
        double TotalNeedle;
        double TotalPow;
        public List<SalableInventory> InventoryDia;
        public List<SalableInventory> InventoryBlo;
        public List<SalableInventory> InventoryPow;
        public List<SalableInventory> InventoryNeedle;
        public string status;
        public static string pagetitles = "Salable";
        public void DisplayDiaChart(string statu)
        {
            TotalDia = 0;
            InventoryDia = new List<SalableInventory>();
            var entries=new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT TOP 100 PERCENT sto_kod AS 'Code',substring(sto_isim,7,20) AS 'Title', Round(dbo.fn_DepodakiMiktar(sto_kod,"+statu+" ,getdate()),0) 'QTY' "+
                    "FROM dbo.STOKLAR WITH (NOLOCK) where sto_anagrup_kod in ('PR1') and sto_kod like '4%' AND dbo.fn_DepodakiMiktar(sto_kod,"+statu+ " ,getdate())>0 ORDER BY Round(dbo.fn_DepodakiMiktar(sto_kod,"+statu+" ,getdate()),0)  desc", sqlcon);
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
                LabelDia.Text = chartheadersdynamic + " Dialyzers: "+TotalDia.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach(var data in InventoryDia)
                {
                    Random ran=new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(347, 89, 78),
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

                this.chartViewDia.Chart = chartBar;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }

        public void DisplayBloChart(string statu)
        {
            TotalBlo = 0;
            InventoryBlo = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT TOP 100 PERCENT sto_kod AS 'Code', substring(sto_isim,7,20) AS 'Title',Round(dbo.fn_DepodakiMiktar(sto_kod,"+statu+" ,getdate()),0) 'QTY' FROM dbo.STOKLAR WITH (NOLOCK) " +
                    "where sto_anagrup_kod in ('PR2') and sto_kod like '4%' AND dbo.fn_DepodakiMiktar(sto_kod,"+statu+" ,getdate())>0 and sto_isim not like '%need%' ORDER BY dbo.fn_DepodakiMiktar(sto_kod,90 ,getdate()) desc", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryBlo.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    });
                    TotalBlo = TotalBlo + Convert.ToDouble(sdr[2].ToString());
                }
                LabelBlo.Text= chartheadersdynamic + "  Bloodlines: " + TotalBlo.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryBlo)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(347, 89, 78),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries.Add(entry);
                };


                var chartPie = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };

                this.chartViewLine.Chart = chartPie;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }

        public void DisplayPowChart(string statu)
        {
            TotalPow = 0;
            InventoryPow = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT TOP 100 PERCENT sto_kod AS 'Code',substring(sto_isim,7,20) AS 'Title',Round(dbo.fn_DepodakiMiktar(sto_kod,"+statu+" ,getdate()),0) 'QTY' FROM dbo.STOKLAR WITH (NOLOCK)" +
                    "where sto_anagrup_kod in ('PR3') and sto_kod like '4%' AND dbo.fn_DepodakiMiktar(sto_kod,90 ,getdate())>0 ORDER BY dbo.fn_DepodakiMiktar(sto_kod,"+statu+" ,getdate()) desc", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryPow.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    });
                    TotalPow = TotalPow + Convert.ToDouble(sdr[2].ToString());
                }
                LabelPow.Text = chartheadersdynamic + "  Powders: " + TotalPow.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryPow)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(347, 89, 78),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor=randomColor,
                    };
                    entries.Add(entry);
                };


                var chartPie = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };

                this.chartViewPowder.Chart = chartPie;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }

        public void DisplayNeedleChart(string statu)
        {
            TotalNeedle = 0;
            InventoryNeedle = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT TOP 100 PERCENT sto_kod AS 'Code', substring(sto_isim,7,20) AS 'Title',Round(dbo.fn_DepodakiMiktar(sto_kod,"+statu+" ,getdate()),0) 'QTY' FROM dbo.STOKLAR WITH (NOLOCK)" +
                    "where sto_anagrup_kod in ('PR2') and sto_kod like '4%' AND dbo.fn_DepodakiMiktar(sto_kod,"+statu+" ,getdate())>0 and sto_isim like '%need%' ORDER BY dbo.fn_DepodakiMiktar(sto_kod,90 ,getdate()) desc", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryNeedle.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    });
                    TotalNeedle=TotalNeedle+Convert.ToDouble(sdr[2].ToString());
                }
                LabelNeedle.Text = chartheadersdynamic + "  Needles: " + TotalNeedle.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryNeedle)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(347, 89, 78),
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

                this.chartViewNeedle.Chart = chartBar;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }

        public async Task DisplayCharts()
        {
            if (status != "All")
            {
                if(status=="90" || status=="80")
                {

                    DisplayDiaChart(status);
                    DisplayBloChart(status);
                    DisplayNeedleChart(status);
                    if(status=="90")
                    {

                        DisplayPowChart(status);
                    }
                    else
                    {

                        DisplayPowChartNotReleased();
                    }
                }
                else
                {
                    if(status=="60")
                    {

                        DisplayDiaChartNonSteril(status);
                        DisplayBloChartNonSteril(status);
                        DisplayNeedleChartNonSteril(status);
                    }
                    else
                    {

                        DisplayDiaChartSterilizationProcess(status);
                        DisplayBloChartSterilizationProcess(status);
                        DisplayNeedleChartSterilizationProcess(status);
                    }
                }
            }
            else
            {

                DisplayDiaChartAll();
                DisplayBloChartAll();
                DisplayNeedleChartAll();
                DisplayPowChartAll();
            }
        }
        public SummaryPage()
        {           
            InitializeComponent();
            status = "90";
            DisplayCharts();
        }
        private async void ToolBarItemFilter_Clicked(object sender, EventArgs e)
        {
            var pageInfo = new PopUpPackedInvFilter();
            pageInfo.DataEventHandler += async (popupsender,userdata) =>
            {
                switch (userdata)
                {
                    case "Salable":
                        status = "90";
                        break;
                    case "Waiting For Release":
                        status = "80";
                        break;
                    case "Sterilization":
                        status = "70";
                        break;
                    case "Waiting For Sterilization":
                        status = "60";
                        break;
                    default:
                        status = "All";
                        break;
                }
                chartheadersdynamic = userdata;
                await DisplayCharts();
                if(userdata != string.Empty)
                PageSummery.Title = userdata+" Inventory";

            };

            await PopupNavigation.Instance.PushAsync(pageInfo);
            //Navigation.PushPopupAsync(new PopUpPackedInvFilter());
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(2000);
            InventoryDia.Clear();
            InventoryBlo.Clear();
            InventoryNeedle.Clear();
            InventoryPow.Clear();
            await DisplayCharts();
            RefreshView.IsRefreshing = false;
        }

        private void TapGestureRecognizerDia(object sender, EventArgs e)
        {
            pagetitles = Title;
            Navigation.PushAsync(new SalableInventoryPage(InventoryDia));
        }
        private void TapGestureRecognizerBlo(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SalableInventoryPage(InventoryBlo));
        }
        private void TapGestureRecognizerPow(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SalableInventoryPage(InventoryPow));
        }
        private void TapGestureRecognizerNeedle(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SalableInventoryPage(InventoryNeedle));
        }

        public void DisplayDiaChartNonSteril(string statu)
        {
            TotalDia = 0;
            InventoryDia = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT TOP 100 PERCENT sto_kod AS 'Code',substring(sto_isim,7,20) AS 'Title', Round((dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,30 ,getdate())),0) 'QTY' " +
                    "FROM dbo.STOKLAR WITH (NOLOCK) where sto_anagrup_kod in ('PR1') and (sto_kod like '4%' or sto_kod like '%PACK') AND Round((dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,30 ,getdate())),0)>0 ORDER BY Round((dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,30 ,getdate())),0) desc", sqlcon);
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
                LabelDia.Text = chartheadersdynamic + " Dialyzers: " + TotalDia.ToString();
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
                        Color = SKColor.FromHsv(347, 89, 78),
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

                this.chartViewDia.Chart = chartBar;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }
        public void DisplayBloChartNonSteril(string statu)
        {
            TotalBlo = 0;
            InventoryBlo = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT TOP 100 PERCENT sto_kod AS 'Code', substring(sto_isim,7,20) AS 'Title',Round((dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,40 ,getdate())),0) 'QTY' FROM dbo.STOKLAR WITH (NOLOCK) " +
                    "where sto_anagrup_kod in ('PR2') and (sto_kod like '4%' or sto_kod like '%PACK') AND Round((dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,40 ,getdate())),0)>0 and sto_isim not like '%need%' ORDER BY Round((dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,40 ,getdate())),0) desc", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryBlo.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    });
                    TotalBlo = TotalBlo + Convert.ToDouble(sdr[2].ToString());
                }
                LabelBlo.Text = chartheadersdynamic + "  Bloodlines: " + TotalBlo.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryBlo)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(347, 89, 78),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries.Add(entry);
                };


                var chartPie = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };

                this.chartViewLine.Chart = chartPie;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }
        public void DisplayNeedleChartNonSteril(string statu)
        {
            TotalNeedle = 0;
            InventoryNeedle = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT TOP 100 PERCENT sto_kod AS 'Code', substring(sto_isim,7,20) AS 'Title',Round((dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,40 ,getdate())),0) 'QTY' FROM dbo.STOKLAR WITH (NOLOCK)" +
                    "where sto_anagrup_kod in ('PR2') and (sto_kod like '4%' or sto_kod like '%PACK') AND Round((dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,40 ,getdate())),0)>0 and sto_isim like '%need%' ORDER BY Round((dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,40 ,getdate())),0) desc", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryNeedle.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    });
                    TotalNeedle = TotalNeedle + Convert.ToDouble(sdr[2].ToString());
                }
                LabelNeedle.Text = chartheadersdynamic + "  Needles: " + TotalNeedle.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryNeedle)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(347, 89, 78),
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

                this.chartViewNeedle.Chart = chartBar;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }
        public void DisplayPowChartNotReleased()
        {
            TotalPow = 0;
            InventoryPow = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT TOP 100 PERCENT sto_kod AS 'Code',substring(sto_isim,7,20) AS 'Title',Round((dbo.fn_DepodakiMiktar(sto_kod,60 ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,50 ,getdate())),0) 'QTY' FROM dbo.STOKLAR WITH (NOLOCK)" +
                    "where sto_anagrup_kod in ('PR3') and (sto_kod like '4%' or sto_kod like '%PACK') AND Round((dbo.fn_DepodakiMiktar(sto_kod,60 ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,50 ,getdate())),0)>0 ORDER BY Round((dbo.fn_DepodakiMiktar(sto_kod,60 ,getdate())+dbo.fn_DepodakiMiktar(sto_kod,50 ,getdate())),0) desc", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryPow.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    });
                    TotalPow = TotalPow + Convert.ToDouble(sdr[2].ToString());
                }
                LabelPow.Text = chartheadersdynamic + "  Powders: " + TotalPow.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryPow)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(347, 89, 78),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries.Add(entry);
                };


                var chartPie = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };

                this.chartViewPowder.Chart = chartPie;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }
        public void DisplayDiaChartSterilizationProcess(string statu)
        {
            TotalDia = 0;
            InventoryDia = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT TOP 100 PERCENT sto_kod AS 'Code',substring(sto_isim,7,20) AS 'Title', Round((dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())),0) 'QTY' " +
                    "FROM dbo.STOKLAR WITH (NOLOCK) where sto_anagrup_kod in ('PR1') and (sto_kod like '4%' or sto_kod like '%PACK') AND dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())>0 ORDER BY Round((dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())),0) desc", sqlcon);
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
                LabelDia.Text = chartheadersdynamic + " Dialyzers: " + TotalDia.ToString();
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
                        Color = SKColor.FromHsv(347, 89, 78),
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

                this.chartViewDia.Chart = chartBar;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }
        public void DisplayBloChartSterilizationProcess(string statu)
        {
            TotalBlo = 0;
            InventoryBlo = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT TOP 100 PERCENT sto_kod AS 'Code', substring(sto_isim,7,20) AS 'Title',Round((dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())),0) 'QTY' FROM dbo.STOKLAR WITH (NOLOCK) " +
                    "where sto_anagrup_kod in ('PR2') and (sto_kod like '4%' or sto_kod like '%PACK') AND dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())>0 and sto_isim not like '%need%' ORDER BY dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate()) desc", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryBlo.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    });
                    TotalBlo = TotalBlo + Convert.ToDouble(sdr[2].ToString());
                }
                LabelBlo.Text = chartheadersdynamic + "  Bloodlines: " + TotalBlo.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryBlo)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(347, 89, 78),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries.Add(entry);
                };


                var chartPie = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };

                this.chartViewLine.Chart = chartPie;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }
        public void DisplayNeedleChartSterilizationProcess(string statu)
        {
            TotalNeedle = 0;
            InventoryNeedle = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT TOP 100 PERCENT sto_kod AS 'Code', substring(sto_isim,7,20) AS 'Title',Round((dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())),0) 'QTY' FROM dbo.STOKLAR WITH (NOLOCK)" +
                    "where sto_anagrup_kod in ('PR2') and (sto_kod like '4%' or sto_kod like '%PACK') AND dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate())>0 and sto_isim like '%need%' ORDER BY dbo.fn_DepodakiMiktar(sto_kod," + statu + " ,getdate()) desc", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryNeedle.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    });
                    TotalNeedle = TotalNeedle + Convert.ToDouble(sdr[2].ToString());
                }
                LabelNeedle.Text = chartheadersdynamic + "  Needles: " + TotalNeedle.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryNeedle)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        Color = SKColor.FromHsv(347, 89, 78),
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

                this.chartViewNeedle.Chart = chartBar;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
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
                LabelDia.Text = chartheadersdynamic + " Dialyzers: " + TotalDia.ToString();
                sdr.Close();
                sqlcon.Close();
                //List<int> i = new List<int>();
                //List<int> j = new List<int>();
                //List<int> k = new List<int>();
                //for (int z=0;z<5;z++)
                //{
                //    i
                //}
                int i = 0;
                foreach (var data in InventoryDia)
                {
                    i = i + 20;
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        //Color = SKColor.FromHsv(347, 89, 78),
                        //ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        //TextColor = SKColor.FromHsv(0, 0, 0),
                        Color = SKColor.FromHsv(347, 89, 78),
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

                this.chartViewDia.Chart = chartBar;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }
        public void DisplayBloChartAll()
        {
            TotalBlo = 0;
            InventoryBlo = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("select top 9999 'Code',substring([msg_S_0002], 7, 20) 'Title',Round(sum([msg_S_1563\\T]),0) 'QTY' from dbo.StokEnvanterYonetimi('20210101',getdate(),0,'',0,0)" +
                    " where [msg_S_1563\\T]>0 and [#msg_S_0873] in (30,40,50,60,70,80,90) and ([msg_S_0001] like '4%' or [msg_S_0001] like '%PACK') and [#msg_S_0013]='PR2' and [msg_S_0002] not like '%Need%' Group by [msg_S_0002] Order by Round(sum([msg_S_1563\\T]),0) desc", sqlcon); SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryBlo.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    });
                    TotalBlo = TotalBlo + Convert.ToDouble(sdr[2].ToString());
                }
                LabelBlo.Text = chartheadersdynamic + "  Bloodlines: " + TotalBlo.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryBlo)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        //Color = SKColor.FromHsv(347, 89, 78),
                        //ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        //TextColor = SKColor.FromHsv(0, 0, 0),
                        Color = SKColor.FromHsv(347, 89, 78),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries.Add(entry);
                };


                var chartPie = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };

                this.chartViewLine.Chart = chartPie;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }
        public void DisplayNeedleChartAll()
        {
            TotalNeedle = 0;
            InventoryNeedle = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("select top 9999 'Code',substring([msg_S_0002], 7, 20) 'Title',Round(sum([msg_S_1563\\T]),0) 'QTY' from dbo.StokEnvanterYonetimi('20210101',getdate(),0,'',0,0)" +
                    " where [msg_S_1563\\T]>0 and [#msg_S_0873] in (30,40,50,60,70,80,90) and ([msg_S_0001] like '4%' or [msg_S_0001] like '%PACK') and [#msg_S_0013]='PR2' and [msg_S_0002] like '%Need%' Group by [msg_S_0002] Order by Round(sum([msg_S_1563\\T]),0) desc", sqlcon); 
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryNeedle.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    });
                    TotalNeedle = TotalNeedle + Convert.ToDouble(sdr[2].ToString());
                }
                LabelNeedle.Text = chartheadersdynamic + "  Needles: " + TotalNeedle.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryNeedle)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        //Color = SKColor.FromHsv(347, 89, 78),
                        //ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        //TextColor = SKColor.FromHsv(0, 0, 0),
                        Color = SKColor.FromHsv(347, 89, 78),
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

                this.chartViewNeedle.Chart = chartBar;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }
        public void DisplayPowChartAll()
        {
            TotalPow = 0;
            InventoryPow = new List<SalableInventory>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("select top 9999 'Code',substring([msg_S_0002], 7, 20) 'Title',Round(sum([msg_S_1563\\T]),0) 'QTY' from dbo.StokEnvanterYonetimi('20210101',getdate(),0,'',0,0)" +
                    " where [msg_S_1563\\T]>0 and [#msg_S_0873] in (30,40,50,60,70,80,90) and ([msg_S_0001] like '4%' or [msg_S_0001] like '%PACK') and [#msg_S_0013]='PR3' Group by [msg_S_0002] Order by Round(sum([msg_S_1563\\T]),0) desc", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    InventoryPow.Add(new SalableInventory
                    {
                        Code = sdr[0].ToString(),
                        Title = sdr[1].ToString(),
                        QTY = float.Parse(sdr[2].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    });
                    TotalPow = TotalPow + Convert.ToDouble(sdr[2].ToString());
                }
                LabelPow.Text = chartheadersdynamic + "  Powders: " + TotalPow.ToString();
                sdr.Close();
                sqlcon.Close();

                foreach (var data in InventoryPow)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.QTY)
                    {
                        Label = data.Title,
                        ValueLabel = data.QTY.ToString(),
                        //Color = SKColor.FromHsv(347, 89, 78),
                        //ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        //TextColor = SKColor.FromHsv(0, 0, 0),
                        Color = SKColor.FromHsv(347, 89, 78),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries.Add(entry);
                };


                var chartPie = new BarChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };

                this.chartViewPowder.Chart = chartPie;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }
    }



}