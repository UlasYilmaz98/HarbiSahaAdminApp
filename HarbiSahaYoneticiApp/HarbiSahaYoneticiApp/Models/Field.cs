using System;
using System.Collections.Generic;
using System.Text;
using HarbiSahaYoneticiApp.Models;

namespace HarbiSahaYoneticiApp.Models
{
    
    public class Field
    {
        
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string FieldName { get; set; }

        public string LocationPath { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string BaseAdress { get; set; }


        
        public string FieldInformation { get; set; }

        //public List<Match> Matches { get; set; }

        public virtual List<RateFormField> RateFormFields { get; set; }

        public virtual List<FieldZone> FieldZones { get; set; }

        public string PhotoPath1 { get; set; }

        public string PhotoPath2 { get; set; }

        public string PhotoPath3 { get; set; }

        public string PhotoPath4 { get; set; }

        public string PhotoPath5 { get; set; }

        public string PhotoPath6 { get; set; }

        public string PhotoPath7 { get; set; }

        public string PhotoPath8 { get; set; }
        public string RandomLine1 { get; set; }

        public string RandomLine2 { get; set; }

        public string RandomLine3 { get; set; }

        public string RandomLine4 { get; set; }

        public string RandomLine5 { get; set; }

        public string RandomLine6 { get; set; }

        public bool haveShower { get; set; }

        public bool haveFood { get; set; }

        public bool haveShoes { get; set; }

        public bool haveGloves { get; set; }

        public bool haveInternet { get; set; }

        public bool haveWC { get; set; }

        public bool haveCarPark { get; set; }

        public bool haveCafe { get; set; }

        public bool haveCamera { get; set; }

        public bool haveCreditCard { get; set; }

        public bool haveTribun { get; set; }

        public string ExtraProp1 { get; set; }

        public string ExtraProp2 { get; set; }

        public int CancelDays { get; set; }


        public Field()
        {
            //Matches = new List<Match>();
            RateFormFields = new List<RateFormField>();
            FieldZones = new List<FieldZone>();
        }

    }
}
