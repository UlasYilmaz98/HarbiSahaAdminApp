﻿using Rg.Plugins.Popup.Extensions;
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
    public partial class BasePopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public BasePopupPage(string text)
        {
            InitializeComponent();
            lblText.Text = text;
        }

    }
}