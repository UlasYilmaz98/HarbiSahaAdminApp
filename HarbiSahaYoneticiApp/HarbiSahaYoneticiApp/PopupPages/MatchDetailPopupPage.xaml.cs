using HarbiSahaYoneticiApp.Models;
using HarbiSahaYoneticiApp.Models.ViewModels;
using HarbiSahaYoneticiApp.ServiceController;
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
    public partial class MatchDetailPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        CalendarPageViewModel mainModel = new CalendarPageViewModel();
        Match mainMatch = new Match();
        ReportServices service = new ReportServices();
        User ownerUser = new User();
        List<Announcement> notes = new List<Announcement>();
        public MatchDetailPopupPage(CalendarPageViewModel model)
        {
            InitializeComponent();
            mainModel = model;
            lblDate.Text = model.dayStr;
            if ( App.Current.Properties.ContainsKey("Notes"))
            {
                notes = App.Current.Properties["Notes"] as List<Announcement>;
                Announcement note = notes.Where(x => x.NoteDay == model.date.Day && x.NoteMonth == model.date.Month &&
               x.NoteYear == model.date.Year && x.NoteMatchTime == model.TimeInt).FirstOrDefault();
                if (note != null)
                    lblNote.Text = note.NoteText;


            }                
            if (model.match != null)               
            {
                mainMatch = model.match;
                if ( model.match.MatchOwnerId != 27 && model.match.MatchOwnerId != 1 )
                {
                    FillHS();
                }
                else if ( model.match.MatchOwnerId == 27)
                {
                    lblUser.Text = "-";
                    lblUserEmail.Text = "-";
                    lblUserPhone.Text = "-";
                    if (String.IsNullOrEmpty(model.match.RandomLine3) || model.match.RandomLine3 == "0")
                        lblSituationKapora.Text = "Ödenmedi";
                    else
                        lblSituationKapora.Text = "Ödendi, " + model.match.RandomLine3 + " TL";
                    lblUserPhone.Text = "-";
                    lblSituation.Text = "Normal kiralandı.";                   
                }
                else if ( model.match.MatchOwnerId == 1)
                {
                    lblUser.Text = "-";
                    lblUserEmail.Text = "-";
                    lblUserPhone.Text = "-";
                    lblSituationKapora.Text = "Ödenmedi";
                    lblUserPhone.Text = "-";
                    lblSituation.Text = "Online kiralamaya uygun, yayında.Kapora:" + model.match.RandomLine3 + " TL. Toplam:" + model.match.RandomLine4 + " TL";
                    
                }
            }
            else
            {
                lblUser.Text = "-";
                lblUserEmail.Text = "-";
                lblUserPhone.Text = "-";
                lblSituationKapora.Text = "Ödenmedi";
                lblUserPhone.Text = "-";
                lblSituation.Text = "Belirtilmemiş, online kiralamaya açık değil.";
            }
        }

        private async void FillHS()
        {
            layoutMain.IsVisible = false;
            ActivityLayout.IsVisible = true;
            //BasePopupPage waiting = new BasePopupPage("Yükleniyor...");
            //await Navigation.PushPopupAsync(waiting);
            ownerUser = await service.GetSingleUser(mainModel.match.MatchOwnerId);
            lblUser.Text = ownerUser.Name;
            lblUserPhone.Text = ownerUser.PhoneNumber;
            lblSituationKapora.Text = "Ödendi," + mainModel.match.RandomLine3 + " TL";
            lblSituation.Text = "Online olarak kiralandı, Kapora:" + mainMatch.RandomLine3 + " TL , " + " Toplam:" + mainMatch.RandomLine4 + " TL";
            lblUserEmail.Text = ownerUser.Email;
            layoutMain.IsVisible = true;
            ActivityLayout.IsVisible = false;
            //MessagingCenter.Send(this, "BasePopupPage");
            //await Navigation.PopPopupAsync();
        }

        private async void BtnExit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        
    }
}