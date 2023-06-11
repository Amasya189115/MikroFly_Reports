using DevExpress.Data.Native;
using DocumentFormat.OpenXml.Drawing.Charts;
using MikroFly_Reports.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
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
    public partial class SalesDetailPage : ContentPage
    {
        public List<string> Market;
        public List<string> Country;
        public List<string> Customer;
        public List<string> ProductType;
        public List<string> ColumnList;
        public string Group=string.Empty;
        public string FilterByMarket=string.Empty;
        public string FilterByCountry = string.Empty;
        public string FilterByCustomer = string.Empty;
        public string FilterByProductType = string.Empty;
        public string FilterByGroupBy = string.Empty;
        public string FilterByStockName = string.Empty;
        public DateTime FilterByStartDate = new DateTime(2015, 12, 31);
        public DateTime FilterByEndDate = new DateTime(2015, 12, 31);
        public List<SalesRecords> SalesRecord { get; set; }
        public List<SalesRecords> FilteredSalesRecord { get; set; }
        public SalesDetailPage()
        {
            InitializeComponent();
            SalesRecord = new List<SalesRecords>();
            FilteredSalesRecord = new List<SalesRecords>();
            Market = new List<string>();
            ColumnList = new List<string>();
            Country = new List<string>();
            Customer = new List<string>();  
            ProductType = new List<string>();
            Group = "Group";
            FillDataGrid();
            GroupSaleColumns();
        }

        private async void ToolBarItemFilter_Clicked(object sender, EventArgs e)
        {
                try
                {
                       Group = "Group";
                       FilterByMarket = string.Empty;
                       FilterByCountry = string.Empty;
                       FilterByCustomer = string.Empty;
                       FilterByProductType = string.Empty;
                       FilterByGroupBy = string.Empty;
                       FilterByStockName = string.Empty;
                var pageInfo = new PopUpSalesFilters(Customer, Country, ProductType, ColumnList);
                pageInfo.MarketEventHandler += async (popupsender, userdata) =>
                {
                    FilterByMarket = userdata;
                };
                pageInfo.CountryEventHandler += async (popupsender, userdata) =>
                {
                    FilterByCountry = userdata;
                };
                pageInfo.CustomerEventHandler += async (popupsender, userdata) =>
                {
                    FilterByCustomer = userdata;
                };
                pageInfo.ProductTypeEventHandler += async (popupsender, userdata) =>
                {
                    if (userdata == "Dialyzer")
                    {
                        FilterByProductType = "PR1";
                    }
                    else
                    {
                        if (userdata == "Bloodline")
                        {
                            FilterByProductType = "PR2";
                            FilterByStockName = "Line";
                        }
                        else
                        {
                            if (userdata == "Needle")
                            {
                                FilterByProductType = "PR2";
                                FilterByStockName = "Need";
                            }
                            else
                            {
                                FilterByProductType = "PR3";
                            }
                        }
                    }
                };
                pageInfo.GroupByEventHandler += async (popupsender, userdata) =>
                {
                    FilterByGroupBy = userdata;
                };
                pageInfo.StartDateEventHandler += async (popupsender, userdata) =>
                {
                    FilterByStartDate = userdata;
                };
                pageInfo.EndDateEventHandler += async (popupsender, userdata) =>
                {
                    FilterByEndDate = userdata;
                    FillDataGridFiltered();
                };

                await PopupNavigation.Instance.PushAsync(pageInfo);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",ex.Message,"Ok");
            }
            
        }

        private void GroupSaleColumns()
        {
            try
            {
                var groupByCustomer =
                from student in SalesRecord
                group student by student.Customer into groupedCustomer
                select groupedCustomer.Key;
                foreach (var item in groupByCustomer)
                {
                    Customer.Add(item.ToString());
                }
                var groupByCountry =
                from student in SalesRecord
                group student by student.Country into groupedCustomer
                select groupedCustomer.Key;
                foreach (var item in groupByCountry)
                {
                    Country.Add(item.ToString());
                }
                var groupByProductType =
                from student in SalesRecord
                group student by student.Group into groupedCustomer
                select groupedCustomer.Key;
                foreach (var item in groupByProductType)
                {
                    ProductType.Add(item.ToString());
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error",ex.Message,"Ok");
            }

            
        }
        private void FillDataGrid()
        {
            try
            {
                SalesRecord.Clear();
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select Left(adr_ulke,13) 'Country',Left(cari_unvan1,13) 'Customer',sth_parti_kodu 'LotNu',sth_lot_no 'SterNu',sto_isim 'Product',\r\nsum(sth_miktar) 'Quantity',sth_tarih 'Date', \r\ncase when sto_anagrup_kod ='PR1' then 'Dialyzer'\r\nwhen sto_anagrup_kod='PR2' and sto_isim like '%Need%' then 'Needle'\r\nwhen sto_anagrup_kod='PR2' and sto_isim not like '%Need%' then 'Bloodline'\r\nwhen sto_anagrup_kod='PR3' then 'Powder'\r\nelse '' end as 'Group'\r\nfrom CARI_HESAP_HAREKETLERI \r\ninner join STOK_HAREKETLERI on sth_fat_uid=cha_Guid\r\ninner join CARI_HESAPLAR on cha_kod=cari_kod\r\ninner join STOKLAR on sth_stok_kod=sto_kod\r\nleft outer join CARI_HESAP_ADRESLERI on adr_cari_kod=cari_kod and adr_adres_no=1\r\nwhere cha_evrak_tip=63 and sto_anagrup_kod in ('PR1','PR2','PR3') and \r\ncari_unvan1 not in ('MEDICALPARK EOOD','ERDEMLER GERİ DÖNÜŞÜM METAL SANAYİ VE TİCARET LİMİTED ŞİRKETİ','TURKA GROUP MEDİKAL TİCARET ANONİM ŞİRKETİ','TURKAGROUP MEDİKAL TİCARET LTD.ŞTİ.')\r\ngroup by adr_ulke,cari_unvan1,sth_stok_kod,sto_isim,sth_parti_kodu,sth_lot_no,sto_anagrup_kod,sth_tarih\r\n", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    SalesRecord.Add(new SalesRecords
                    {
                        Country = sdr[0].ToString(),
                        Customer = sdr[1].ToString(),
                        LotNu = sdr[2].ToString(),
                        SterNu = sdr[3].ToString(),
                        Product = sdr[4].ToString(),
                        Quantity = float.Parse(sdr[5].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                        Date = Convert.ToDateTime(sdr[6]),
                        Group = sdr[7].ToString(),
                    });
                }
                DataGridSalesList.ItemsSource = null;
                DataGridSalesList.ItemsSource = SalesRecord;
                ColumnList.Clear();

                foreach (var column in DataGridSalesList.Columns)
                {
                    ColumnList.Add(column.FieldName);
                    column.HeaderFontAttributes = FontAttributes.Bold;
                    column.HeaderFontSize = 15;
                    column.Width = 100;
                    column.AutoFilterCondition = DevExpress.Data.AutoFilterCondition.Contains;

                    if (column.FieldName == "Product")
                    { column.FixedStyle = DevExpress.XamarinForms.DataGrid.FixedStyle.Start; }
                    if (column.FieldName == "Customer")
                    { column.IsGrouped = true; }
                }
                sdr.Close();
                sqlcon.Close();
            }
            catch (Exception ex)
            {

                Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Ok");
            }

        }
        private async void FillDataGridFiltered()
        {
            try
            {
                FilteredSalesRecord.Clear();
                SqlConnection sqlcon = new SqlConnection(LoginPage.ConnectionString);
                sqlcon.Open();
                ////SqlCommand sqlcom = new SqlCommand("SELECT * FROM [dbo].[FX_BarkodMiktar] ('" + message.Text + "'," + StokHareketAyarSayfasi.cikisdepodeger + "," + resultsthevraktip.Last().ToString() + ",0)", sqlcon);
                SqlCommand sqlcom = new SqlCommand("Select Left(adr_ulke,13) 'Country',Left(cari_unvan1,13) 'Customer',sth_parti_kodu 'LotNu',sth_lot_no 'SterNu',sto_isim 'Product',\r\nsum(sth_miktar) 'Quantity',sth_tarih 'Date', \r\ncase when sto_anagrup_kod ='PR1' then 'Dialyzer'\r\nwhen sto_anagrup_kod='PR2' and sto_isim like '%Need%' then 'Needle'\r\nwhen sto_anagrup_kod='PR2' and sto_isim not like '%Need%' then 'Bloodline'\r\nwhen sto_anagrup_kod='PR3' then 'Powder'\r\nelse '' end as 'Group'\r\nfrom CARI_HESAP_HAREKETLERI \r\ninner join STOK_HAREKETLERI on sth_fat_uid=cha_Guid\r\ninner join CARI_HESAPLAR on cha_kod=cari_kod\r\ninner join STOKLAR on sth_stok_kod=sto_kod\r\nleft outer join CARI_HESAP_ADRESLERI on adr_cari_kod=cari_kod and adr_adres_no=1\r\nwhere"+
                    " Left(adr_ulke,13) Like '%"+FilterByCountry+ "%' and  Left(cari_unvan1,13) Like '%" + FilterByCustomer+ "%' and  sto_anagrup_kod Like '%" + FilterByProductType+ "%' and  sto_isim Like '%" + FilterByStockName+"%' and cha_evrak_tip=63 and sto_anagrup_kod in ('PR1','PR2','PR3') and \r\ncari_unvan1 not in ('MEDICALPARK EOOD','ERDEMLER GERİ DÖNÜŞÜM METAL SANAYİ VE TİCARET LİMİTED ŞİRKETİ','TURKA GROUP MEDİKAL TİCARET ANONİM ŞİRKETİ','TURKAGROUP MEDİKAL TİCARET LTD.ŞTİ.')\r\ngroup by adr_ulke,cari_unvan1,sth_stok_kod,sto_isim,sth_parti_kodu,sth_lot_no,sto_anagrup_kod,sth_tarih\r\n", sqlcon);
                SqlDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    FilteredSalesRecord.Add(new SalesRecords
                    {
                        Country = sdr[0].ToString(),
                        Customer = sdr[1].ToString(),
                        LotNu = sdr[2].ToString(),
                        SterNu = sdr[3].ToString(),
                        Product = sdr[4].ToString(),
                        Quantity = float.Parse(sdr[5].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                        Date = Convert.ToDateTime(sdr[6]),
                        Group = sdr[7].ToString(),
                    });
                }
                DataGridSalesList.ItemsSource=null;
                DataGridSalesList.ItemsSource = FilteredSalesRecord;
                ColumnList.Clear();
                if (FilterByGroupBy != String.Empty)
                    Group = FilterByGroupBy;

                foreach (var column in DataGridSalesList.Columns)
                {
                    ColumnList.Add(column.FieldName);
                    column.HeaderFontAttributes = FontAttributes.Bold;
                    column.HeaderFontSize = 15;
                    column.Width = 100;
                    column.AutoFilterCondition = DevExpress.Data.AutoFilterCondition.Contains;

                    if (column.FieldName == "Product")
                    { column.FixedStyle = DevExpress.XamarinForms.DataGrid.FixedStyle.Start; }
                    if (column.FieldName == Group)
                    { column.IsGrouped = true; }
                }
                sdr.Close();
                sqlcon.Close();
            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Ok");
            }
        }
        private async void DataGridSalesList_PullToRefresh(object sender, EventArgs e)
        {
            FillDataGrid();
            await Task.Delay(2000);
            DataGridSalesList.IsRefreshing = false;
        }
    }
}