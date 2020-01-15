using System;
using System.Collections.Generic;
using System.Text;

namespace HarbiSahaYoneticiApp.Models
{
    public class Match
    {

        
        public int Id { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public bool isFromHarbiSaha { get; set; }

        public bool isPlayed { get; set; }

        public string FieldName { get; set; }

        public string BaseAdress { get; set; }

        public DateTime MatchStartOn { get; set; }

        public int MatchOwnerId { get; set; }

        public virtual User MatchOwner { get; set; }

        //public List<MatchPlayer> MatchPlayers { get; set; }

        //public virtual List<MatchTeam> MatchTeams { get; set; }

        //public virtual List<PlayerAdvert> PlayerAdverts { get; set; }

        //public virtual List<ChatChannel> ChatChannels { get; set; }

        //public virtual List<MatchPlayer> MatchPlayers { get; set; }

        //public virtual List<PlayerAdvert> PlayerAdverts { get; set; }

        public int FieldZoneId { get; set; }

        public virtual FieldZone FieldZone { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public int StartOn { get; set; }

        public int FinishOn { get; set; }

        //public virtual List<OpponentAdvert> OpponentAdverts { get; set; }

        public bool isOpponentFound { get; set; }

        public string RandomLine1 { get; set; } // Discount Or None

        public string RandomLine2 { get; set; } // DiscountPercentage

        public string RandomLine3 { get; set; }

        public string RandomLine4 { get; set; }

        public string RandomLine5 { get; set; }

        public string RandomLine6 { get; set; }

        public virtual List<Purchase> Purchases { get; set; }

        public Match()
        {
            //MatchPlayers = new List<MatchPlayer>();
            //MatchTeams = new List<MatchTeam>();
            //ChatChannels = new List<ChatChannel>();
            //PlayerAdverts = new List<PlayerAdvert>();
            //MatchPlayers = new List<MatchPlayer>();
            Purchases = new List<Purchase>();
        }

    }
}
