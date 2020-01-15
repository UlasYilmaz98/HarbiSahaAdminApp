using HarbiSahaYoneticiApp.Models;
using HarbiSahaYoneticiApp.Models.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HarbiSahaYoneticiApp.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DefineCostPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        CalendarPageViewModel mainModel = new CalendarPageViewModel();
        public DefineCostPopupPage(CalendarPageViewModel model)
        {

            FieldZone zone = App.Current.Properties["currentFieldZone"] as FieldZone;
            mainModel = model;
            

            InitializeComponent();
            lblDate1.Text = model.dayStr;
            if ( model.match == null)
            {
                entryKapora.Text = "0";
                entryPrice.Text = zone.PaymentModel1FullCost.ToString();
            }
            else
            {
                if (model.match.RandomLine3 != null)
                {
                    entryKapora.Text = model.match.RandomLine3;
                }
                else
                    entryKapora.Text = "0";
                entryPrice.Text = model.match.RandomLine4;

            }
           
        }

        private async void TapSave_Tapped(object sender, EventArgs e)
        {
            if ( String.IsNullOrWhiteSpace(entryKapora.Text) || String.IsNullOrWhiteSpace(entryPrice.Text))
            {
                await DisplayAlert("Hata", "Alanlar boş geçilmemelidir.Kapora alınmadıysa 0 yazınız.", "Tamam");
                return;
            }
            int kapora = Convert.ToInt32(entryKapora.Text);
            int fiyat = Convert.ToInt32(entryPrice.Text);

            if (kapora < fiyat || kapora > 500 || fiyat > 500) 
                await DisplayAlert("HATA", "Lütfen alanları kontrol ediniz", "Tamam");
            else
            {
                Dictionary<int, int> kaporaVeFiyat = new Dictionary<int, int>();
                //EĞER MAÇ YOKSA 4 KEY-VALUE VAR ONLAR DA FİYAT.
                kaporaVeFiyat.Add(kapora, fiyat);
                kaporaVeFiyat.Add(1000, mainModel.date.Day);
                kaporaVeFiyat.Add(1001, mainModel.date.Month);
                kaporaVeFiyat.Add(1002, mainModel.date.Year);
                kaporaVeFiyat.Add(1003, mainModel.TimeInt);
                if (mainModel.match != null)
                    kaporaVeFiyat.Add(1004, mainModel.match.Id);
               
                MessagingCenter.Send(this, "CalendarPage", kaporaVeFiyat);
            }
            
        }

        private async void TapCancel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(entryKapora.Text) || String.IsNullOrWhiteSpace(entryPrice.Text))
            {
                await DisplayAlert("Hata", "Alanlar boş geçilmemelidir.Kapora alınmadıysa 0 yazınız.", "Tamam");
                return;
            }
            int kapora = Convert.ToInt32(entryKapora.Text);
            int fiyat = Convert.ToInt32(entryPrice.Text);

            if (kapora > fiyat || kapora > 500 || fiyat > 500)
                await DisplayAlert("HATA", "Lütfen alanları kontrol ediniz", "Tamam");
            else
            {
                Dictionary<int, int> kaporaVeFiyat = new Dictionary<int, int>();
                //EĞER MAÇ YOKSA 4 KEY-VALUE VAR ONLAR DA FİYAT.
                kaporaVeFiyat.Add(kapora, fiyat);
                kaporaVeFiyat.Add(1000, mainModel.date.Day);
                kaporaVeFiyat.Add(1001, mainModel.date.Month);
                kaporaVeFiyat.Add(1002, mainModel.date.Year);
                kaporaVeFiyat.Add(1003, mainModel.TimeInt);
                if (mainModel.match != null)
                    kaporaVeFiyat.Add(1004, mainModel.match.Id);

                MessagingCenter.Send(this, "CalendarPage", kaporaVeFiyat);
                //await Navigation.PopPopupAsync();
            }
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}