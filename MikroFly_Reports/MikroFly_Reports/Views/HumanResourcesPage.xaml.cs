using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using Microcharts;
using MikroFly_Reports.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MikroFly_Reports.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HumanResourcesPage : ContentPage
    {
        public static List<PersonelInfo> PersonelInfos;
        double workdays = 0;

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(3000);
            GetWorkDays();
            GetPersonnelInfo();
            FillGenderChart();
            FillAbsenteeChart();
            FillEducationLevelChart();
            FillAgeGroupChart();
            RefreshView.IsRefreshing = false;
        }

        public HumanResourcesPage()
        {
            InitializeComponent();
            GetWorkDays();
            GetPersonnelInfo();
            FillGenderChart();
            FillAbsenteeChart();
            FillEducationLevelChart();
            FillAgeGroupChart();
        }
        private void GetPersonnelInfo()
        {
            PersonelInfos = new List<PersonelInfo>();
            try
            {
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcomM1 = new SqlCommand("SELECT Code,[Name],EntryDate,DepartmentCode,DepartmentName,SectionName,[Group],Title,Education,EducationLevel,Gender,BirthDate,MilitaryService,ISNULL(Absentee,0) 'Absentee', ISNULL(RemainedLeaves,0) 'RemainedLeaves', Contact from MikroDB_V16_PERSONEL.dbo.[IDA_PERSONEL_DURUM]", sqlcon);
                SqlDataReader sdr = sqlcomM1.ExecuteReader();
                while (sdr.Read())
                {
                    PersonelInfos.Add(new PersonelInfo
                    {
                        Code = sdr["Code"].ToString(),
                        Name = sdr["Name"].ToString(),
                        EntryDate = sdr["EntryDate"].ToString().Split(' ')[0],
                        DepartmentCode = sdr["DepartmentCode"].ToString(),
                        DepartmentName = sdr["DepartmentName"].ToString(),
                        SectionName = sdr["SectionName"].ToString(),
                        Group = sdr["Group"].ToString(),
                        Title = sdr["Title"].ToString(),
                        Education = sdr["Education"].ToString(),
                        EducationLevel = sdr["EducationLevel"].ToString(),
                        Gender = sdr["Gender"].ToString(),
                        BirthDate = sdr["BirthDate"].ToString().Split(' ')[0],
                        MilitaryService = sdr["MilitaryService"].ToString(),
                        Absentee = Convert.ToDouble(sdr["Absentee"].ToString()),
                        RemainedLeaves = sdr["RemainedLeaves"].ToString(),
                        Contact = sdr["Contact"].ToString(),
                    });
                }
                sdr.Close();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Uyarı", ex.ToString(), "Tamam");
            }
        }
        private void FillGenderChart()
        {
            float CountMale = 0;
            float CountFemale = 0;
            foreach(var item in PersonelInfos)
            {
                if(item.Gender=="Erkek")
                    CountMale++;
                if(item.Gender=="Kadın")
                    CountFemale++;
            }
            var entries = new List<ChartEntry>();

            var entry = new ChartEntry(CountMale)
            {
                
                Label = "Male",
                ValueLabel = (CountMale.ToString()),
                Color = SKColor.FromHsv(347, 89, 78),
                ValueLabelColor = SKColor.FromHsv(347, 89, 78),
                TextColor = SKColor.FromHsv(347, 89, 78),
            };
            var entry1 = new ChartEntry(CountFemale)
            {
                Label = "Female",
                ValueLabel = (CountFemale).ToString(),
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
                LabelMode=LabelMode.None
            };
            this.ChartGenderRatio.Chart = chartdonut;
            LabelTotalEmployee.Text = (CountFemale+CountMale).ToString();
            LabelMale.Text="M: %"+ Math.Round((CountMale / (CountFemale + CountMale) * 100), 0).ToString();
            LabelFemale.Text = "F: %" + Math.Round((CountFemale / (CountFemale + CountMale) * 100),0) .ToString();
            //LabelObjective.Text = Achieved.ToString();
        }

        private void FillAbsenteeChart()
        {
            double CountAbsentDays = 0;
            double CountPeople = 0;
            foreach (var item in PersonelInfos)
            {
                CountPeople++;
                CountAbsentDays = CountAbsentDays + (item.Absentee);
            }
            var entries = new List<ChartEntry>();

            var entry = new ChartEntry((float)CountAbsentDays)
            {
                Color = SKColor.FromHsv(347, 89, 78),
                ValueLabelColor = SKColor.FromHsv(347, 89, 78),
                TextColor = SKColor.FromHsv(347, 89, 78),
            };
            var entry1 = new ChartEntry((float)((CountPeople*workdays)-CountAbsentDays))
            {
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
            this.ChartAbsenteeRate.Chart = chartdonut;
            LabelAbsenteePercentage.Text = "%" + Math.Round(((CountAbsentDays / (CountPeople * workdays)) * 100),0).ToString();
            //LabelObjective.Text = Achieved.ToString();
        }
        private void FillEducationLevelChart()
        {
            double Yok = 0, Ilk = 0, Orta = 0, Lise = 0, YuksekOkul = 0, Lisans = 0, YuksekLisans = 0, Doktora = 0;
            foreach (var data in PersonelInfos)
            {
                switch (data.EducationLevel)
                {
                    case "Yok":
                        Yok++;
                        break;
                    case "İlk":
                        Ilk++;
                        break;
                    case "Orta":
                        Orta++;
                        break;
                    case "Lise":
                        Lise++;
                        break;
                    case "Yüksek Okul":
                        YuksekOkul++;
                        break;
                    case "Lisans":
                        Lisans++;
                        break;
                    case "Yüksek Lisans":
                        YuksekLisans++;
                        break;
                    default:
                        Doktora++;
                        break;
                }

            }
            var entries = new List<ChartEntry>();

            var entry = new ChartEntry((float)Yok)
            {

                Label = "None",
                ValueLabel = (Yok.ToString()),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry1 = new ChartEntry((float)Ilk)
            {
                Label = "Primary",
                ValueLabel = (Ilk).ToString(),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry2 = new ChartEntry((float)Orta)
            {
                Label = "Secondary",
                ValueLabel = (Orta).ToString(),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry3 = new ChartEntry((float)Lise)
            {
                Label = "High",
                ValueLabel = (Lise).ToString(),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry4 = new ChartEntry((float)YuksekOkul)
            {
                Label = "Associate",
                ValueLabel = (YuksekOkul).ToString(),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry5 = new ChartEntry((float)Lisans)
            {
                Label = "Bachelor",
                ValueLabel = (Lisans).ToString(),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry6 = new ChartEntry((float)YuksekLisans)
            {
                Label = "Master",
                ValueLabel = (YuksekLisans).ToString(),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry7 = new ChartEntry((float)Doktora)
            {
                Label = "Doctorate",
                ValueLabel = (Doktora).ToString(),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            entries.Add(entry);
            entries.Add(entry1);
            entries.Add(entry2);
            entries.Add(entry3);
            entries.Add(entry4);
            entries.Add(entry5);
            entries.Add(entry6);
            entries.Add(entry7);

            var chartdonut = new  LineChart()
            {
                Entries = entries,
                LabelTextSize = 25,
            };
            this.ChartEducation.Chart = chartdonut;


        }
        private void FillAgeGroupChart()
        {
            double Yirmi = 0, YirmiBes = 0, Otuz = 0, OtuzBes = 0, Kirk = 0, KirkBes = 0, Elli = 0, ElliBes = 0, Altmis = 0, AltmisBes = 0, AltmisBesUst = 0;
            int yas = 0;
            foreach (var data in PersonelInfos)
            {
                yas = DateTime.Now.Year-Convert.ToInt32(data.BirthDate.Split('.').Last()); 
                switch (yas)
                {
                    case int n when (n<=20):
                        Yirmi++;
                        break;
                    case int n when (n <= 25):
                        YirmiBes++;
                        break;
                    case int n when (n <= 30):
                        Otuz++;
                        break;
                    case int n when (n <= 35):
                        OtuzBes++;
                        break;
                    case int n when (n <= 40):
                        Kirk++;
                        break;
                    case int n when (n <= 45):
                        KirkBes++;
                        break;
                    case int n when (n <= 50):
                        Elli++;
                        break;
                    case int n when (n <= 55):
                        ElliBes++;
                        break;
                    case int n when (n <= 60):
                        Altmis++;
                        break;
                    case int n when (n <= 65):
                        AltmisBes++;
                        break;
                    default:
                        AltmisBesUst++;
                        break;
                }

            }
            var entries = new List<ChartEntry>();

            var entry = new ChartEntry((float)Yirmi)
            {
                Label = "<21",
                ValueLabel = (Yirmi.ToString()),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry1 = new ChartEntry((float)YirmiBes)
            {
                Label = "21-25",
                ValueLabel = (YirmiBes.ToString()),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry2 = new ChartEntry((float)Otuz)
            {
                Label = "26-30",
                ValueLabel = (Otuz.ToString()),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry3 = new ChartEntry((float)OtuzBes)
            {
                Label = "31-35",
                ValueLabel = (OtuzBes.ToString()),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry4 = new ChartEntry((float)Kirk)
            {
                Label = "36-40",
                ValueLabel = (Kirk.ToString()),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry5 = new ChartEntry((float)KirkBes)
            {
                Label = "41-45",
                ValueLabel = (KirkBes.ToString()),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry6 = new ChartEntry((float)Elli)
            {
                Label = "46-50",
                ValueLabel = (Elli.ToString()),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry7 = new ChartEntry((float)ElliBes)
            {
                Label = "51-55",
                ValueLabel = (ElliBes.ToString()),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry8 = new ChartEntry((float)Altmis)
            {
                Label = "56-60",
                ValueLabel = (Altmis.ToString()),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry9 = new ChartEntry((float)AltmisBes)
            {
                Label = "61-65",
                ValueLabel = (AltmisBes.ToString()),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            var entry10 = new ChartEntry((float)AltmisBesUst)
            {
                Label = "65<",
                ValueLabel = (AltmisBesUst.ToString()),
                Color = SKColor.FromHsv(0, 0, 255),
                ValueLabelColor = SKColor.FromHsv(0, 0, 255),
                TextColor = SKColor.FromHsv(0, 0, 255),
            };
            entries.Add(entry);
            entries.Add(entry1);
            entries.Add(entry2);
            entries.Add(entry3);
            entries.Add(entry4);
            entries.Add(entry5);
            entries.Add(entry6);
            entries.Add(entry7);
            entries.Add(entry8);
            entries.Add(entry9);
            entries.Add(entry10);
            var chartdonut = new LineChart()
            {
                Entries = entries,
                LabelTextSize = 25,
            };
            this.ChartAge.Chart = chartdonut;
        }
        private void GetWorkDays()
        {
            workdays = 22;
        }

        private void ChartAbsenteeRate_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PersonnelInfo("Absentee"));
        }
        private void ChartGenderRatio_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PersonnelInfo("Gender"));
        }
        private void ChartEducation_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PersonnelInfo("EducationLevel"));
        }
        private void ChartAge_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PersonnelInfo("BirthDate"));
        }
    }
}