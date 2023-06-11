using DevExpress.XamarinForms.DataGrid;
using Microcharts;
using MikroFly_Reports.Models;
using MikroFly_Reports.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaProdDetailsPage : ContentPage
    {
        int count;
        public Objectives Objectives { get; set; }
        public List<Operators> Operators { get; set; }
        public List<MonthlyProduction> MonthlyProductions { get; set; }
        public List<MonthlyProductionWithProducts> MonthlyProductionWithProduct { get; set; }
        string module;
        float Achieved;
        string OpCode;
        private ExcelServices excelService;
        public DiaProdDetailsPage(string machine)
        {
            InitializeComponent();
            excelService = new ExcelServices();
            MonthlyProductions = new List<MonthlyProduction>();
            MonthlyProductionWithProduct=new List<MonthlyProductionWithProducts>();
            ChangeTitle();
            module = machine;
            GetObjectives();
            GetAchievements();
            CreateChart();
            GetMonthlyProduction();
            GetMonthlyProductionWithProducts();
            ChangeTitle();
            ConvertOperationCode();
            GetInstantData();
        }
        private void GetInstantData()
        {
            Operators = new List<Operators>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcomM1 = new SqlCommand("select OpT_IsEmriKodu 'JobOrder',sto_isim 'Code',per_adi+' '+per_soyadi 'Operator' from URETIM_OPERASYON_HAREKETLERI" +
                    " inner join PERSONELLER on URETIM_OPERASYON_HAREKETLERI.OpT_PersonelKodu=PERSONELLER.per_kod" +
                    " inner join STOKLAR on STOKLAR.sto_kod=URETIM_OPERASYON_HAREKETLERI.OpT_UrunKodu WHERE OpT_bitis_tarihi<'19000101' and OpT_OperasyonKodu='"+OpCode+"'", sqlcon);
                SqlDataReader sdr = sqlcomM1.ExecuteReader();
                while (sdr.Read())
                {
                    LabelJobOrder.Text = sdr[0].ToString();
                    LabelProduct.Text = sdr[1].ToString();
                    LabelOperator.Text = sdr[2].ToString();
                }
                sdr.Close();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.ToString(), "Tamam");
            }
            
        }
        private void ConvertOperationCode()
        {
            switch (module)
            {
                case "M1":
                    OpCode = "3-M10";
                    break;
                case "M2":
                    OpCode = "3-M20";
                    break;
                case "M3":
                    OpCode = "3-M30";
                    break;
                case "M4":
                    OpCode = "3-M40";
                    break;
                case "M5":
                    OpCode = "3-M50";
                    break;
                default:
                    OpCode = "3-M60";
                    break;
            }
        }
        private void ChangeTitle()
        {
            string Add = string.Empty;
            switch (module)
            {
                case "M1":
                    Add="Module1";
                    break;
                case "M2":
                    Add = "Module2";
                    break;
                case "M3":
                    Add = "Module3";
                    break;
                case "M4":
                    Add = "Module4";
                    break;
                case "M5":
                    Add = "Module5";
                    break;
                default:
                    Add = "Packing";
                    break;
            }
            Title = "Work Center Stats:  "+Add;
        }
        private void GetAchievements()
        {           
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcomM1 = new SqlCommand("select Isnull(sum(sth_miktar),0) from STOK_HAREKETLERI inner join STOKLAR on sto_kod=sth_stok_kod" +
                    " where YEAR(sth_create_date) =YEAR(GETDATE()) and sth_stok_kod like '%" + module+"' and sth_evraktip=7" +
                    " and sth_tip=0 and sth_cins=7 and sto_anagrup_kod='PR1'", sqlcon);
                Achieved = (float)Math.Round(Convert.ToDouble(sqlcomM1.ExecuteScalar().ToString()),0);
            }
            catch(Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.ToString(), "Tamam");
            }
        }
        private void GetObjectives()
        {
            Objectives = new Objectives();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[DIA_URETIM_HEDEFLERI]", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    Objectives.Dialyzers = int.Parse(sdr[0].ToString());
                    Objectives.Bloodllines = int.Parse(sdr[1].ToString());
                    Objectives.Powders = int.Parse(sdr[2].ToString());
                    Objectives.Needles = int.Parse(sdr[3].ToString());
                }
                sdr.Close();
                sqlcon.Close();
            }
            catch(Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı",ex.ToString(),"Tamam");
            }
        }
        private void CreateChart()
        {
            var entries = new List<ChartEntry>();

            var entry = new ChartEntry(Objectives.Dialyzers- Achieved)
            {
                Label = "Realization",
                ValueLabel = (Objectives.Dialyzers - Achieved).ToString(),
                Color = SKColor.FromHsv(0,0,255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };


            var entry1 = new ChartEntry(Achieved)
            {
                Label = "Realization",
                ValueLabel = (Achieved).ToString(),
                Color = SKColor.FromHsv(347, 89, 78),
                ValueLabelColor = SKColor.FromHsv(347, 89, 78),
                TextColor = SKColor.FromHsv(347, 89, 78),
            };
            entries.Add(entry);
            entries.Add(entry1);

            var chartdonut = new DonutChart()
            {
                Entries = entries,
                LabelTextSize = 25,
                LabelMode = 0
            };

            this.ChartDiaAchievement.Chart = chartdonut;
            LabelPieChartHeader.Text = DateTime.Now.Year.ToString() +" Plan: " + Objectives.Dialyzers.ToString();
            LabelObjective.Text = Achieved.ToString();
        }
        private void GetMonthlyProduction()
        {
            MonthlyProductions.Clear();
            var entries = new List<ChartEntry>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("select FORMAT(sth_create_date, 'MMMM'),MONTH(sth_create_date), Isnull(sum(sth_miktar),0) from STOK_HAREKETLERI inner join STOKLAR on sto_kod=sth_stok_kod" +
                    " where Year(sth_create_date) =Year(GETDATE()) and sth_stok_kod like '%"+module+"' and sth_evraktip=7 and sth_tip=0 and sth_cins=7 and sto_anagrup_kod='PR1'" +
                    " GROUP BY FORMAT(sth_create_date, 'MMMM'),MONTH(sth_create_date) order by MONTH(sth_create_date)", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    MonthlyProductions.Add(new MonthlyProduction
                    {
                        month = sdr[0].ToString(),
                        value = (float)(Math.Round(Convert.ToDouble(sdr[2].ToString())))
                    });
                }
                sdr.Close();
                sqlcon.Close();
                foreach (var data in MonthlyProductions)
                {
                    Random ran = new Random();
                    SKColor randomColor = SKColor.FromHsv(ran.Next(256), ran.Next(256), ran.Next(256));

                    var entry = new ChartEntry(data.value)
                    {
                        Label = data.month,
                        ValueLabel = data.value.ToString(),
                        Color = randomColor,
                        ValueLabelColor = randomColor,
                        TextColor = randomColor,
                    };
                    entries.Add(entry);
                };


                var chartPie = new LineChart()
                {
                    Entries = entries,
                    LabelTextSize = 25,
                    LabelOrientation=Orientation.Horizontal
                };

                this.ChartMonthlyProd.Chart = chartPie;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.ToString(), "Tamam");
            }
        }
        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(3000);
            GetObjectives();
            GetAchievements();
            CreateChart();
            GetMonthlyProduction();
            GetMonthlyProductionWithProducts();
            ChangeTitle();
            ConvertOperationCode();
            GetInstantData();
            RefreshView.IsRefreshing = false;
        }
        private void GetMonthlyProductionWithProducts()
        {
            MonthlyProductionWithProduct.Clear();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("select FORMAT(sth_create_date, 'MMMM') 'Month',sto_isim 'Desc',sth_parti_kodu 'LotNu', Isnull(sum(sth_miktar),0) 'Quantity' from STOK_HAREKETLERI inner join STOKLAR on sto_kod=sth_stok_kod" +
                    " where Year(sth_create_date) =Year(GETDATE()) and sth_stok_kod like '%" + module + "' and sth_evraktip=7 and sth_tip=0 and sth_cins=7 and sto_anagrup_kod='PR1'" +
                    " GROUP BY FORMAT(sth_create_date, 'MMMM'),MONTH(sth_create_date),sto_isim,sth_parti_kodu order by MONTH(sth_create_date)", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    MonthlyProductionWithProduct.Add(new MonthlyProductionWithProducts
                    {
                        Month = sdr[0].ToString(),
                        Desc = sdr[1].ToString(),
                        Lot = sdr[2].ToString(),
                        Value = (float)(Math.Round(Convert.ToDouble(sdr[3].ToString())))
                    });
                }
                sdr.Close();
                sqlcon.Close();
                GridMonthlyProductionWithProducts.ItemsSource = MonthlyProductionWithProduct;
                foreach (var column in GridMonthlyProductionWithProducts.Columns)
                {
                    column.HeaderFontAttributes = FontAttributes.Bold;
                    column.HeaderFontSize = 15;
                    //column.Width = 80;
                    //column.FixedStyle = DevExpress.XamarinForms.DataGrid.FixedStyle.Start;
                    if (column.FieldName == "Name")
                    { column.Width = 80; }
                    if (column.FieldName == "Month")
                    { column.IsGrouped = true; }
                }

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.ToString(), "Tamam");
            }
        }
        void grid_CalculateCustomSummary(object sender, DevExpress.XamarinForms.DataGrid.CustomSummaryEventArgs e)
        {
            if (e.FieldName.ToString() == "Shipped")
                if (e.IsTotalSummary)
                {
                    if (e.SummaryProcess == CustomSummaryProcess.Start)
                    {
                        count = 0;
                    }
                    if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                    {
                        if (!(bool)e.FieldValue)
                            count++;
                        e.TotalValue = count;
                    }
                }
        }

        private async void ToolBarToExcel_Clicked(object sender, EventArgs e)
        {
            var fileName = $"Contacts-{Guid.NewGuid()}.xlsx";
            string filepath = excelService.GenerateExcel(fileName);

            var data = new ExcelStructure
            {
                Headers = new List<string>() { "Month", "Desc", "Lot", "Amount" }
            };

            foreach (var item in MonthlyProductionWithProduct)
            {
                data.Values.Add(new List<string>() { item.Month, item.Desc, item.Lot, item.Value.ToString() });
            }

            excelService.InsertDataIntoSheet(filepath, "MonthlyProduction", data);

            await Launcher.OpenAsync(new OpenFileRequest()
            {
                File = new ReadOnlyFile(filepath)
            });
        }
    }
}