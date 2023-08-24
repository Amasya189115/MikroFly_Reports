using DocumentFormat.OpenXml.Bibliography;
using Microcharts;
using MikroFly_Reports.Models;
using Rg.Plugins.Popup.Services;
using SkiaSharp;
using System;
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
    public partial class SalesDialyzerPage : ContentPage
    {
        public List<GeneralTwoColumn> SoldDia;
        public List<Sqm> sizes;
        public float TotalDia = 0;
        public string DateFilter = string.Empty;
        public string IlkTarih = "'20180101'";
        public string SonTarih = "GetDate()";
        public string TitleString = "Dialyzer Sales of ";
        public string TitleString2 = "All Times";
        public SalesDialyzerPage()
        {
            InitializeComponent();
            DisplayDiaChart(IlkTarih,SonTarih);
            DisplayFluxChart();
            DisplayTypeChart();
            DisplaySizeChart();
        }
        private void DisplayDiaChart(string firstdate, string lastdate)
        {
            SoldDia = new List<GeneralTwoColumn>();
            var entries = new List<ChartEntry>();
            TotalDia = 0;
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select * from [dbo].[FX_MOBILAPP_SALES_DIALYZERS_QTY] ( "+firstdate+"  , "+lastdate+", '%%', '', '%DIA%', '%%', '%%', '%%' )", sqlcon);

                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    SoldDia.Add(new GeneralTwoColumn
                    {
                        Name = sdr[0].ToString(),
                        Qty = float.Parse(sdr[1].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });
                }
                sdr.Close();
                sqlcon.Close();

                foreach (var data in SoldDia)
                {

                    var entry = new ChartEntry(data.Qty)
                    {
                        Label = data.Name,
                        ValueLabel = data.Qty.ToString(),
                        Color = SKColor.FromHsv(0, 0, 0),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries.Add(entry);
                    TotalDia = TotalDia + data.Qty;
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
            foreach (var item in SoldDia)
            {
                if (item.Name.Substring(item.Name.IndexOf("S")).Remove(item.Name.Substring(item.Name.IndexOf("S")).Length - 1).Length == 3)
                {
                    
                    CountLF = CountLF + item.Qty;
                }
                else
                {
                    CountHF = CountHF + item.Qty;
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
                LabelMode = LabelMode.None,
                LabelColor = SKColor.FromHsv(0, 0, 0),
            };
            this.ChartFluxRatio.Chart = chartdonut;
            LabelHF.Text = "HF: %" + Math.Round((CountHF / (CountLF + CountHF) * 100), 0).ToString();
            LabelLF.Text = "LF: %" + Math.Round((CountLF / (CountLF + CountHF) * 100), 0).ToString();
        }
        private void DisplayTypeChart()
        {
            float CountPS = 0;
            float CountPES = 0;
            foreach (var item in SoldDia)
            {
                if (item.Name.Contains("PS"))
                {
                    CountPS = CountPS + item.Qty;
                }
                else
                {
                    CountPES = CountPES + item.Qty;
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
                LabelMode = LabelMode.None,
                LabelColor = SKColor.FromHsv(0, 0, 0),
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
                foreach (var item in SoldDia)
                {
                    foreach (var data in sizes)
                    {
                        if (item.Name.Substring(item.Name.IndexOf("S") + 1, 2) == data.Dimens)
                        {
                            check = true;
                            data.Quantity = data.Quantity + item.Qty;
                        }

                    }
                    if (check)
                    {

                    }
                    else
                    {
                        sizes.Add(new Sqm
                        {
                            Dimens = item.Name.Substring(item.Name.IndexOf("S") + 1, 2),
                            Quantity = item.Qty,
                        });
                    }
                    check = false;
                };
                foreach (var data in sizes)
                {
                    {
                        Random ran = new Random();
                        SKColor randomColor = SKColor.FromHsv(0, 0, 0);

                        var entry = new ChartEntry(data.Quantity)
                        {
                            Label = data.Dimens,
                            ValueLabel = "% " + Math.Round((data.Quantity / TotalDia) * 100, 0).ToString(),
                            Color = SKColor.FromHsv(0, 0, 0),
                            ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                            TextColor = SKColor.FromHsv(0, 0, 0),
                        };
                        entries.Add(entry);
                    }

                };


                var chartBar = new LineChart()
                {
                    Entries = entries,
                    LabelTextSize = 35,
                    LabelColor = SKColor.FromHsv(0, 0, 0),

                };

                this.ChartSize.Chart = chartBar;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.Message.ToString(), "Tamam");
            }
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(3000);
            DisplayDiaChart(IlkTarih, SonTarih);
            DisplayFluxChart();
            DisplayTypeChart();
            DisplaySizeChart();
            RefreshView.IsRefreshing = false;
        }
        private async void ToolBartoDateFilter_Clicked(object sender, EventArgs e)
        {
            try
            {
                DateFilter = String.Empty;
                var pageInfo = new PopUpSalesDateFilter();
                pageInfo.DateEventHandler += async (popupsender, userdata) =>
                {

                    if (userdata != "All Years")
                    {
                        DateFilter = userdata;
                        TitleString2 = userdata;
                        SalesDialyzerAnalysisPage.Title = TitleString + TitleString2;

                        IlkTarih = "'" + userdata + "0101'";
                        SonTarih = "'" + userdata + "1231'";
                    }
                    else
                    {
                        TitleString2 = userdata;
                        SalesDialyzerAnalysisPage.Title = TitleString + TitleString2;

                        IlkTarih = "'20180101'";
                        SonTarih = "GetDate()";
                    }
                    DisplayDiaChart(IlkTarih, SonTarih);
                    DisplayFluxChart();
                    DisplayTypeChart();
                    DisplaySizeChart();

                };



                await PopupNavigation.Instance.PushAsync(pageInfo);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}