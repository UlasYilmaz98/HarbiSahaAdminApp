using HarbiSahaYoneticiApp.Models;
using HarbiSahaYoneticiApp.Models.ControlModels;
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
    public partial class CalendarPage : ContentPage
    {
        List<string> fieldZoneNames = new List<string>();
        Field currentField = new Field();
        List<Announcement> notes = new List<Announcement>();
        List<Announcement> subs = new List<Announcement>();
        int fieldZoneId;
        List<Match> matches = new List<Match>();
        List<Announcement> abonelikler = new List<Announcement>();
        ManageServices service = new ManageServices();
        List<CalendarPageViewModel> viewModels = new List<CalendarPageViewModel>();
        DateTime threeWeeksLater;
        DateTime today;
        DateTime currentDay;
        public List<DateTime> Days = new List<DateTime>();
        public List<FieldZone> fieldZones = new List<FieldZone>();
        public FieldZone currentFieldZone = new FieldZone();
        ActivityControl control = new ActivityControl();
        bool isButtonClicked = false;
        bool isChancing = false;

        public CalendarPage()
        {
            InitializeComponent();
            currentField = Application.Current.Properties["loggedField"] as Field;
            fieldZones = currentField.FieldZones;
            foreach (FieldZone fz in fieldZones)
                fieldZoneNames.Add(fz.FieldZoneName);
            pickerFieldZone.ItemsSource = fieldZoneNames;          
            currentFieldZone = Application.Current.Properties["currentFieldZone"] as FieldZone;
            fieldZoneId = currentFieldZone.Id;
            pickerFieldZone.SelectedItem = currentFieldZone.FieldZoneName;
            control.MakeVisible(ActivityLayout, activity);
            datePicker1.Date = DateTime.Today;
            datePicker1.Format = " dddd dd/MM";
            datePicker1.MinimumDate = DateTime.Today;
            datePicker1.MaximumDate = DateTime.Today.AddDays(21);
            if (datePicker1.Date == today)
            {
                //btnPrev.IsEnabled = false;
            }
            //btnPrev.IsEnabled = false;
            Fill();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            MessagingCenter.Subscribe<AddNotesPopupPage>(this, "CalendarPage", async (sender) =>
            {
                DatePicker1_DateSelected(datePicker1, new DateChangedEventArgs(datePicker1.Date, datePicker1.Date)); // servisten veri çek ve doldur.
            });
            MessagingCenter.Subscribe<DefineCostPopupPage,Dictionary<int,int>>(this, "CalendarPage", async (sender,kaporaVeFiyat) =>
              {

                  if (kaporaVeFiyat.Count > 5)
                      FixCost(kaporaVeFiyat);
                  else
                      SetRentedFromClose(kaporaVeFiyat);
                  
                  //DatePicker1_DateSelected(datePicker1, new DateChangedEventArgs(datePicker1.Date, datePicker1.Date)); // servisten veri çek ve doldur.


              });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<AddNotesPopupPage>(this, "CalendarPage");
            MessagingCenter.Unsubscribe<DefineCostPopupPage, Dictionary<int, int>>(this, "CalendarPage");
        }


        //YENİ DÜZENDE 3 DURUM SÖZ KONUSU OLACAK.
        // 1 - AÇIK ( ONLINE SİSTEMDE YAYINDA )
        // 2 - KAPALI ( YAYINDA DEĞİL )
        // 3 - DOLU ( KAPORA ALINDI,KAYDEDİLDİ,KİRALANDI VS ) ( GELİR BİLGİSİ GİRİLDİ ),(YAYINDA DEĞİL)
        // 4-  ABONELİK ( KAPALI OLARAK KABUL EDİLİR. ) 

            // AÇIK DURUMUNDA DBDE MAÇ KAYDI VARDIR VE OWNERID=1 DİR.
            // KAPALI DURUMDA DB'DE MAÇ KAYDI YOKTUR.
            // DOLU DURUMDA DB'DE MAÇ KAYDI VARDIR, OWNERID != 1' DİR.EĞER OWNERID = 27 İSE BU KİRALAMA HARBİ SAHADAN YAPILMAMIŞTIR.AKSİ 
            //TAKDİRDE HARBİ SAHADAN YAPILMIŞTIR.

        
       // İLK ETAPTA SEANSLAR KAPALI GELECEK.
       // AÇIK -> KAPALI GEÇİŞİ : DB'DEN GEREKLİ MAÇ İLAN KAYDI SİLİNİR.
       // KAPALI -> DOLU GEÇİŞİ : SADECE HALI SAHA PANELİ ÜZERİNDEN GERÇEKLEŞİR.YENİ BİR MAÇ KAYDI OLUŞTURULUR.OWNERID=27 'DİR.
       // AÇIK -> DOLU GEÇİŞİ : HEM PANEL ÜZERİNDEN HEM DE UYGULAMADAN GERÇEKLEŞİR.PANEL ÜZERİNDEN İSE OWNERID=27 YAPILIR VE KAPORA,FİYAT
              //BİLGİSİ GİRİLİR.UYGULAMA ÜZERİNDEN İSE ZATEN KAPORA ALINMIŞTIR VE OWNERID KİRALAYAN USER'IN ID'SİNE EŞİTLENİR.
       // KAPALI -> AÇIK GEÇİŞİ : DB'YE OWNERID=1 OLAN YENİ BİR MAÇ KAYDI ATILIR.
       // DOLU -> AÇIK GEÇİŞİ : OWNERID != 27 İSE BU MÜMKÜN DEĞİLDİR.OWNERID == 27 İSE OWNERID=1 YAPILIR VE KAPORA VE FİYAT BİLGİSİ GÜNCELLENİR.
       // DOLU -> KAPALI GEÇİŞİ : OWNERID != 27 İSE MÜMKÜN DEĞİLDİR.OWNERID== 27 İSE DB'DEN İLGİLİ KAYIT SİLİNİR.
       // ABONELİK DURUMUNDA İLİŞKİLİ SUBSCRIBE VARDIR VE KAPORA ALINMIŞ KABUL EDİLMEZ.  


    //METOD KARŞILIKLARI :
        //KAPALI = CLOSE
        //AÇIK = OPEN
        //DOLU = RENTED


        private async void Fill()
        {
            List<Announcement> allNotes = App.Current.Properties["Notes"] as List<Announcement>;
            List<Announcement> allSubs = App.Current.Properties["Subscribes"] as List<Announcement>;
            foreach (Announcement note in allNotes)
                if (note.FieldZoneId == fieldZoneId)
                    notes.Add(note);
            foreach (Announcement sub in allSubs)
                if (sub.FieldZoneId == fieldZoneId)
                    subs.Add(sub);

            

            matches = await service.GetMatches(fieldZoneId);
            today = DateTime.Today;
            Days.Add(today);
            for ( int i=1;i<21;i++)
            {
                Days.Add(today.AddDays(i));
            }

            currentDay = datePicker1.Date;
            if ( currentFieldZone.TimeMethod == "exact")
            {
                for ( int i=0;i<24;i++)
                {
                    viewModels.Add(new CalendarPageViewModel()
                    {
                        date = currentDay,
                        EnableSituation = true,
                        match = null,                       
                        TimeInt = i * 100,
                        
                    });
                    
                }
            }
            else if ( currentFieldZone.TimeMethod == "half" )
            {
                for (int i = 0; i < 24; i++)
                {
                    viewModels.Add(new CalendarPageViewModel()
                    {
                        date = currentDay,
                        EnableSituation = true,
                        match = null,
                        TimeInt = i * 100 + 30,
                        

                    });

                }
            }
            
            foreach(  Match match in matches)
            {
                if ( match.MatchOwnerId != 1)
                {

                    CalendarPageViewModel model = viewModels.Where(x => x.TimeInt == match.StartOn && x.date.Day == match.Day && x.date.Month == match.Month && x.date.Year == match.Year).FirstOrDefault();
                    if (model != null)
                    {
                        model.EnableSituation = true;
                        model.match = match;
                    }
                }
                else
                {
                    CalendarPageViewModel model = viewModels.Where(x => x.TimeInt == match.StartOn && x.date.Day == match.Day && x.date.Month == match.Month && x.date.Year == match.Year).FirstOrDefault();
                    if (model != null)
                    {
                        model.EnableSituation = true;
                        model.match = match;
                    }
                }
            }

            foreach (CalendarPageViewModel model in viewModels)
            {

                if (model.TimeInt / 100 <= DateTime.Now.Hour)
                {
                    model.EnableSituation = false;
                }
                if (notes.Count > 0)
                {
                    Announcement note = notes.Where(x => x.NoteDay == datePicker1.Date.Day && x.NoteMonth == datePicker1.Date.Month
                   && x.NoteYear == datePicker1.Date.Year && x.NoteMatchTime == model.TimeInt && x.FieldZoneId == currentFieldZone.Id).FirstOrDefault();
                    if (note != null)
                        model.Note = note;
                }
                if (subs.Count > 0)
                {
                    Announcement sub = subs.Where(x => x.SubscribeDayOfWeek == Convert.ToInt32(datePicker1.Date.DayOfWeek)  && x.SubscribeMatchTime == model.TimeInt).FirstOrDefault();
                    if (sub != null)
                    {
                        model.Subscribe = sub;
                        
                    }
                       
                }
            }


            colMatches.ItemsSource = viewModels;

            control.MakeUnVisible(ActivityLayout, activity);
            colMatches.IsVisible = true;
            isChancing = true;

        }

        private async void Refresh()
        {

        }

        private void DatePicker1_DateSelected(object sender, DateChangedEventArgs e)
        {
            notes.Clear();
            subs.Clear();
            List<Announcement> allNotes = App.Current.Properties["Notes"] as List<Announcement>;
            List<Announcement> allSubs = App.Current.Properties["Subscribes"] as List<Announcement>;
            foreach (Announcement note in allNotes)
                if (note.FieldZoneId == fieldZoneId)
                    notes.Add(note);
            foreach (Announcement sub in allSubs)
                if (sub.FieldZoneId == fieldZoneId)
                    subs.Add(sub);
           


            colMatches.ItemsSource = null;
            if (isButtonClicked == false)
                viewModels.Clear();
            //viewModels.Clear();
            currentDay = datePicker1.Date;
            if (currentFieldZone.TimeMethod == "exact")
            {
                for (int i = 0; i < 24; i++)
                {
                    viewModels.Add(new CalendarPageViewModel()
                    {
                        date = currentDay,
                        EnableSituation = true,
                        match = null,
                        TimeInt = i * 100,

                    });

                }
            }
            else if (currentFieldZone.TimeMethod == "half")
            {
                for (int i = 0; i < 24; i++)
                {
                    viewModels.Add(new CalendarPageViewModel()
                    {
                        date = currentDay,
                        EnableSituation = true,
                        match = null,
                        TimeInt = i * 100 + 30,


                    });

                }
            }
            currentDay = datePicker1.Date;
            foreach (Match match in matches)
            {
                if (match.MatchOwnerId != 1)
                {
                    CalendarPageViewModel model = viewModels.Where(x => x.TimeInt == match.StartOn && x.date.Day == match.Day && x.date.Month == match.Month && x.date.Year == match.Year).FirstOrDefault();
                    if (model != null)
                    {
                        model.EnableSituation = true;
                        model.match = match;
                    }
                }
                else
                {
                    CalendarPageViewModel model = viewModels.Where(x => x.TimeInt == match.StartOn && x.date.Day == match.Day && x.date.Month == match.Month && x.date.Year == match.Year).FirstOrDefault();
                    if (model != null)
                    {
                        model.EnableSituation = true;
                        model.match = match;
                    }
                }
            }
            if( datePicker1.Date.Date == DateTime.Today.Date)
            {
                foreach( CalendarPageViewModel model in viewModels)
                {
                    
                    if ( model.TimeInt/100 <= DateTime.Now.Hour)
                    {
                        model.EnableSituation = false;
                    }
                    if( notes.Count > 0)
                    {
                        Announcement note = notes.Where(x => x.NoteDay == datePicker1.Date.Day && x.NoteMonth == datePicker1.Date.Month
                       && x.NoteYear == datePicker1.Date.Year && x.NoteMatchTime == model.TimeInt && x.FieldZoneId == currentFieldZone.Id).FirstOrDefault();
                            if (note != null)
                                model.Note = note;
                    }
                    if ( subs.Count > 0)
                    {
                        Announcement sub = subs.Where(x => x.SubscribeDayOfWeek == Convert.ToInt32(datePicker1.Date.DayOfWeek) + 1 && x.SubscribeMatchTime == model.TimeInt).FirstOrDefault();
                        if (sub != null)
                            model.Subscribe = sub;
                    }
                        
                }
            }
            else
            {
                foreach (CalendarPageViewModel model in viewModels)
                {

                    if (notes.Count > 0)
                    {
                        Announcement note = notes.Where(x => x.NoteDay == datePicker1.Date.Day && x.NoteMonth == datePicker1.Date.Month
                       && x.NoteYear == datePicker1.Date.Year && x.NoteMatchTime == model.TimeInt).FirstOrDefault();
                        if (note != null)
                            model.Note = note;
                    }
                    if (subs.Count > 0)
                    {
                        Announcement sub = subs.Where(x => x.SubscribeDayOfWeek == Convert.ToInt32(datePicker1.Date.DayOfWeek)  && x.SubscribeMatchTime == model.TimeInt).FirstOrDefault();
                        if (sub != null)
                            model.Subscribe = sub;


                    }

                }
            }

            colMatches.ItemsSource = viewModels;
            control.MakeUnVisible(ActivityLayout, activity);
            colMatches.IsVisible = true;
            
        }

       
        //  S E L E C T I O N S --------------------------
        private async void ColMatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ( colMatches.SelectedItem != null)
            {
                CollectionView cv = (CollectionView)sender;
                CalendarPageViewModel model = e.CurrentSelection[0] as CalendarPageViewModel;
                string selection;
                if (model.EnableSituation == false)
                    return;
                //if ( model.Subscribe != null)
                //{
                //    selection = await DisplayActionSheet(model.date.ToString("dddd, dd MMMM ") + " " + model.Time, "Tamam", "Vazgeç", new string[] { "Gözat", "Bu saati uygun olarak işaretle", "Not Ekle" });
                //}
                if (model.match == null)
                {
                    selection = await DisplayActionSheet(model.date.ToString("dddd, dd MMMM ") + " " + model.Time, "Tamam", "Vazgeç", new string[] { "Bu saati uygun olarak işaretle", "Bu saati kiralandı olarak işaretle", "Gözat", "Not Ekle" });
                }
                else
                {


                    if (model.match.MatchOwnerId == 1)
                    {
                        if ( model.match.RandomLine1 == null || model.match.RandomLine1 == "None")
                        {
                            selection = await DisplayActionSheet(model.date.ToString("dddd, dd MMMM ") + " " + model.Time, "Tamam", "Vazgeç", new string[] {  "Bu saati kapalı olarak işaretle", "Gözat", "İndirim Uygula", "Not Ekle" });
                        }
                        else if ( model.match.RandomLine1 == "Discount" )
                        {
                            selection = await DisplayActionSheet(model.date.ToString("dddd, dd MMMM ") + " " + model.Time, "Tamam", "Vazgeç", new string[] { "Bu saati kiralandı olarak işaretle", "Bu saati kapalı olarak işaretle", "Gözat", "İndirimi iptal et", "Not Ekle" });
                        }
                        else
                        {
                            selection = await DisplayActionSheet(model.date.ToString("dddd, dd MMMM ") + " " + model.Time, "Tamam", "Vazgeç", new string[] { "Bu saati kiralandı olarak işaretle", "Bu saati kapalı olarak işaretle", "Gözat", "Not Ekle" });
                        }
                    }
                    else if ( model.match.MatchOwnerId == 27)
                    {
                        selection = await DisplayActionSheet(model.date.ToString("dddd, dd MMMM ") + " " + model.Time, "Tamam", "Vazgeç", new string[] { "Kapora ve fiyat düzenle", "Bu saati uygun olarak işaretle","Bu saati kapalı olarak işaretle","Gözat", "Not Ekle" });
                    }
                    else
                    {
                        selection = await DisplayActionSheet(model.date.ToString("dddd, dd MMMM ") + " " + model.Time, "Tamam", "Vazgeç", new string[] { "Gözat", "Not Ekle" });
                    }
                }
                //SELECTION'DAN SONRA

                if (selection == "Not Ekle")
                {
                    await Navigation.PushPopupAsync(new AddNotesPopupPage(model));
                }
                else if ( selection == "Gözat")
                {
                    await Navigation.PushPopupAsync(new MatchDetailPopupPage(model));
                }
                else if ( model.match != null)
                {
                    if ( selection == "Bu saati uygun olarak işaretle")
                    {
                        if ( model.Subscribe != null)
                        {
                            string insideSelection = await DisplayActionSheet("Bu saate abonelik ayarlanmış.Yine de uygun olarak işaretlemen istiyor musunuz?", "Vazgeç",null, new string[] { "Evet", "Hayır" });
                            if (insideSelection == "Evet")
                            {
                                await Navigation.PushPopupAsync(new BasePopupPage("İşleniyor..."));
                                SetOpenFromClose(model);
                            }
                        }
                        else
                        {
                            if ( model.match.MatchOwnerId == 27)
                            {
                                await Navigation.PushPopupAsync(new BasePopupPage("İşleniyor..."));
                                SetOpenFromRented(model);
                            }
                           
                        }
                    }
                    else if ( selection == "Bu saati kiralandı olarak işaretle")
                    {
                        await Navigation.PushPopupAsync(new DefineCostPopupPage(model));
                    }
                    else if ( selection == "Bu saati kapalı olarak işaretle")
                    {
                        if ( model.match.MatchOwnerId == 1)
                        {
                            await Navigation.PushPopupAsync(new BasePopupPage("İşleniyor..."));
                            SetCloseFromOpen(model);
                        }
                        else if ( model.match.MatchOwnerId == 27)
                        {
                            await Navigation.PushPopupAsync(new BasePopupPage("İşleniyor..."));
                            SetCloseFromRented(model);
                        }
                    }
                    else if ( selection == "Kapora ve fiyat düzenle")
                    {
                        if ( model.match.MatchOwnerId == 27)
                        {
                           // await Navigation.PushPopupAsync(new BasePopupPage("İşleniyor..."));
                            //FixCost(model);
                            await Navigation.PushPopupAsync(new DefineCostPopupPage(model));
                        }
                    }
                    else if ( selection == "İndirim Uygula")
                    {
                        if ( model.match.MatchOwnerId == 1)
                        {
                            
                            string percentage = await DisplayActionSheet("Uygulanacak indirimi seçin", "Vazgeç", null, new string[] { "%20 İndirim uygula", "%30 İndirim uygula","%40 İndirim uygula","%50 İndirim uygula" });
                            int _percentage = 0;
                            if (percentage == "%20 İndirim uygula")
                                _percentage = 20;
                            else if (percentage == "%30 İndirim uygula")
                                _percentage = 30;
                            else if (percentage == "%40 İndirim uygula")
                                _percentage = 40;
                            else if (percentage == "%50 İndirim uygula")
                                _percentage = 50;
                            if ( _percentage != 0 )
                            {
                                await Navigation.PushPopupAsync(new BasePopupPage("İşleniyor..."));
                                MakeDiscount(model,_percentage);
                            }

                        }
                    }
                    else if ( selection == "İndirimi iptal et")
                    {
                        if ( model.match.MatchOwnerId == 1 && model.match.RandomLine1 != null && model.match.RandomLine1 != "none")
                        {
                            await Navigation.PushPopupAsync(new BasePopupPage("İşleniyor..."));
                            CancelDiscount(model);
                        }
                    }
                }
                else if ( model.match == null)
                {
                    if ( selection == "Bu saati uygun olarak işaretle")
                    {
                        await Navigation.PushPopupAsync(new BasePopupPage("İşleniyor..."));
                        SetOpenFromClose(model);
                    }
                    else if ( selection == "Bu saati kiralandı olarak işaretle")
                    {
                        
                        //SetRentedFromClose(model);
                        await Navigation.PushPopupAsync(new DefineCostPopupPage(model));
                    }
                }
                colMatches.SelectedItem = null;
               
            }
        }

        private async void CancelDiscount(CalendarPageViewModel model)
        {
            Match match = await service.CancelDiscount(model.date.Day, model.date.Month, model.date.Year, model.TimeInt);
            if (match.Id == 0)
            {
                await DisplayAlert("Hata", "Bir sorun oluştu", "Tamam");
            }
            else
            {
                //matches.Add(match);,
                Match updated = matches.Where(x => x.Id == model.match.Id).FirstOrDefault();
                matches.Remove(updated);
                matches.Add(match);
                DatePicker1_DateSelected(datePicker1, new DateChangedEventArgs(datePicker1.Date, datePicker1.Date));
                await Navigation.PopAllPopupAsync();
                await DisplayAlert("Başarılı", "İndirim iptal edildi!", "Tamam");

            }

        }

        private async void SetRentedFromClose(Dictionary<int,int> dict)
        {
            await Navigation.PushPopupAsync(new BasePopupPage("İşleniyor..."));
            int halfPrice = dict.ElementAt(0).Key;
            int fullPrice = dict.ElementAt(0).Value;
            int day = dict.ElementAt(1).Value;           
            int month = dict.ElementAt(2).Value;
            int year = dict.ElementAt(3).Value;
            int time = dict.ElementAt(4).Value;

            Match match = await service.SetRentedFromClose(day, month, year, time, halfPrice, fullPrice);
            if ( match == null || match.Id == 0)
            {
                await DisplayAlert("Hata", "Bir sorun oluştu", "Tamam");
            }
            else
            {
                matches.Add(match);
                DatePicker1_DateSelected(datePicker1, new DateChangedEventArgs(datePicker1.Date, datePicker1.Date));
                await Navigation.PopAllPopupAsync();
                await DisplayAlert("Kaydedildi", "Değişiklikler kaydedildi!", "Tamam");

            }

           
        }

        private async void MakeDiscount(CalendarPageViewModel model,int percentage)
        {
            Match match = await service.MakeDiscount(model.date.Day, model.date.Month, model.date.Year, model.TimeInt,percentage);
            if (match.Id == 0)
            {
                await DisplayAlert("Hata", "Bir sorun oluştu", "Tamam");
            }
            else
            {
                //matches.Add(match);,
                Match updated = matches.Where(x => x.Id == model.match.Id).FirstOrDefault();
                matches.Remove(updated);
                matches.Add(match);
                DatePicker1_DateSelected(datePicker1, new DateChangedEventArgs(datePicker1.Date, datePicker1.Date));
                await Navigation.PopPopupAsync();
                await DisplayAlert("Başarılı", "İndirim uygulandı!", "Tamam");

            }
        }

        private async void FixCost(Dictionary<int,int> dict)
        {
            int halfPrice = dict.ElementAt(0).Key;
            int fullPrice = dict.ElementAt(0).Value;
            int day = dict.ElementAt(1).Value;
            int month = dict.ElementAt(2).Value;
            int year = dict.ElementAt(3).Value;
            int time = dict.ElementAt(4).Value;
            int matchId = dict.ElementAt(5).Value;

            Match updated = await service.FixCost(day, month, year, time, halfPrice, fullPrice);
            // Match match = await service.SetRentedFromClose(day, month, year, time, halfPrice, fullPrice);
            Match match = matches.Where(x => x.Id == matchId).FirstOrDefault();

            if (match == null || match.Id == 0)
            {
                await DisplayAlert("Hata", "Bir sorun oluştu", "Tamam");
            }
            else
            {
                matches.Remove(match);
                matches.Add(updated);
                DatePicker1_DateSelected(datePicker1, new DateChangedEventArgs(datePicker1.Date, datePicker1.Date));
                await Navigation.PopAllPopupAsync();
                await DisplayAlert("Kaydedildi", "Değişiklikler kaydedildi!", "Tamam");

            }

        }

        private async void SetCloseFromRented(CalendarPageViewModel model)
        {
            Match match = await service.SetCloseFromRented(model.date.Day, model.date.Month, model.date.Year, model.TimeInt);
            if (match.Id == 0)
            {
                await DisplayAlert("Hata", "Bir sorun oluştu", "Tamam");
            }
            else
            {
                //matches.Add(match);,
                Match updated = matches.Where(x => x.Id == model.match.Id).FirstOrDefault();
                matches.Remove(updated);
               // matches.Add(match);
                DatePicker1_DateSelected(datePicker1, new DateChangedEventArgs(datePicker1.Date, datePicker1.Date));
                await Navigation.PopPopupAsync();
                await DisplayAlert("Başarılı", "Değişiklikler kaydedildi!", "Tamam");

            }
        }

        private async void SetOpenFromRented(CalendarPageViewModel model)
        {
            Match match = await service.SetOpenFromRented(model.date.Day, model.date.Month, model.date.Year, model.TimeInt);
            if (match.Id == 0)
            {
                await DisplayAlert("Hata", "Bir sorun oluştu", "Tamam");
            }
            else
            {
                //matches.Add(match);,
                Match updated = matches.Where(x => x.Id == model.match.Id).FirstOrDefault();
                matches.Remove(updated);
                matches.Add(match);
                DatePicker1_DateSelected(datePicker1, new DateChangedEventArgs(datePicker1.Date, datePicker1.Date));
                await Navigation.PopPopupAsync();
                await DisplayAlert("Başarılı", "Bu saat artık kiralanmaya uygun!", "Tamam");

            }
        }

        private async void SetOpenFromClose(CalendarPageViewModel model)
        {
            Match match = new Match();
            match = await service.SetCloseToOpen(model.date.Year,model.date.Month,model.date.Day,model.TimeInt);
            try
            {
                if (match.Id == 0)
                {
                    await DisplayAlert("Hata", "Bir sorun oluştu", "Tamam");
                }
                else
                {
                    matches.Add(match);
                    DatePicker1_DateSelected(datePicker1,new DateChangedEventArgs(datePicker1.Date,datePicker1.Date));
                    await Navigation.PopPopupAsync();
                    await DisplayAlert("Başarılı", "Bu saat artık kiralanmaya uygun!", "Tamam");
                    
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Hata", "Lütfen internet bağlantınızı kontrol edin.", "Tamam");               
            }
        }

        private async void SetCloseFromOpen(CalendarPageViewModel model)
        {
            string response = "Hata";
            response = await service.SetOpenToClose(model.date.Year, model.date.Month, model.date.Day, model.TimeInt);
            try
            {
                if (response != "Başarılı")
                {
                    await DisplayAlert("Hata", response, "Tamam");
                }
                else
                {
                    matches.Remove(model.match);
                    DatePicker1_DateSelected(datePicker1, new DateChangedEventArgs(datePicker1.Date, datePicker1.Date));
                    await Navigation.PopPopupAsync();
                    await DisplayAlert("Başarılı", "İşlem Başarılı", "Tamam");

                }
            }
            catch (Exception)
            {
                await DisplayAlert("Hata", "Lütfen internet bağlantınızı kontrol edin.", "Tamam");
            }
        }

        private void PickerFieldZone_SelectedIndexChanged(object sender, EventArgs e)
        {
           if ( isChancing)
            {
                control.MakeVisible(ActivityLayout, activity);
                string fzName;
                FieldZone fz1 = new FieldZone();
                fzName = pickerFieldZone.SelectedItem as string;
                fz1 = fieldZones.Where(x => x.FieldZoneName == fzName).FirstOrDefault();
                colMatches.ItemsSource = null;

                // currentFieldZone = fz;
                Application.Current.Properties["currentFieldZone"] = fz1;

                Days.Clear();
                viewModels.Clear();
                notes.Clear();
                subs.Clear();
                matches.Clear();
               // fieldZoneNames.Clear();
               // fieldZones.Clear();

                currentField = Application.Current.Properties["loggedField"] as Field;
                fieldZones = currentField.FieldZones;
                foreach (FieldZone fz in fieldZones)
                    fieldZoneNames.Add(fz.FieldZoneName);
                pickerFieldZone.ItemsSource = fieldZoneNames;
                currentFieldZone = Application.Current.Properties["currentFieldZone"] as FieldZone;
                fieldZoneId = currentFieldZone.Id;
                pickerFieldZone.SelectedItem = currentFieldZone.FieldZoneName;

                datePicker1.Date = DateTime.Today;
                datePicker1.Format = " dddd dd/MM";
                datePicker1.MinimumDate = DateTime.Today;
                datePicker1.MaximumDate = DateTime.Today.AddDays(21);
                if (datePicker1.Date == today)
                {
                    //btnPrev.IsEnabled = false;
                }
                //btnPrev.IsEnabled = false;
                Fill();
              
            }



        }
    }
}