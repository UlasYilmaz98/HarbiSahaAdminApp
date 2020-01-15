using System;
using System.Collections.Generic;
using System.Text;

namespace HarbiSahaYoneticiApp.Models
{
    public class User
    {
        
        public int Id { get; set; }

       

        public string Name { get; set; }

        public string Surname { get; set; }

      

       

        

        public int BirthYear { get; set; }

        public int BirthMonth { get; set; }

        public int BirthDay { get; set; }

     

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string PhotoPath { get; set; }

        
        //public virtual Team Team { get; set; }

        //public int TeamId { get; set; }

      
        public virtual List<Purchase> Purchases { get; set; }



        

       


        public User()
        {
            Purchases = new List<Purchase>();
            
        }


    }
}
