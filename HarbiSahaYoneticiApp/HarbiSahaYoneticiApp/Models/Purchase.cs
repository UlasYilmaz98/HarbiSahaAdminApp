using System;
using System.Collections.Generic;
using System.Text;

namespace HarbiSahaYoneticiApp.Models
{
    public class Purchase
    {
        
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int Cost { get; set; }

        public bool isPaid { get; set; }

        public int Day { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int MatchId { get; set; }

        public virtual Match Match { get; set; }

        public int FieldId { get; set; }

        public virtual Field Field { get; set; }

        public bool isReturned { get; set; }

        public bool isApproved { get; set; }

        public bool isDelivered { get; set; }

        public string RandomLine1 { get; set; }

        public string RandomLine2 { get; set; }

        public string RandomLine3 { get; set; }

        public string RandomLine4 { get; set; }

        public string RandomLine5 { get; set; }

        public string RandomLine6 { get; set; }

        public DateTime PurchasedOn { get; set; }

        public bool isFormPrepared { get; set; }

    }
}
