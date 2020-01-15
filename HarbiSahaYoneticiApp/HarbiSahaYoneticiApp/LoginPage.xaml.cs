using HarbiSahaYoneticiApp.Models;
using HarbiSahaYoneticiApp.Models.OtherModels;
using HarbiSahaYoneticiApp.PopupPages;
using HarbiSahaYoneticiApp.ServiceController;
using Plugin.SecureStorage;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HarbiSahaYoneticiApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        ManageServices service = new ManageServices();
        LoginFieldModel model = new LoginFieldModel();
        Field field = new Field();
        public LoginPage()
        {
            InitializeComponent();
            try
            {
                if (CrossSecureStorage.Current.HasKey("loggedFieldEmail") && CrossSecureStorage.Current.HasKey("loggedFieldPassword"))
                {
                    string email = CrossSecureStorage.Current.GetValue("loggedFieldEmail");
                    string password = CrossSecureStorage.Current.GetValue("loggedFieldPassword");
                    LoginFunc(email, password);
                }
            }
            catch (Exception ex)
            {

                
            }
        }

        private async void LoginFunc(string email,string password)
        {
            try
            {

                await Navigation.PushPopupAsync(new BasePopupPage("Giriş yapılıyor..."));
                //Field field = new Field();
                //field = App.Current.Properties["loggedField"] as Field;
                field = await service.Login(email,password);
                if (field != null)
                {
                    
                    App.Current.MainPage = new MainMasterDetailPage();
                    await Navigation.PopPopupAsync();
                }
                else
                {
                    
                    await DisplayAlert("HATA", "Bir hata oluştu.Lütfen tekrar giriş yapın.", "OK");
                    await Navigation.PopPopupAsync();
                    return;
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Bağlantı yok", "Lütfen internet bağlantınızı kontrol edin.", "OK");
            }
            
        }

        private void EntryEmail_Unfocused(object sender, FocusEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(entryEmail.Text))
                entryPassword.Focus();
        }

        private void PhoneCallMethod(object sender, EventArgs e)
        {

        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            if ( String.IsNullOrWhiteSpace(entryEmail.Text) || String.IsNullOrWhiteSpace(entryPassword.Text))
            {
                await DisplayAlert("UYARI", "E-posta ve şifre alanı boş geçilemez", "OK");
            }
            else if ( !entryEmail.Text.Contains("@") || entryEmail.Text.Length <3 || entryPassword.Text.Length < 4 )
            {
                await DisplayAlert("UYARI", "Lütfen geçerli bir e-posta adresi ve şifre giriniz.","OK");
            }
            else
            {
                await Navigation.PushPopupAsync(new BasePopupPage("Giriş yapılıyor..."));
                field = await service.Login(entryEmail.Text, entryPassword.Text);
                if ( field != null)
                {
                    CrossSecureStorage.Current.SetValue("loggedFieldId", field.Id.ToString());
                    CrossSecureStorage.Current.SetValue("loggedFieldEmail", field.Email);
                    CrossSecureStorage.Current.SetValue("loggedFieldPassword", field.Password);
                    App.Current.MainPage = new MainMasterDetailPage();
                }
                else
                {
                    await DisplayAlert("UYARI", "Yanlış e-posta yada şifre", "OK");
                }
                await Navigation.PopPopupAsync();
                    


            }
            
            
        }

        private void EntryEmail_Completed(object sender, EventArgs e)
        {

        }

        private void BtnForgotPassword_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("mailto:info@harbisaha.com"));
        }
    }
}