using HarbiSahaYoneticiApp.DetailPages;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HarbiSahaYoneticiApp.MasterPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMasterPage : ContentPage
    {
        List<MainMasterDetailPageMasterMenuItem> items = new List<MainMasterDetailPageMasterMenuItem>();
        public MainMasterPage()
        {
            
            InitializeComponent();
            items.Add(new MainMasterDetailPageMasterMenuItem()
            {
                IconText = "calendar-week",
                Text = "Takvim"
            });
            items.Add(new MainMasterDetailPageMasterMenuItem()
            {
                IconText = "copy",
                Text = "Rezervasyonlar"
            });
            items.Add(new MainMasterDetailPageMasterMenuItem()
            {
                IconText = "futbol",
                Text = "Abonelikler"
            });
            items.Add(new MainMasterDetailPageMasterMenuItem()
            {
                IconText = "chart-line",
                Text = "Durum Bilgisi"
            });
            items.Add(new MainMasterDetailPageMasterMenuItem()
            {
                IconText = "bell",
                Text = "Bildirimler"
            });
            items.Add(new MainMasterDetailPageMasterMenuItem()
            {
                IconText = "sign-out-alt",
                Text = "Çıkış Yap"

            });
            
            lstItems.ItemsSource = items;
            
        }

        private async void LstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ( lstItems.SelectedItem != null)
            {
                MainMasterDetailPage page = App.Current.MainPage as MainMasterDetailPage;
                MainMasterDetailPageMasterMenuItem item = e.CurrentSelection[0] as MainMasterDetailPageMasterMenuItem;
                switch (item.Text)
                {
                    case "Rezervasyonlar": page.Detail = new NavigationPage(new CurrentMatchesPage())
                    { BarBackgroundColor = Color.FromHex("#495764"),BarTextColor = Color.FromHex("#F6F8D3"),Title="Güncel Kiralamalar" };break;
                    case "Takvim":
                        page.Detail = new NavigationPage(new CalendarPage())
                        { BarBackgroundColor = Color.FromHex("#495764"), BarTextColor = Color.FromHex("#F6F8D3"), Title = "Takvim" };page.IsPresented = false; break;
                    case "Abonelikler":
                        page.Detail = new NavigationPage(new SubscribesPage())
                        { BarBackgroundColor = Color.FromHex("#495764"), BarTextColor = Color.FromHex("#F6F8D3"), Title = "Abonelikler" }; page.IsPresented = false; break;
                    case "Durum Bilgisi":
                        page.Detail = new NavigationPage(new ReportPage())
                        { BarBackgroundColor = Color.FromHex("#495764"), BarTextColor = Color.FromHex("#F6F8D3"), Title = "Abonelikler" }; page.IsPresented = false; break;
                    case "Çıkış Yap":
                        Sign_Out();break;
                }
                lstItems.SelectedItem = null;
            }
        }

        private async void Sign_Out()
        {
            if ( App.Current.Properties.ContainsKey("Notes") )
                App.Current.Properties.Remove("Notes");
            if (App.Current.Properties.ContainsKey("Subscribes"))
                App.Current.Properties.Remove("Subscribes");

           
            App.Current.Properties.Remove("loggedField");
            App.Current.Properties.Remove("currentFieldZone");
            CrossSecureStorage.Current.DeleteKey("loggedFieldId");
            CrossSecureStorage.Current.DeleteKey("loggedFieldEmail");
            CrossSecureStorage.Current.DeleteKey("loggedFieldPassword");
            App.Current.MainPage = new LoginPage();
            
        }

        private void TapToPhone_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("tel:08503055095"));
        }

        private void TapToMail_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("mailto:info@harbisaha.com"));
        }
    }
}