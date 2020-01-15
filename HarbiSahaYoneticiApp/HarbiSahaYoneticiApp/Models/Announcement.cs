using System;
using System.Collections.Generic;
using System.Text;

namespace HarbiSahaYoneticiApp.Models
{
    public class Announcement
    {

        //ABONELİK VEYA NOT OLARAK KULLANILACAK

        public int Id { get; set; }

        public string Type { get; set; }  //  VERİ TİPİ: SUBSCRIBE - NOTE - ANNOUNCEMENT

        public int FieldZoneId { get; set; }

        public int SubscribeMatchTime { get; set; } //  ABONELİK İÇİN MATCHTIME

        public int NoteMatchTime { get; set; }  // NOT İÇİN MATCHTIME

        public string NoteText { get; set; } // NOT İÇİN NOT METNİ

        public int SubscribeDayOfWeek { get; set; }  // ABONELİK İÇİN HAFTANIN KAÇINCI GÜNÜ OLDUĞUNU TUTAR 

        public int NoteDay { get; set; } // NOT İÇİN DAY

        public int NoteMonth { get; set; } // NOT İÇİN MONTH

        public int NoteYear { get; set; }  // NOT İÇİN YEAR

    }
}
