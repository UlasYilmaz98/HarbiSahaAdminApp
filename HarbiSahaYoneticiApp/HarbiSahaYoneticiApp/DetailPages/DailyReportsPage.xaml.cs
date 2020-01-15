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
    public partial class DailyReportsPage : ContentPage
    {
        List<Purchase> purchases = new List<Purchase>();
        Field currentField = new Field();
        List<Match> matches = new List<Match>();
        ReportServices service = new ReportServices();
        
        //int selectedDay;
        //int selectedMonth;
        //int selectedYear;

        public DailyReportsPage()
        {
            currentField = App.Current.Properties["loggedField"] as Field;
            
            DateTime minDate = new DateTime();
            minDate = Convert.ToDateTime(currentField.RandomLine1);

            InitializeComponent();
            Fill();
            datePicker1.Date = DateTime.Today;
            datePicker1.Format = " dddd dd/MM";
            datePicker1.MinimumDate = minDate;
            datePicker1.MaximumDate = DateTime.Today;
           
        }

        private async void Fill()
        {
            int TotalIncome = 0, countTotalRenting = 0, countTotalRentingHS = 0;
            matches.Clear();
            await Navigation.PushPopupAsync(new BasePopupPage("Rapor oluşturuluyor..."));
            DateTime selectedDay = datePicker1.Date;
            matches = await service.GetDailyMatches(selectedDay.Year, selectedDay.Month, selectedDay.Day);
            
            if ( matches != null && matches.Count > 0)
            {
                foreach (Match match in matches  )
                {
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

                }

            }
            lblDailyTotalIncome.Text = TotalIncome.ToString() + " TL";
            lblDailyTotalRenting.Text = countTotalRenting.ToString() + " adet";
            lblDailyTotalRentingHS.Text = countTotalRentingHS.ToString() + " adet";
            await Navigation.PopPopupAsync();
        }

        private void DatePicker1_DateSelected(object sender, DateChangedEventArgs e)
        {
            Fill();
        }

        private void BtnNext_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnPrev_Clicked(object sender, EventArgs e)
        {

        }
    }
}