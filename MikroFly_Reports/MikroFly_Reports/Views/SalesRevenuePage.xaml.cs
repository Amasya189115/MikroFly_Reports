using DevExpress.XamarinForms.Editors;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Microcharts;
using MikroFly_Reports.Models;
using MikroFly_Reports.Services;
using Rg.Plugins.Popup.Services;
using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Color = Xamarin.Forms.Color;
using LineChart = Microcharts.LineChart;
using PieChart = Microcharts.PieChart;

namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalesRevenuePage : ContentPage
    {
        public List<GeneralTwoColumnRevenue> Revenue;
        public List<GeneralTwoColumnRevenue> RevenuePerMonth;
        public List<GeneralTwoColumnRevenue> RevenuePerYear;
        public List<GeneralTwoColumnRevenue> RevenueLine;
        public List<ColorPalette> Palette;
        public List<ColorPalette> PaletteLinear;
        public string DateFilter = string.Empty;
        public string Month = "%%";
        public string Year= "%%";
        public string Group = "%%";
        public string IlkTarih = "'20180101'";
        public string SonTarih = "GetDate()";
        public string Country = "%%";
        public string CustomerName = "%%";
        public string ProductName = "%%";
        public string FilteredColumn = String.Empty;
        public string TitleString = "Sales Revenue of ";
        public string TitleString2 = "All Time";
        public int RevenueOrQty = 1;
        public static string CurrencyOrPcs = "€ ";
        public SalesRevenuePage()
        {
            InitializeComponent();
            ChipCountry.IsSelected = true;
            LabelSelected.Text = "Country";
            SalesRevenueValuePage.Title = TitleString + TitleString2;
            FillColors();
            FillRevenuePerProdLineChart();
            FillRevenuePerTimeChart();
            FillRevenuePerTopicChart();
        }
        private void FillColors()
        {
            Palette = new List<ColorPalette>();

            PaletteLinear = new List<ColorPalette>();
            try
            {
                Palette.Add(new ColorPalette { Hue = 206, Saturation = 41, Value = 83 });
                Palette.Add(new ColorPalette { Hue = 7, Saturation = 56, Value = 99 });
                Palette.Add(new ColorPalette { Hue = 82, Saturation = 57, Value = 88 });
                Palette.Add(new ColorPalette { Hue = 299, Saturation = 34, Value = 74 });
                Palette.Add(new ColorPalette { Hue = 33, Saturation = 65, Value = 100 });
                Palette.Add(new ColorPalette { Hue = 53, Saturation = 60, Value = 100 });
                Palette.Add(new ColorPalette { Hue = 249, Saturation = 15, Value = 86 });
                Palette.Add(new ColorPalette { Hue = 329, Saturation = 19, Value = 99 });
                Palette.Add(new ColorPalette { Hue = 170, Saturation = 34, Value = 83 });
                Palette.Add(new ColorPalette { Hue = 206, Saturation = 94, Value = 90 });

                PaletteLinear.Add(new ColorPalette { Hue = 214, Saturation = 10, Value = 93 });
                PaletteLinear.Add(new ColorPalette { Hue = 214, Saturation = 11, Value = 89 });               
                PaletteLinear.Add(new ColorPalette { Hue = 214, Saturation = 13, Value = 86 });
                PaletteLinear.Add(new ColorPalette { Hue = 216, Saturation = 14, Value = 82 });
                PaletteLinear.Add(new ColorPalette { Hue = 260, Saturation = 4, Value = 66 });
                PaletteLinear.Add(new ColorPalette { Hue = 9, Saturation = 46, Value = 87 });
                PaletteLinear.Add(new ColorPalette { Hue = 7, Saturation = 56, Value = 78 });
                PaletteLinear.Add(new ColorPalette { Hue = 5, Saturation = 68, Value = 69 });

                PaletteLinear.Add(new ColorPalette { Hue = 4, Saturation = 85, Value = 60 });
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "Ok");
            }

        }
        private void FillRevenuePerProdLineChart()
        {

            var entries = new List<ChartEntry>();
            var entries1 = new List<ChartEntry>();
            Revenue = new List<GeneralTwoColumnRevenue>();
            RevenueLine = new List<GeneralTwoColumnRevenue>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT * from FX_MOBILAPP_SALES_REVENUE("+ IlkTarih+ ","+SonTarih+","+RevenueOrQty+ ",'"+Group+"')", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    Revenue.Add(new GeneralTwoColumnRevenue
                    {
                        Name = sdr[0].ToString(),
                        Revenue_Euro = float.Parse(sdr[RevenueOrQty].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });
                }
                sdr.Close();
                sqlcon.Close();
                int i = 0;
                Label1.Text = String.Empty;
                Label2.Text = String.Empty;
                Label3.Text = String.Empty;
                Label4.Text = String.Empty;
                double TotalRevenue = 0;
                foreach (var data in Revenue)
                {
                    TotalRevenue = TotalRevenue + data.Revenue_Euro;
                }
                LabelTotalRevenue.Text = CurrencyOrPcs + String.Format("{0:N}",TotalRevenue);
                    foreach (var data in Revenue)
                {
                    var entry = new ChartEntry(data.Revenue_Euro)
                    {
                        Label = data.Name,
                        ValueLabel = data.Revenue_Euro.ToString(),
                        Color = SKColor.FromHsv(Palette[i].Hue, Palette[i].Saturation, Palette[i].Value),
                        ValueLabelColor = SKColor.FromHsv(Palette[i].Hue, Palette[i].Saturation, Palette[i].Value),
                        TextColor = SKColor.FromHsv(Palette[i].Hue, Palette[i].Saturation, Palette[i].Value),
                    };
                    var entry1 = new ChartEntry(data.Revenue_Euro)
                    {
                        Label = data.Name,
                        ValueLabel = data.Revenue_Euro.ToString(),
                        Color = SKColor.FromHsv(0, 0, 255),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                        TextColor = SKColor.FromHsv(0, 0, 255),
                    };
                    entries.Add(entry);
                    entries1.Add(entry1);

                    switch (i)
                    {
                        case 0:
                            Label1.Text=data.Name.ToString()+" %"+Math.Round((data.Revenue_Euro/TotalRevenue)*100,0).ToString();
                            Label1.TextColor = Color.FromHex("7eb0d5"); 
                            break;
                        case 1:
                            Label2.Text = data.Name.ToString() + " %" + Math.Round((data.Revenue_Euro / TotalRevenue)*100, 0).ToString();
                            Label2.TextColor = Color.FromHex("fd7f6f");
                            break;
                        case 2:
                            Label3.Text = data.Name.ToString() + " %" + Math.Round((data.Revenue_Euro / TotalRevenue)*100, 0).ToString();
                            Label3.TextColor = Color.FromHex("b2e061");
                            break;
                        case 3:
                            Label4.Text = data.Name.ToString() + " %" + Math.Round((data.Revenue_Euro / TotalRevenue)*100, 0).ToString();
                            Label4.TextColor = Color.FromHex("bd7ebe");
                            break;
                    }
                    i++;
                };

                RevenueLine = Revenue;
                var chartBar = new PieChart() 
                {
                    Entries = entries,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(Palette[i].Hue, Palette[i].Saturation, Palette[i].Value),
                    LabelMode = 0,
                };
                var chartLine = new LineChart()
                {
                    Entries = entries1,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 255),
                };

                this.PieChartViewProductTypes.Chart = chartBar;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Okey");
            }
        }
        private void FillRevenuePerTimeChart()
        {

            if(DateFilter != string.Empty)
            {
                FillRevenuePerMonthChart();
            }
            else
            {
                FillRevenuePerYearChart();
            }
        }
        private void FillRevenuePerYearChart()
        {          
            var entries1 = new List<ChartEntry>();
            Revenue = new List<GeneralTwoColumnRevenue>();
            RevenuePerYear = new List<GeneralTwoColumnRevenue>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select * from [FX_MOBILAPP_SALES_PERYEAR] ('"+Group+"')", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    Revenue.Add(new GeneralTwoColumnRevenue
                    {
                        Name = sdr[0].ToString(),
                        Revenue_Euro = float.Parse(sdr[RevenueOrQty].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });
                }
                sdr.Close();
                sqlcon.Close();
                foreach (var data in Revenue)
                {

                    var entry1 = new ChartEntry(data.Revenue_Euro)
                    {
                        Label = data.Name,
                        ValueLabel =CurrencyOrPcs + String.Format("{0:N0}", data.Revenue_Euro),
                        Color = SKColor.FromHsv(0, 0, 0),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries1.Add(entry1);
                };
                var chartLine = new LineChart()
                {
                    Entries = entries1,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };
                RevenuePerYear = Revenue;
                this.PointChartViewTime.Chart = chartLine;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }
        private void FillRevenuePerMonthChart()
        {
            //string TimeInterval = "Year(cha_tarihi) 'Year'";
            //string TimeFilter = "Year(cha_tarihi)";
            //string GroupBy = string.Empty;
            //if (DateFilter != string.Empty)
            //{
            //    TimeInterval = "Format(cha_tarihi,'MMM') 'Month'";
            //    GroupBy = " Format(cha_tarihi,'MMM'), ";
            //    TimeFilter = "Month(cha_tarihi) ";
            //}
            var entries1 = new List<ChartEntry>();
            Revenue = new List<GeneralTwoColumnRevenue>();
            RevenuePerMonth = new List<GeneralTwoColumnRevenue>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Execute [Monthly_Revenue] "+IlkTarih+","+SonTarih+ ",'"+ Group + "'", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    Revenue.Add(new GeneralTwoColumnRevenue
                    {
                        Name = sdr[0].ToString(),
                        Revenue_Euro = float.Parse(sdr[RevenueOrQty].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });
                }
                sdr.Close();
                sqlcon.Close();
                foreach (var data in Revenue)
                {

                    var entry1 = new ChartEntry(data.Revenue_Euro)
                    {
                        Label = data.Name,
                        ValueLabel = CurrencyOrPcs + String.Format("{0:N0}", data.Revenue_Euro),
                        Color = SKColor.FromHsv(0, 0, 0),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries1.Add(entry1);
                };
                var chartLine = new LineChart()
                {
                    Entries = entries1,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };
                RevenuePerMonth = Revenue;
                this.PointChartViewTime.Chart = chartLine;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }
        private void FillRevenuePerTopicChart()
        {
            var entries1 = new List<ChartEntry>();
            Revenue = new List<GeneralTwoColumnRevenue>();
            string sqlcom = string.Empty;
            if(LabelSelected.Text=="Country")
            {
                sqlcom = "Select * From [dbo].[FX_MOBILAPP_SALES_TOP10COUNTRY] ( "+IlkTarih+"  ,"+SonTarih+","+RevenueOrQty+",'"+Group+"' )";
            }

            if(LabelSelected.Text == "Customer")
            {
                sqlcom = "Select * From [dbo].[FX_MOBILAPP_SALES_TOP10CUSTOMER] ( "+IlkTarih+"  ,"+SonTarih+","+RevenueOrQty+ ",'"+Group+"'  )";
            }

            if(LabelSelected.Text == "Type")
            {
                sqlcom = "Select * From [dbo].[FX_MOBILAPP_SALES_TOP10PRODUCT] ( "+IlkTarih+"  ,"+SonTarih+","+RevenueOrQty+ ",'"+Group+"'  )";
            }

            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                SqlCommand sqlcommand = new SqlCommand(sqlcom, sqlcon);
                SqlDataReader sdr = sqlcommand.ExecuteReader();

                while (sdr.Read())
                {
                    Revenue.Add(new GeneralTwoColumnRevenue
                    {
                        Name = sdr[0].ToString(),
                        Revenue_Euro = float.Parse(sdr[RevenueOrQty].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                    });
                }
                sdr.Close();
                sqlcon.Close();
                foreach (var data in Revenue)
                {

                    var entry1 = new ChartEntry(data.Revenue_Euro)
                    {
                        Label = data.Name,
                        ValueLabel = CurrencyOrPcs + String.Format("{0:N0}", data.Revenue_Euro),
                        Color = SKColor.FromHsv(0, 0, 0),
                        ValueLabelColor = SKColor.FromHsv(0, 0, 0),
                        TextColor = SKColor.FromHsv(0, 0, 0),
                    };
                    entries1.Add(entry1);
                };
                var chartLine = new LineChart()
                {
                    Entries = entries1,
                    LabelTextSize = 25,
                    LabelColor = SKColor.FromHsv(0, 0, 0),
                };

                this.PointChartViewTopic.Chart = chartLine;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", "Connection Failed", "Tamam");
            }
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(2000);
            FillRevenuePerProdLineChart();
            FillRevenuePerTimeChart();
            FillRevenuePerTopicChart();
            RefreshView.IsRefreshing = false;
        }

        private void Chip_Tap(object sender, System.ComponentModel.HandledEventArgs e)
        {
            LabelSelected.Text=(sender as Chip).Text;
            FillRevenuePerTopicChart();
        }

        private async void ToolBartoDateFilter_Clicked(object sender, EventArgs e)
        {
            try
            {
                DateFilter = String.Empty;
                var pageInfo = new PopUpSalesDateFilter();
                pageInfo.DateEventHandler += async (popupsender, userdata) =>
                {
                    
                    if(userdata!= "All Years")
                    { DateFilter =  userdata;
                        TitleString2 = userdata;
                        SalesRevenueValuePage.Title = TitleString + TitleString2;
                        Year = userdata;
                        IlkTarih = "'"+userdata+"0101'";
                        SonTarih = "'" + userdata + "1231'";
                    }
                    else
                    {
                        TitleString2 = userdata;
                        SalesRevenueValuePage.Title = TitleString + TitleString2;
                        Year = string.Empty;
                        IlkTarih = "'20180101'";
                        SonTarih = "GetDate()";    
                    }


                };

                pageInfo.ProductGroupEventHandler += async (popupsender, userdata) =>
                {
                    Group="%"+userdata+"%";
                    FillRevenuePerProdLineChart();
                    FillRevenuePerTimeChart();
                    FillRevenuePerTopicChart();
                };



                await PopupNavigation.Instance.PushAsync(pageInfo);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }


        }

        private void TapGestureRecognizer_PointChartViewTopic(object sender, EventArgs e)
        {
            FilteredColumn = LabelSelected.Text;
            Navigation.PushAsync(new SalesRevenueDetailPage(Revenue,Month,Year,Group,Country,CustomerName,ProductName,IlkTarih,SonTarih, FilteredColumn));
        }

        private void TapGestureRecognizer_PointChartViewTime(object sender, EventArgs e)
        {
            if (DateFilter != string.Empty)
            {
                FilteredColumn = "Month";
                Navigation.PushAsync(new SalesRevenueDetailPage(RevenuePerMonth,Month, Year, Group, Country, CustomerName, ProductName, IlkTarih, SonTarih, FilteredColumn));
            }
            else
            {
                FilteredColumn = "Year";
                Navigation.PushAsync(new SalesRevenueDetailPage(RevenuePerYear,Month, Year, Group, Country, CustomerName, ProductName, IlkTarih, SonTarih, FilteredColumn));
            }

        }

        private void TapGestureRecognizer_Pie(object sender, EventArgs e)
        {
            FilteredColumn = "Group";
            Navigation.PushAsync(new SalesRevenueDetailPage(RevenueLine,Month, Year, Group, Country, CustomerName, ProductName, IlkTarih, SonTarih,FilteredColumn));
        }

        private void SwitchQtytoRevenue_Toggled(object sender, ToggledEventArgs e)
        {
            if (RevenueOrQty == 1)
            {
                CurrencyOrPcs = string.Empty;
                RevenueOrQty = 2;
                TitleString = "Sales Quantity of ";
                SalesRevenueValuePage.Title = TitleString + TitleString2;
            }                
            else
            {
                CurrencyOrPcs = "€ ";  
                RevenueOrQty = 1;
                TitleString = "Sales Revenue of ";
                SalesRevenueValuePage.Title = TitleString + TitleString2;
            }
                
            FillRevenuePerProdLineChart();
            FillRevenuePerTimeChart();
            FillRevenuePerTopicChart();
        }
    }
}