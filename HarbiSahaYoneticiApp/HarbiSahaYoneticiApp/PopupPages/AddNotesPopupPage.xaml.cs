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
    public partial class AddNotesPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        CalendarPageViewModel mainModel = new CalendarPageViewModel();
        ManageServices service = new ManageServices();
        Announcement _note = new Announcement();
        int fieldZoneId = 1;
        public AddNotesPopupPage( CalendarPageViewModel model )
        {
            InitializeComponent();
            mainModel = model;
            lblDate.Text = model.dayStr;

            if( model.Note != null)
            {
                _note = model.Note;
                editorNote.Text = model.Note.NoteText;
            }
            
        }

        private async void TapSave_Tapped(object sender, EventArgs e)
        {
            
            List<Announcement> allNotes = App.Current.Properties["Notes"] as List<Announcement>;
           
            if ( String.IsNullOrEmpty(editorNote.Text))
            {
                await DisplayAlert("UYARI", "Not alanı boş geçilemez.", "Tamam");
                return;
            }
            await Navigation.PushPopupAsync(new BasePopupPage("İşleniyor..."));
            if ( mainModel.haveNote )
            {
                //Announcement note = allNotes.Where(x => x.Type == "Note" && x.FieldZoneId == _note.FieldZoneId && x.NoteDay == _note.NoteDay && x.NoteMonth == _note.NoteMonth &&
                //   x.NoteYear == _note.NoteYear && x.NoteMatchTime == _note.NoteMatchTime).FirstOrDefault();
                _note = await service.AddNote(fieldZoneId, mainModel.date.Day, mainModel.date.Month, mainModel.date.Year, mainModel.TimeInt, 1, editorNote.Text);
                Announcement note = allNotes.Where(x => x.Type == "Note" && x.FieldZoneId == _note.FieldZoneId && x.NoteDay == _note.NoteDay && x.NoteMonth == _note.NoteMonth &&
                   x.NoteYear == _note.NoteYear && x.NoteMatchTime == _note.NoteMatchTime).FirstOrDefault();
                note.NoteText = _note.NoteText;
                App.Current.Properties["Notes"] = allNotes;

            }
            else
            {
                _note = await service.AddNote(fieldZoneId, mainModel.date.Day, mainModel.date.Month, mainModel.date.Year, mainModel.TimeInt, 0, editorNote.Text);
                Announcement note = _note;
                allNotes.Add(note);
                App.Current.Properties["Notes"] = allNotes;
            }
            await Navigation.PopAllPopupAsync();
            MessagingCenter.Send(this, "CalendarPage");

        }

        private async void TapCancel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}