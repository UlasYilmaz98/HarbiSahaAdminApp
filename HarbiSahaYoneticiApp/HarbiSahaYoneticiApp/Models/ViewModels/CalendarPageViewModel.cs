using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HarbiSahaYoneticiApp.Models.ViewModels
{
    public class CalendarPageViewModel
    {
        public Announcement Subscribe { get; set; }
        public Match match { get; set; }

        public Color BgColor
        {
            get
            {
                if ( Subscribe != null && match == null)
                {
                    return Color.FromHex("#828282");
                }
                else if ( match != null)
                {
                    if( match.MatchOwnerId == 1)
                    {
                        return Color.FromHex("#428f54");
                    }
                    else if (match.MatchOwnerId == 27)
                    {
                        return Color.FromHex("#616161");
                    }
                    else
                    {
                        return Color.FromHex("#616161");
                    }
                }
                else
                {
                    return Color.FromHex("#828282");
                }
            }
        }

        public Color TextColor
        {
            get
            {
                if (Subscribe != null)
                {
                    return Color.White;
                }
                else if(match != null)
                {
                    if (match.MatchOwnerId == 1)
                    {
                        return Color.White;
                    }
                    else
                    {
                        return Color.White;
                    }
                }
                else
                {
                    return Color.White;
                }
            }
        }

        public int TimeInt { get; set; }

        public string Time {

            get
            {
                int StartOn = TimeInt;

                int firstPart, secondPart;
                string firstPartStr, secondPartStr;
                if (StartOn < 1000)
                {
                    if (StartOn == 0)
                    {
                        firstPartStr = "00";
                        secondPartStr = "00";
                    }
                    else if (StartOn < 100)
                    {
                        firstPartStr = "00";
                        secondPartStr = StartOn.ToString();
                    }
                    else
                    {
                        firstPart = StartOn / 100;
                        secondPart = StartOn % 100;
                        firstPartStr = "0" + firstPart.ToString();

                        if (secondPart == 0)
                            secondPartStr = "00";
                        else
                            secondPartStr = secondPart.ToString();
                    }

                }
                else
                {
                    firstPart = StartOn / 100;
                    secondPart = StartOn % 100;
                    firstPartStr = firstPart.ToString();

                    if (secondPart == 0)
                        secondPartStr = "00";
                    else
                        secondPartStr = secondPart.ToString();
                }
                return firstPartStr + "." + secondPartStr;


            }

        }

        public bool EnableSituation { get; set; }

        public double Opacity
        {
            get
            {
                if ( EnableSituation == true )
                {
                    return 1.0;
                }
                else
                {
                    return 0.3;
                }
            }
        }

        public DateTime date { get; set; }

        public string dayStr
        {
            get
            {
                //DateTime matchDay = new DateTime(PlayerAdvert.Year, PlayerAdvert.Month, PlayerAdvert.Day);

                return date.ToString("dddd, dd MMMM ");
            }
        }
        //public int DayNo { get; set; }

        public string rentedFrom
        {
            get
            {
                if (match != null)
                {
                    if (match.MatchOwnerId == 1)
                    {
                        return "Yayında";
                    }
                    else if ( match.MatchOwnerId == 27)
                    {
                        return "Kiralandı";
                    }
                    else
                    {
                        return "Harbi Saha";
                    }
                }
                else
                {
                    return "Yayında Değil";
                }
            }
        }

        public string Situation
        {
            get
            {
                if ( Subscribe != null  )
                {
                    return " ABONE ";
                }               
                else if (match != null)
                {
                    
                    if (match.MatchOwnerId == 1)
                    {
                        return " UYGUN ";
                    }
                    else if ( match.MatchOwnerId == 27)
                    {
                        return " DOLU ";
                    }
                    else
                    {
                        return " DOLU ";
                    }
                }
                else
                {
                    return " KAPALI ";
                }
            }
        }

        

        public Announcement Note { get; set; }

       

        public bool haveNote
        {
            get
            {
                if (Note != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public bool haveSubscribe
        {
            get
            {
                if (Subscribe != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public bool haveDiscount
        {
            get
            {
                if ( match != null && match.MatchOwnerId==1 && match.RandomLine1 == "Discount")
                {
                    return true;
                }
                return false;
            }
        }

    }
}
