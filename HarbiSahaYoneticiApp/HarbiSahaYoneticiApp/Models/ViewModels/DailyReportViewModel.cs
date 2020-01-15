using System;
using System.Collections.Generic;
using System.Text;

namespace HarbiSahaYoneticiApp.Models.ViewModels
{
    public class DailyReportViewModel
    {
        public Field Field { get; set; }

        public List<Purchase> purchases { get; set; }

        //public int DailyIncomeHalf(DateTime date)
        //{
        //    int total = 0;
        //    foreach( Purchase purch in purchases)
        //    {
        //        if ( purch.Day == date.Day && purch.Month == date.Month && purch.Year == date.Year)
        //        {
        //            total += purch.Cost;
        //        }
        //    }
        //}

    }
}
