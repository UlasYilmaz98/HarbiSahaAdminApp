using System;
using System.Collections.Generic;
using System.Text;

namespace HarbiSahaYoneticiApp.Models.ViewModels
{
    public class CurrentMatchesPageViewModel
    {

        public Match Match { get; set; }

        public Purchase Purchase {

            get
            {
                return Match.Purchases[0];
            }
        }

        public User User
        {
            get
            {
                return Match.MatchOwner;
            }
        }



        public DateTime MatchDay
        {
            get
            {
                DateTime toBeReturned;
                int virgul, tam;
                if ( Match.StartOn < 100)
                {
                    if ( Match.StartOn == 0 )
                    {
                        return new DateTime(Match.Year, Match.Month, Match.Day, 0, 0, 0);
                    }
                    else
                    {
                        return new DateTime(Match.Year, Match.Month, Match.Day, 0, Match.StartOn, 0);
                    }
                }
                else
                {
                    return new DateTime(Match.Year, Match.Month, Match.Day, Match.StartOn/100, Match.StartOn%100, 0);
                }
            }
        }

        public string DateStr
        {
            get
            {
                 return MatchDay.ToString("dddd, dd MMMM ");
            }
        }

        public string Time
        {

            get
            {
                int StartOn = Match.StartOn;

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

        public string UserName
        {
            get
            {
                return User.Name;
            }
        }

        public string UserNameText
        {
            get
            {
                return UserName + " isimli kullanıcı tarafından kiralandı";
            }
        }

        public string KaporaSituation {

            get
            {
                if (  Purchase != null)
                {
                    return Purchase.Cost.ToString() + " TL Ödendi";
                }
                else
                {
                    return "Ödeme yapılmadı";
                }
            }

        }

        public string OnTheFieldSituation
        {
            get
            {
                int payed = 0;
                int total = Convert.ToInt32(Match.RandomLine4);
                if ( Purchase != null)
                {
                    return (total - payed).ToString() + " TL sahada ödenecek";
                }
                return total.ToString() + " TL sahada ödenecek";
            }
        }

        public bool checkVisible {
            get
            {
                if (Match.Purchases.Count < 0)
                    return true;
                else
                    return false;
            }
        }


    }
}
