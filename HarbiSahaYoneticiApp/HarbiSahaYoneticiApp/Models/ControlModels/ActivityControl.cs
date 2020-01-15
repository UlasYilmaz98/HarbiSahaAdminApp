using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HarbiSahaYoneticiApp.Models.ControlModels
{
    public class ActivityControl
    {
        public void MakeUnVisible(StackLayout activityLayout, ActivityIndicator activity)
        {
            activityLayout.IsVisible = false;
            activity.IsRunning = false;
            activity.IsEnabled = false;
            activity.IsVisible = false;

        }

        public void MakeVisible(StackLayout activityLayout, ActivityIndicator activity)
        {
            activityLayout.IsVisible = true;
            activity.IsRunning = true;
            activity.IsEnabled = true;
            activity.IsVisible = true;

        }

        //public async Task MakeUnvisibleaAsync()
        //{
        //    activityLayout.IsVisible = false;
        //    activity.IsRunning = false;
        //    activity.IsEnabled = false;
        //    activity.IsVisible = false;
        //}


    }
}
