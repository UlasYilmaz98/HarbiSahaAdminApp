using HarbiSahaYoneticiApp.Models;
using HarbiSahaYoneticiApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Extensions;
using HarbiSahaYoneticiApp.PopupPages;
using HarbiSahaYoneticiApp.ServiceController;

namespace HarbiSahaYoneticiApp.DetailPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubscribesPage : ContentPage
    {
        bool isChanged = false;
        FieldZone currentFz = new FieldZone();
        Field currentField = new Field();
        List<FieldZone> fieldZones = new List<FieldZone>();
        List<Announcement> allSubs = new List<Announcement>();
        List<Announcement> subscribeList = new List<Announcement>();
        List<string> fieldZoneNameList = new List<string>();
        List<SubscribesPageViewModel> viewModelsPzt = new List<SubscribesPageViewModel>();
        List<SubscribesPageViewModel> viewModelsSali = new List<SubscribesPageViewModel>();
        List<SubscribesPageViewModel> viewModelsCrs = new List<SubscribesPageViewModel>();
        List<SubscribesPageViewModel> viewModelsPrs = new List<SubscribesPageViewModel>();
        List<SubscribesPageViewModel> viewModelsCum = new List<SubscribesPageViewModel>();
        List<SubscribesPageViewModel> viewModelsCts = new List<SubscribesPageViewModel>();
        List<SubscribesPageViewModel> viewModelsPzr = new List<SubscribesPageViewModel>();
        ManageServices service = new ManageServices();
        public SubscribesPage()
        {
            InitializeComponent();
            currentField = App.Current.Properties["loggedField"] as Field;
            foreach (FieldZone fz in currentField.FieldZones)
            {
                fieldZoneNameList.Add(fz.FieldZoneName);
                fieldZones.Add(fz);
            }
            pickerFieldZone.ItemsSource = fieldZoneNameList;
            
            Fill();
        }

        private async void Fill()
        {
            //allSubs.Clear();
            subscribeList.Clear();
          //await Navigation.PushPopupAsync(new BasePopupPage("Yükleniyor..."));
            allSubs = App.Current.Properties["Subscribes"] as List<Announcement>;
            currentFz = App.Current.Properties["currentFieldZone"] as FieldZone;
            pickerFieldZone.SelectedItem = currentFz.FieldZoneName;
            if( allSubs.Count > 0 )
            {
                foreach ( Announcement sub in allSubs )
                {
                    if (sub.FieldZoneId == currentFz.Id)
                        subscribeList.Add(sub);
                }
            }
            if (subscribeList == null)
            {

                lblAlertMonday.IsVisible = true;
                lblAlertTuesday.IsVisible = true;
                lblAlertWednesday.IsVisible = true;
                lblAlertThursday.IsVisible = true;
                lblAlertFriday.IsVisible = true;
                lblAlertSaturday.IsVisible = true;
                lblAlertSunday.IsVisible = true;
            }
            else
            {
                if ( subscribeList.Count <= 0)
                {
                    lblAlertMonday.IsVisible = true;
                    lblAlertTuesday.IsVisible = true;
                    lblAlertWednesday.IsVisible = true;
                    lblAlertThursday.IsVisible = true;
                    lblAlertFriday.IsVisible = true;
                    lblAlertSaturday.IsVisible = true;
                    lblAlertSunday.IsVisible = true;
                }
                else
                {
                    foreach (Announcement sub in subscribeList)
                    {
                        switch( sub.SubscribeDayOfWeek)
                        {
                            case 1: viewModelsPzt.Add(new SubscribesPageViewModel() { Subscribe = sub });break;
                            case 2: viewModelsSali.Add(new SubscribesPageViewModel() { Subscribe = sub }); break;
                            case 3: viewModelsCrs.Add(new SubscribesPageViewModel() { Subscribe = sub }); break;
                            case 4: viewModelsPrs.Add(new SubscribesPageViewModel() { Subscribe = sub }); break;
                            case 5: viewModelsCum.Add(new SubscribesPageViewModel() { Subscribe = sub }); break;
                            case 6: viewModelsCts.Add(new SubscribesPageViewModel() { Subscribe = sub }); break;
                            case 7: viewModelsPzr.Add(new SubscribesPageViewModel() { Subscribe = sub }); break;
                        }
                    }
                    viewModelsPzt = viewModelsPzt.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    viewModelsSali = viewModelsSali.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    viewModelsCrs = viewModelsCrs.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    viewModelsPrs = viewModelsPrs.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    viewModelsCum = viewModelsCum.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    viewModelsCts = viewModelsCts.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    viewModelsPzr = viewModelsPzr.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    colMonday.ItemsSource = viewModelsPzt;
                    colTuesday.ItemsSource = viewModelsSali;
                    colWednesday.ItemsSource = viewModelsCrs;
                    colThursday.ItemsSource = viewModelsPrs;
                    colFriday.ItemsSource = viewModelsCum;
                    colSaturday.ItemsSource = viewModelsCts;
                    colSunday.ItemsSource = viewModelsPzr;
                    //colMonday.ItemsSource = viewModelsPzt;
                }
            }
            if (viewModelsPzt == null || viewModelsPzt.Count <= 0)
                lblAlertMonday.IsVisible = true;
            if (viewModelsSali == null || viewModelsPzt.Count <= 0)
                lblAlertTuesday.IsVisible = true;
            if (viewModelsCrs == null || viewModelsPzt.Count <= 0)
                lblAlertWednesday.IsVisible = true;
            if (viewModelsPrs == null || viewModelsPzt.Count <= 0)
                lblAlertThursday.IsVisible = true;
            if (viewModelsCum == null || viewModelsPzt.Count <= 0)
                lblAlertFriday.IsVisible = true;
            if (viewModelsCts == null || viewModelsPzt.Count <= 0)
                lblAlertSaturday.IsVisible = true;
            if (viewModelsPzr == null || viewModelsPzt.Count <= 0)
                lblAlertSunday.IsVisible = true;


            if (viewModelsPzt.Count > 4)
                colMonday.HeightRequest = 120;
            else
                colMonday.HeightRequest = 60;
            if (viewModelsSali.Count > 4)
                colTuesday.HeightRequest = 120;
            else
                colTuesday.HeightRequest = 60;
            if (viewModelsCrs.Count > 4)
                colWednesday.HeightRequest = 120;
            else
                colWednesday.HeightRequest = 60;
            if (viewModelsPrs.Count > 4)
                colThursday.HeightRequest = 120;
            else
                colThursday.HeightRequest = 60;
            if (viewModelsCum.Count > 4)
                colFriday.HeightRequest = 120;
            else
                colFriday.HeightRequest = 60;
            if (viewModelsCts.Count > 4)
                colSaturday.HeightRequest = 120;
            else
                colSaturday.HeightRequest = 60;
            if (viewModelsPzr.Count > 4)
                colSunday.HeightRequest = 120;
            else
                colSunday.HeightRequest = 60;

            isChanged = true;
        }

        private async void ColMonday_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionView coll = (CollectionView)sender;
                
            if ( coll.SelectedItem != null)
            {
                string responseAction = await DisplayActionSheet("Seçenekler", "Vazgeç", null, new string[] { "Aboneliği kaldır" });
                if (responseAction != "Aboneliği kaldır")
                {
                    coll.SelectedItem = null;
                    return;
                }
                   
                await Navigation.PushPopupAsync(new BasePopupPage("İşleniyor..."));
                SubscribesPageViewModel model = e.CurrentSelection[0] as SubscribesPageViewModel;
                string response = await service.CancelSubscribe(model.Subscribe.SubscribeDayOfWeek, model.Subscribe.SubscribeMatchTime);
                if (response == "Başarılı")
                {
                    List<Announcement> subsList = new List<Announcement>();
                    subsList = App.Current.Properties["Subscribes"] as List<Announcement>;
                    Announcement subToDelete = subsList.Where(x => x.FieldZoneId == currentFz.Id && x.SubscribeMatchTime == model.Subscribe.SubscribeMatchTime &&
                   x.SubscribeDayOfWeek == model.Subscribe.SubscribeDayOfWeek).FirstOrDefault();
                    subsList.Remove(subToDelete);
                    App.Current.Properties["Subscribes"] = subsList;
                    FillAgain();
                    await DisplayAlert("Başarılı", "Seçili abonelik kaldırıldı!", "OK");

                }
                else
                {
                   await DisplayAlert("Hata", "Bir şeyler ters gitti!", "OK");
                }
                await Navigation.PopPopupAsync();
                
              
               
            }
        }

        

        private async void AddSub_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new AddSubscribePopupPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<AddSubscribePopupPage>(this, "SubscribesPage", async (sender) =>
            {
                //DatePicker1_DateSelected(datePicker1, new DateChangedEventArgs(datePicker1.Date, datePicker1.Date)); // servisten veri çek ve doldur.
                FillAgain();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<AddSubscribePopupPage>(this, "SubscribesPage");
        }

        private async void FillAgain()
        {
            viewModelsPzt.Clear();
            viewModelsSali.Clear();
            viewModelsCrs.Clear();
            viewModelsPrs.Clear();
            viewModelsCum.Clear();
            viewModelsCts.Clear();
            viewModelsPzr.Clear();
            //allSubs.Clear();
            subscribeList.Clear();
            //await Navigation.PushPopupAsync(new BasePopupPage("Yükleniyor..."));
            allSubs = App.Current.Properties["Subscribes"] as List<Announcement>;
            currentFz = App.Current.Properties["currentFieldZone"] as FieldZone;
            pickerFieldZone.SelectedItem = currentFz.FieldZoneName;
            if (allSubs.Count > 0)
            {
                foreach (Announcement sub in allSubs)
                {
                    if (sub.FieldZoneId == currentFz.Id)
                        subscribeList.Add(sub);
                }
            }
            //await Navigation.PushPopupAsync(new BasePopupPage("Yükleniyor..."));
            //subscribeList = App.Current.Properties["Subscribes"] as List<Announcement>;
            if (subscribeList == null)
            {

                lblAlertMonday.IsVisible = true;
                lblAlertTuesday.IsVisible = true;
                lblAlertWednesday.IsVisible = true;
                lblAlertThursday.IsVisible = true;
                lblAlertFriday.IsVisible = true;
                lblAlertSaturday.IsVisible = true;
                lblAlertSunday.IsVisible = true;
            }
            else
            {
                if (subscribeList.Count <= 0)
                {
                    lblAlertMonday.IsVisible = true;
                    lblAlertTuesday.IsVisible = true;
                    lblAlertWednesday.IsVisible = true;
                    lblAlertThursday.IsVisible = true;
                    lblAlertFriday.IsVisible = true;
                    lblAlertSaturday.IsVisible = true;
                    lblAlertSunday.IsVisible = true;
                }
                else
                {
                    foreach (Announcement sub in subscribeList)
                    {
                        switch (sub.SubscribeDayOfWeek)
                        {
                            case 1: viewModelsPzt.Add(new SubscribesPageViewModel() { Subscribe = sub }); break;
                            case 2: viewModelsSali.Add(new SubscribesPageViewModel() { Subscribe = sub }); break;
                            case 3: viewModelsCrs.Add(new SubscribesPageViewModel() { Subscribe = sub }); break;
                            case 4: viewModelsPrs.Add(new SubscribesPageViewModel() { Subscribe = sub }); break;
                            case 5: viewModelsCum.Add(new SubscribesPageViewModel() { Subscribe = sub }); break;
                            case 6: viewModelsCts.Add(new SubscribesPageViewModel() { Subscribe = sub }); break;
                            case 7: viewModelsPzr.Add(new SubscribesPageViewModel() { Subscribe = sub }); break;
                        }
                    }
                    viewModelsPzt = viewModelsPzt.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    viewModelsSali = viewModelsSali.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    viewModelsCrs = viewModelsCrs.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    viewModelsPrs = viewModelsPrs.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    viewModelsCum = viewModelsCum.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    viewModelsCts = viewModelsCts.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    viewModelsPzr = viewModelsPzr.OrderBy(x => x.Subscribe.SubscribeMatchTime).ToList();
                    colMonday.ItemsSource = viewModelsPzt;
                    colTuesday.ItemsSource = viewModelsSali;
                    colWednesday.ItemsSource = viewModelsCrs;
                    colThursday.ItemsSource = viewModelsPrs;
                    colFriday.ItemsSource = viewModelsCum;
                    colSaturday.ItemsSource = viewModelsCts;
                    colSunday.ItemsSource = viewModelsPzr;
                    //colMonday.ItemsSource = viewModelsPzt;
                }
            }
            if (viewModelsPzt == null || viewModelsPzt.Count <= 0)
                lblAlertMonday.IsVisible = true;
            if (viewModelsSali == null || viewModelsPzt.Count <= 0)
                lblAlertTuesday.IsVisible = true;
            if (viewModelsCrs == null || viewModelsPzt.Count <= 0)
                lblAlertWednesday.IsVisible = true;
            if (viewModelsPrs == null || viewModelsPzt.Count <= 0)
                lblAlertThursday.IsVisible = true;
            if (viewModelsCum == null || viewModelsPzt.Count <= 0)
                lblAlertFriday.IsVisible = true;
            if (viewModelsCts == null || viewModelsPzt.Count <= 0)
                lblAlertSaturday.IsVisible = true;
            if (viewModelsPzr == null || viewModelsPzt.Count <= 0)
                lblAlertSunday.IsVisible = true;


            if (viewModelsPzt.Count > 4)
                colMonday.HeightRequest = 120;
            else
                colMonday.HeightRequest = 60;
            if (viewModelsSali.Count > 4)
                colTuesday.HeightRequest = 120;
            else
                colTuesday.HeightRequest = 60;
            if (viewModelsCrs.Count > 4)
                colWednesday.HeightRequest = 120;
            else
                colWednesday.HeightRequest = 60;
            if (viewModelsPrs.Count > 4)
                colThursday.HeightRequest = 120;
            else
                colThursday.HeightRequest = 60;
            if (viewModelsCum.Count > 4)
                colFriday.HeightRequest = 120;
            else
                colFriday.HeightRequest = 60;
            if (viewModelsCts.Count > 4)
                colSaturday.HeightRequest = 120;
            else
                colSaturday.HeightRequest = 60;
            if (viewModelsPzr.Count > 4)
                colSunday.HeightRequest = 120;
            else
                colSunday.HeightRequest = 60;

        }

        private void PickerFieldZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( isChanged )
            {
                colSunday.ItemsSource = null;
                colMonday.ItemsSource = null;
                colTuesday.ItemsSource = null;
                colWednesday.ItemsSource = null;
                colThursday.ItemsSource = null;
                colFriday.ItemsSource = null;
                colSaturday.ItemsSource = null;
                viewModelsCrs.Clear();
                viewModelsCts.Clear();
                viewModelsPrs.Clear();
                viewModelsCum.Clear();
                viewModelsPzt.Clear();
                viewModelsPzr.Clear();
                viewModelsSali.Clear();
                lblAlertFriday.Text = "";
                lblAlertMonday.Text = "";
                lblAlertSunday.Text = "";
                lblAlertTuesday.Text = "";
                lblAlertThursday.Text = "";
                lblAlertSaturday.Text = "";
                lblAlertWednesday.Text = "";

                string selectedItem = pickerFieldZone.SelectedItem as string;
                FieldZone selected = fieldZones.Where(x => x.FieldZoneName == selectedItem).FirstOrDefault();
                App.Current.Properties["currentFieldZone"] = selected;
                Fill();
            }
        }
    }
}