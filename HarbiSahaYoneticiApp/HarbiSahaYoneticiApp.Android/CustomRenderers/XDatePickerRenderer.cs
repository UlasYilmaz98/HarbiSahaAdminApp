using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;

using HarbiSahaYoneticiApp.CustomControls;
using HarbiSahaYoneticiApp.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(XDatePicker), typeof(XDatePickerRenderer))]
namespace HarbiSahaYoneticiApp.Droid.CustomRenderers
{
    public class XDatePickerRenderer : DatePickerRenderer
    {
        public XDatePickerRenderer(Context context) : base(context)
        {

        }

        public static void Init() { }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {

            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;

                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);
            }
        }


    }
}