using System;
using System.Collections.Generic;
using System.Text;

namespace HarbiSahaYoneticiApp.Models
{
    public class FieldZone
    {

       
        public int Id { get; set; }

        public string FieldZoneName { get; set; }

        public int FieldId { get; set; }

        public virtual Field Field { get; set; }

        public virtual List<Match> Matches { get; set; }

        
        public string TimeMethod { get; set; } // half or exact

        public int PaymentModel1SmallCost { get; set; }

        public int PaymentModel2SmallCost { get; set; }

        public int PaymentModel3SmallCost { get; set; }

        public int PaymentModel4SmallCost { get; set; }

        public int PaymentModel1FullCost { get; set; }

        public int PaymentModel2FullCost { get; set; }

        public int PaymentModel3FullCost { get; set; }

        public int PaymentModel4FullCost { get; set; }

        public int PaymentModel1TimeStarted { get; set; }

        public int PaymentModel2TimeStarted { get; set; }

        public int PaymentModel3TimeStarted { get; set; }

        public int PaymentModel4TimeStarted { get; set; }




        public FieldZone()
        {
            Matches = new List<Match>();
        }

    }
}
