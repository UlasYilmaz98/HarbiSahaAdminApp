using System;
using System.Collections.Generic;
using System.Text;

namespace HarbiSahaYoneticiApp.Models.ViewModels
{
    public class DayOfTime
    {
        public DateTime date { get; set; }

        public string dayStr
        {
            get
            {
                //DateTime matchDay = new DateTime(PlayerAdvert.Year, PlayerAdvert.Month, PlayerAdvert.Day);

                return date.ToString("dddd, dd MMMM ");
            }
        }


    }
}
