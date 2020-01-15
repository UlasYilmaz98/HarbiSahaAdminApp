using HarbiSahaYoneticiApp.Models;
using HarbiSahaYoneticiApp.Models.ViewModels;
using HarbiSahaYoneticiApp.ServiceController;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HarbiSahaYoneticiApp.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSubscribePopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        List<Announcement> subscribes = new List<Announcement>();
        FieldZone currentFieldZone = new FieldZone();
        List<string> dayList = new List<string>() { "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar" };
        List<Announcement> currentDaySubsList = new List<Announcement>();
        List<int> subscribedTimeList = new List<int>();
        List<string> timeList = new List<string>();
        ManageServices service = new ManageServices();
        public AddSubscribePopupPage()
        {
            InitializeComponent();
            currentFieldZone = Application.Current.Properties["currentFieldZone"] as FieldZone;
            pickerDays.ItemsSource = dayList;
            //PAZARTESİ GİBİ İŞLEM YAP.
            //if (Application.Current.Properties["Subscribes"] != null)
            //{
            //    subscribes.Clear();
            //    subscribes = Application.Current.Properties["Subscribes"] as List<Announcement>;
            //    foreach (Announcement sub in subscribes)
            //    {
            //        if (sub.FieldZoneId == currentFieldZone.Id && sub.SubscribeDayOfWeek == 1)
            //        {
            //            currentDaySubsList.Add(sub);
            //            subscribedTimeList.Add(sub.SubscribeMatchTime);
            //        }
            //    }
            //}
            //if (currentDaySubsList.Count > 0)
            //{
            //    if (currentFieldZone.TimeMethod == "exact")
            //    {
            //        for (int i = 0; i <= 2300; i += 100)
            //        {
            //            if (subscribedTimeList.Contains(i))
            //                continue;
            //            timeList.Add(convertTimeIntToStr(i));
            //        }
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i <= 2300; i += 100)
            //    {

            //        timeList.Add(convertTimeIntToStr(i));
            //    }
            //}
        }

        private void TapCancel_Tapped(object sender, EventArgs e)
        {

        }

        private async void TapCreate_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new BasePopupPage("İşleniyor..."));
            string selectedTime = pickerTimes.SelectedItem.ToString();
            int timeInt = convertTimeStrToInt(selectedTime);
            int selectedDay = 0;
            string selected = pickerDays.SelectedItem.ToString();
            switch (selected)
            {
                case "Pazartesi": selectedDay = 1; break;
                case "Salı": selectedDay = 2; break;
                case "Çarşamba": selectedDay = 3; break;
                case "Perşembe": selectedDay = 4; break;
                case "Cuma": selectedDay = 5; break;
                case "Cumartesi": selectedDay = 6; break;
                case "Pazar": selectedDay = 7; break;

            }
            try
            {
                Announcement sub = await service.CreateNewSubscribe(currentFieldZone.Id, selectedDay, timeInt);
                if (sub != null && sub.Type == "Subscribe")
                {
                    subscribes = App.Current.Properties["Subscribes"] as List<Announcement>;
                    subscribes.Add(sub);
                    MessagingCenter.Send(this, "SubscribesPage");
                    await Navigation.PopAllPopupAsync();
                }
                else
                {
                    await DisplayAlert("Hata", "Bir hata oluştu.", "Tamam");
                    await Navigation.PopAllPopupAsync();
                }
            }
            catch (Exception)
            {

                await DisplayAlert("Hata", "Lütfen internet bağlantınızı kontrol edin.", "Tamam");
                await Navigation.PopAllPopupAsync();
            }
            
           
        }

        
        private void PickerDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            pickerTimes.ItemsSource = null;
            timeList.Clear();
            subscribedTimeList.Clear();
            currentDaySubsList.Clear();
            int selectedDay = 0;
            string selected = pickerDays.SelectedItem.ToString();
            switch (selected)
            {
                case "Pazartesi": selectedDay = 1;break;
                case "Salı": selectedDay = 2; break;
                case "Çarşamba": selectedDay = 3; break;
                case "Perşembe": selectedDay = 4; break;
                case "Cuma": selectedDay = 5; break;
                case "Cumartesi": selectedDay = 6; break;
                case "Pazar": selectedDay = 7; break;

            }


            if (Application.Current.Properties["Subscribes"] != null)
            {
                //subscribes.Clear();
                subscribes = Application.Current.Properties["Subscribes"] as List<Announcement>;
                foreach (Announcement sub in subscribes)
                {
                    if (sub.FieldZoneId == currentFieldZone.Id && sub.SubscribeDayOfWeek == selectedDay)
                    {
                        currentDaySubsList.Add(sub);
                        subscribedTimeList.Add(sub.SubscribeMatchTime);
                    }
                }
            }
            if (currentDaySubsList.Count > 0)
            {
                if (currentFieldZone.TimeMethod == "exact")
                {
                    for (int i = 0; i <= 2300; i += 100)
                    {
                        if (subscribedTimeList.Contains(i))
                            continue;
                        timeList.Add(convertTimeIntToStr(i));
                    }
                }
                else
                {
                    for (int i = 30; i <= 2330; i += 100)
                    {
                        if (subscribedTimeList.Contains(i))
                            continue;
                        timeList.Add(convertTimeIntToStr(i));
                    }
                }
            }
            else
            {
                if (currentFieldZone.TimeMethod == "exact")
                {
                    for (int i = 0; i <= 2300; i += 100)
                    {
                        
                        timeList.Add(convertTimeIntToStr(i));
                    }
                }
                else
                {
                    for (int i = 30; i <= 2330; i += 100)
                    {
                       
                        timeList.Add(convertTimeIntToStr(i));
                    }
                }
            }
            pickerTimes.ItemsSource = timeList;
            pickerTimes.IsEnabled = true;
        }

        private int convertTimeStrToInt(string time)
        {
            string strToReturn;
            if (time == "00.00")
                return 0;
            else if (time[0] == '0' && time[1] == '0')
            {
                strToReturn = time[3].ToString() + time[4].ToString();
                return Convert.ToInt32(strToReturn);
            }
            else if (time[0] == '0')
            {
                strToReturn = time[1].ToString() + time[3].ToString() + time[4].ToString();
                return Convert.ToInt32(strToReturn);
            }
            else
            {
                strToReturn = time[0].ToString() + time[1].ToString() + time[3].ToString() + time[4].ToString();
                return Convert.ToInt32(strToReturn);
            }
        }

        private string convertTimeIntToStr(int time)
        {

            int StartOn = time;

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