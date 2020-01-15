using HarbiSahaYoneticiApp.Models;
using HarbiSahaYoneticiApp.PopupPages;
using HarbiSahaYoneticiApp.ServiceController;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HarbiSahaYoneticiApp.DetailPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPage : ContentPage
    {
        List<Purchase> purchases = new List<Purchase>();
        ManageServices service = new ManageServices();
        public ReportPage()
        {
            InitializeComponent();
            Fill();
        }

        private async void Fill()
        {
            //await Navigation.PushPopupAsync(new BasePopupPage("Yükleniyor..."));
            //purchases = await service.getPurchases();
            //await Navigation.PopPopupAsync();
        }

        private async void BtnDailyReports_Clicked(object sender, EventArgs e)
        {
            MainMasterDetailPage page = App.Current.MainPage as MainMasterDetailPage;
            page.Detail = new NavigationPage(new DailyReportsPage())
            { BarBackgroundColor = Color.FromHex("#495764"), BarTextColor = Color.FromHex("#F6F8D3"), Title = "Günlük Raporlar" }; page.IsPresented = false;
        }

        private void BtnWeeklyReports_Clicked(object sender, EventArgs e)
        {
            MainMasterDetailPage page = App.Current.MainPage as MainMasterDetailPage;
            page.Detail = new NavigationPage(new WeeklyReportPage())
            { BarBackgroundColor = Color.FromHex("#495764"), BarTextColor = Color.FromHex("#F6F8D3"), Title = "haftalık Raporlar" }; page.IsPresented = false;
        }

        private void BtnMonthlyReports_Clicked(object sender, EventArgs e)
        {
            MainMasterDetailPage page = App.Current.MainPage as MainMasterDetailPage;
            page.Detail = new NavigationPage(new MonthlyReportsPage())
            { BarBackgroundColor = Color.FromHex("#495764"), BarTextColor = Color.FromHex("#F6F8D3"), Title = "Aylık Raporlar" }; page.IsPresented = false;
        }

        private void BtnStatistics_Clicked(object sender, EventArgs e)
        {

        }
    }
}