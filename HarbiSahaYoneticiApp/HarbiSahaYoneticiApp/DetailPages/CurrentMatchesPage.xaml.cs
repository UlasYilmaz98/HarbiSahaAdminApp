using HarbiSahaYoneticiApp.Models;
using HarbiSahaYoneticiApp.Models.ViewModels;
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
    public partial class CurrentMatchesPage : ContentPage
    {
        List<CurrentMatchesPageViewModel> viewModels = new List<CurrentMatchesPageViewModel>();
        List<Match> matches = new List<Match>();
        ManageServices service = new ManageServices();
        int fieldZoneId = 1;
        
        public CurrentMatchesPage()
        {
            InitializeComponent();
            Fill();
        }

        private async void Fill()
        {
            MainMasterDetailPage main = App.Current.MainPage as MainMasterDetailPage;
            main.IsPresented = false;
            await Navigation.PushPopupAsync(new BasePopupPage("Yükleniyor..."));
            matches = await service.GetCurrentMatches(fieldZoneId);
            if ( matches == null)
            {
                lblAlert.IsVisible = true;
                return;
            }
            foreach( Match match in matches)
            {
                viewModels.Add(new CurrentMatchesPageViewModel()
                {
                    Match = match
                });
            }
            viewModels = viewModels.OrderBy(x => x.MatchDay).ToList();
            colMatches.ItemsSource = viewModels;
            await Navigation.PopPopupAsync();
            
        }

        private void ColMatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}