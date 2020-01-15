using System;
using System.Collections.Generic;
using System.Text;

namespace HarbiSahaYoneticiApp.Models.ViewModels
{
    public class SubscribesPageViewModel
    {

        public Announcement Subscribe { get; set; }

        public string Time {
            get
            {
                int StartOn = Subscribe.SubscribeMatchTime;

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

    }
}
