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
    public partial class MonthlyReportsPage : ContentPage
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
        public MonthlyReportsPage()
        {
            currentField = App.Current.Properties["loggedField"] as Field;

          

            InitializeComponent();
            months.Add("Aralık 2018");
            months.Add("Ocak 2019");
            months.Add("Şubat 2019");
            months.Add("Mart 2019");
            months.Add("Nisan 2019");
            months.Add("Mayıs 2019");
            months.Add("Haziran 2019");
            months.Add("Temmuz 2019");
            months.Add("Ağustos 2019");
            months.Add("Eylül 2019");
            months.Add("Ekim 2019");
            months.Add("Kasım 2019");
            months.Add("Aralık 2019");
            for (int i = 0; i < DateTime.Now.Month; i++)
            {
                pickerModel.Add(months[i]);
            }
            datePicker1.ItemsSource = pickerModel;
            if (DateTime.Now.Month == 12)
                datePicker1.SelectedIndex = 0;
            else
                datePicker1.SelectedIndex = DateTime.Now.Month; 
            
            
            Fill();
            
        }

        private async void Fill()
        {
            int selectedYear = 0;
            lblDailyTotalIncome.Text = " ";
            lblDailyTotalRenting.Text = " ";
            lblDailyTotalRentingHS.Text = " ";
            lblMostDay.Text = " " ;

            int selectedMonth = 0;
            if (datePicker1.SelectedIndex == 0)
                selectedMonth = 12;
            else
                selectedMonth = datePicker1.SelectedIndex;



            selectedYear = 2020;
            if (selectedMonth == 12)
                selectedYear = 2019;
                
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
           
            matches = await service.GetMonthlyMatches(selectedYear, selectedMonth);

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
            lblDailyTotalIncome.Text = TotalIncome.ToString() + " TL";
            lblDailyTotalRenting.Text = countTotalRenting.ToString() + " adet";
            lblDailyTotalRentingHS.Text = countTotalRentingHS.ToString() + " adet";
            
            await Navigation.PopAllPopupAsync();
        }

        private void DatePicker1_DateSelected(object sender, DateChangedEventArgs e)
        {
            Fill();
        }

      

        private void DatePicker1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill();
        }
    }
}