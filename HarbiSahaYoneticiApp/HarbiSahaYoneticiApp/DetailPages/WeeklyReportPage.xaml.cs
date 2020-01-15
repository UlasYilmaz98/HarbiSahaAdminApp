using HarbiSahaYoneticiApp.Models;
using HarbiSahaYoneticiApp.PopupPages;
using HarbiSahaYoneticiApp.ServiceController;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HarbiSahaYoneticiApp.DetailPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeeklyReportPage : ContentPage
    {
        class DayAndCount
        {
            public string Day { get; set; }

            public int Count { get; set; }
        }


        List<Purchase> purchases = new List<Purchase>();
        Field currentField = new Field();
        List<Match> matches = new List<Match>();
        ReportServices service = new ReportServices();
        List<string> months = new List<string>();
        List<string> pickerModel = new List<string>();

        public WeeklyReportPage()
        {
            InitializeComponent();
            currentField = App.Current.Properties["loggedField"] as Field;
            Fill();

        }
        private async void Fill()
        {
            
            lblWeeklyTotalIncome.Text = " ";
            lblWeeklyTotalRenting.Text = " ";
            lblWeeklyTotalRentingHS.Text = " ";
            lblMostDay.Text = " ";




            int TotalIncome = 0, countTotalRenting = 0, countTotalRentingHS = 0;
            DayAndCount mostDay = new DayAndCount();
            List<DayAndCount> countOfDays = new List<DayAndCount>()
            {
                new DayAndCount(){ Day = "Pazartesi",Count = 0 },
                new DayAndCount(){ Day = "Salı",Count = 0 },
                new DayAndCount(){ Day = "Çarşamba",Count = 0 },
                new DayAndCount(){ Day = "Perşembe",Count = 0 },
                new DayAndCount(){ Day = "Cuma",Count = 0 },
                new DayAndCount(){ Day = "Cumartesi",Count = 0 },
                new DayAndCount(){ Day = "Pazar",Count = 0 }

            };
            matches.Clear();
            await Navigation.PushPopupAsync(new BasePopupPage("Rapor oluşturuluyor..."));
            // DateTime selectedDay = datePicker1.Date;

            matches = await service.GetLastWeekMathes();

            if (matches != null && matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    DateTime matchTime = new DateTime(match.Year, match.Month, match.Day);
                    if (match.MatchOwnerId != 1 && match.MatchOwnerId == 27)
                    {
                        countTotalRenting++;
                        TotalIncome += Convert.ToInt32(match.RandomLine4);
                    }
                    if (match.MatchOwnerId != 1 && match.MatchOwnerId != 27)
                    {
                        int incomeInt = 0;
                        double _income = 0;

                        _income = Convert.ToInt32(match.RandomLine4) - (Convert.ToInt32(match.RandomLine3) * 2.5 / 100);
                        incomeInt = Convert.ToInt32(Math.Round(_income));
                        countTotalRenting++;
                        countTotalRentingHS++;
                        TotalIncome += incomeInt - 10;
                    }


                    switch (matchTime.DayOfWeek)
                    {
                        case DayOfWeek.Friday:
                            countOfDays[4].Count++; break;
                        case DayOfWeek.Monday:
                            countOfDays[0].Count++; break;
                        case DayOfWeek.Saturday:
                            countOfDays[5].Count++; break;
                        case DayOfWeek.Sunday:
                            countOfDays[6].Count++; break;
                        case DayOfWeek.Thursday:
                            countOfDays[3].Count++; break;
                        case DayOfWeek.Tuesday:
                            countOfDays[1].Count++; break;
                        case DayOfWeek.Wednesday:
                            countOfDays[2].Count++; break;
                        default:
                            break;
                    }


                }

                countOfDays = countOfDays.OrderByDescending(X => X.Count).ToList();
                mostDay = countOfDays[0];
                lblMostDay.Text = mostDay.Day + ", " + mostDay.Count.ToString() + " adet";



            }
            lblWeeklyTotalIncome.Text = TotalIncome.ToString() + " TL";
            lblWeeklyTotalRenting.Text = countTotalRenting.ToString() + " adet";
            lblWeeklyTotalRentingHS.Text = countTotalRentingHS.ToString() + " adet";

            await Navigation.PopAllPopupAsync();
        }
    }
}