using System;
using System.Collections.Generic;
using System.Text;

namespace HarbiSahaYoneticiApp.Models
{
    public class RateFormField
    {
       
        public int Id { get; set; }

        public string Type { get; set; }  // ForUser,ForField

        public string Rate { get; set; }

        public string Comment { get; set; }

        public int FieldId { get; set; }

        public virtual Field Field { get; set; }  // DEĞERLENDİRMESİ ALINAN FIELD

        public string OwnerUserEmail { get; set; }

        public int OwnerUserId { get; set; }

        //public int FieldRate { get; set; }

        public DateTime CreatedOn { get; set; }

        public string RandomLine1 { get; set; }

        public string RandomLine2 { get; set; }

        public string RandomLine3 { get; set; }

        public string RandomLine4 { get; set; }

        public string RandomLine5 { get; set; }

        public string RandomLine6 { get; set; }

    }
}
