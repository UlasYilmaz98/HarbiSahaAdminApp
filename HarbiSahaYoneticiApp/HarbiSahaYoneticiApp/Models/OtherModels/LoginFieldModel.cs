using System;
using System.Collections.Generic;
using System.Text;

namespace HarbiSahaYoneticiApp.Models.OtherModels
{
    public class LoginFieldModel
    {
        public Field currentField { get; set; }

        public List<Announcement> NotesAndSubscribes { get; set; }

        public string token { get; set; }

        //public TeamPlayer currentTeamPlayer { get; set; }

    }
}
