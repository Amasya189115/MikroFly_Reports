using Microcharts;
using MikroFly_Reports.Models;
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
        public SalesDialyzerPage()
        {
            InitializeComponent();
            DisplayDiaChart();
            DisplayFluxChart();
            DisplayTypeChart();
        }
        private void DisplayDiaChart()
        {
            SoldDia = new List<GeneralTwoColumn>();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select * from [dbo].[FX_MOBILAPP_SALES_DIALYZERS_QTY] ( '20200101'  , '20230618', '%%', '', '%DIA%', '%%', '%%', '%%' )", sqlcon);

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
                LabelMode = LabelMode.None
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
                LabelMode = LabelMode.None
            };
            this.ChartTypeRatio.Chart = chartdonut;
            LabelPS.Text = "PS: %" + Math.Round((CountPS / (CountPES + CountPS) * 100), 0).ToString();
            LabelPES.Text = "PES: %" + Math.Round((CountPES / (CountPES + CountPS) * 100), 0).ToString();
            //LabelObjective.Text = Achieved.ToString();
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(3000);
            DisplayDiaChart();
            DisplayFluxChart();
            DisplayTypeChart();
            RefreshView.IsRefreshing = false;
        }
    }
}