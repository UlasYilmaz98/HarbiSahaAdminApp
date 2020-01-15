using HarbiSahaYoneticiApp.Models;
using HarbiSahaYoneticiApp.Models.OtherModels;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HarbiSahaYoneticiApp.ServiceController
{
    public class ManageServices
    {
        public async Task<HttpClient> GetClient()
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            if (/*Application.Current.Properties.Keys.Contains("token")*/ CrossSecureStorage.Current.HasKey("token"))
            {
                //var token = Application.Current.Properties["token"];
                string token = CrossSecureStorage.Current.GetValue("token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
            }
            return client;
        }

        public async Task<HttpClient> GetFirstClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<Field> Login(string email, string password)
        {

            try
            {
              
                LoginFieldModel loginModel = new LoginFieldModel();
                CrossSecureStorage.Current.DeleteKey("token");
                var input = $"https://www.harbisaha.com/api/Logon/LoginField?email=" + email + "&password=" + password;
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                loginModel = JsonConvert.DeserializeObject<LoginFieldModel>((result));
                //string tok = loginModel.token.ToString().Substring(1, 556);
                string tok = loginModel.token.ToString();
                CrossSecureStorage.Current.SetValue("token", tok);
                Field loggedField = loginModel.currentField;
                
                if (loginModel.currentField != null)
                {
                    Application.Current.Properties["loggedField"] = loggedField;
                    Application.Current.Properties["currentFieldZone"] = loggedField.FieldZones[0];
                  
                }
                if( loginModel.NotesAndSubscribes != null)
                {
                    List<Announcement> notes = new List<Announcement>();
                    List<Announcement> subs = new List<Announcement>();
                    foreach( Announcement ann in loginModel.NotesAndSubscribes )
                    {
                        if (ann.Type == "Note")
                            notes.Add(ann);
                        else if (ann.Type == "Subscribe")
                            subs.Add(ann);
                    }
                    App.Current.Properties["Notes"] = notes;
                    App.Current.Properties["Subscribes"] = subs;
                }
             
                CrossSecureStorage.Current.SetValue("currentFieldEmail", loggedField.Email);
                await Application.Current.SavePropertiesAsync();
                return loggedField;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Match>> GetMatches(int fieldZoneId)
        {
            List<Match> matches = new List<Match>();
            Field currentField = new Field();
            int userId;
            string mainCity;

            if (App.Current.Properties.ContainsKey("loggedField"))
            {
                currentField = App.Current.Properties["loggedField"] as Field;             
                var input = $"https://www.harbisaha.com/api/Field/GetMatches?fieldZoneId=" + fieldZoneId;
                //var client = await GetClient();
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                matches = JsonConvert.DeserializeObject<List<Match>>((result));
            }
            else // düzenlenecek.
            {
                var input = $"https://www.harbisaha.com/api/Field/GetMatches?fieldZoneId=" + fieldZoneId;
                //var client = await GetClient();
                var client = await GetFirstClient();
                var result = await client.GetStringAsync(input);
                matches = JsonConvert.DeserializeObject<List<Match>>((result));
                //return null;

            }
            try
            {
                return matches;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<Match> SetCloseToOpen(int year,int month,int day,int time)
        {
            FieldZone currentFz = Application.Current.Properties["currentFieldZone"] as FieldZone;
            int fieldZoneId = currentFz.Id;
            Match match = new Match();

            if (App.Current.Properties.ContainsKey("loggedField"))
            {
                FieldZone fz = App.Current.Properties["currentFieldZone"] as FieldZone;
                var input = $"https://www.harbisaha.com/api/Field/TimeMakeAvailable?year=" + year + "&month=" + month + "&day=" + day + "&time=" + time + "&fieldZoneId=" + fieldZoneId + "&halfPrice=" + fz.PaymentModel1SmallCost + "&fullPrice=" + fz.PaymentModel1FullCost;
                //var client = await GetClient();
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                match = JsonConvert.DeserializeObject<Match>((result));
            }
            else // düzenlenecek.
            {
                var input = $"https://www.harbisaha.com/api/Field/TimeMakeAvailable?year=" + year + "&month=" + month + "&day=" + day + "&time=" + time + "&fieldZoneId=" + fieldZoneId;
                //var client = await GetClient();
                var client = await GetFirstClient();
                var result = await client.GetStringAsync(input);
                match = JsonConvert.DeserializeObject<Match>((result));
            }
            try
            {
                return match;

            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<string> SetOpenToClose(int year, int month, int day, int time)
        {
            FieldZone currentFz = Application.Current.Properties["currentFieldZone"] as FieldZone;
            int fieldZoneId = currentFz.Id;
            string response = "Hata";

            if (App.Current.Properties.ContainsKey("loggedField"))
            {
                var input = $"https://www.harbisaha.com/api/Field/TimeMakeUnAvailable?year=" + year + "&month=" + month + "&day=" + day + "&time=" + time + "&fieldZoneId=" + fieldZoneId;
                //var client = await GetClient();
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                response = JsonConvert.DeserializeObject<string>((result));
            }
            else // düzenlenecek.
            {
                var input = $"https://www.harbisaha.com/api/Field/TimeMakeUnAvailable?year=" + year + "&month=" + month + "&day=" + day + "&time=" + time + "&fieldZoneId=" + fieldZoneId;
                //var client = await GetClient();
                var client = await GetFirstClient();
                var result = await client.GetStringAsync(input);
                response = JsonConvert.DeserializeObject<string>((result));
            }
            try
            {
                return response;

            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<Announcement> AddNote(int fieldZoneId, int day, int month, int year, int time, int isExist, string text)
        {
            string response = "Hata";
            Announcement note = new Announcement();
            if (App.Current.Properties.ContainsKey("loggedField"))
            {
                var input = $"https://www.harbisaha.com/api/Field/AddNote?fieldZoneId=" + fieldZoneId + "&day=" + day + "&month=" +month+ "&year=" +year+ "&time=" +time+ "&isExist=" +isExist+ "&text=" +text;
                //var client = await GetClient();
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                note = JsonConvert.DeserializeObject<Announcement>((result));
            }
            else // düzenlenecek.
            {
                var input = $"https://www.harbisaha.com/api/Field/AddNote?fieldZoneId=" + fieldZoneId + "&day=" + day + "&month=" + month + "&year=" + year + "&time=" + time + "&isExist=" + isExist + "&text=" + text;
                //var client = await GetClient();
                var client = await GetFirstClient();
                var result = await client.GetStringAsync(input);
                note = JsonConvert.DeserializeObject<Announcement>((result));
            }
            try
            {
                if (note != null)
                    return note;
                else
                    return null;

            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<List<Match>> GetCurrentMatches(int fieldZoneId)
        {
            //string response = "Hata";
            Announcement note = new Announcement();
            List<Match> MatchList = new List<Match>();
            if (App.Current.Properties.ContainsKey("loggedField"))
            {
                var input = $"https://www.harbisaha.com/api/Field/GetCurrentMatches?fieldZoneId=" + fieldZoneId;
                //var client = await GetClient();
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                MatchList = JsonConvert.DeserializeObject<List<Match>>((result));
            }
            else // düzenlenecek.
            {
                var input = $"https://www.harbisaha.com/api/Field/GetCurrentMatches?fieldZoneId=" + fieldZoneId;
                //var client = await GetClient();
                var client = await GetFirstClient();
                var result = await client.GetStringAsync(input);
                MatchList = JsonConvert.DeserializeObject<List<Match>>((result));
            }
            try
            {

                return MatchList;

            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<Announcement> CreateNewSubscribe(int fieldZoneId, int dayOfWeek, int time)
        {
            Announcement sub = new Announcement();
            if (App.Current.Properties.ContainsKey("loggedField"))
            {
                var input = $"https://www.harbisaha.com/api/Field/CreateNewSubscribe?fieldZoneId=" + fieldZoneId + "&dayOfWeek=" + dayOfWeek + "&time=" + time;            
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                sub = JsonConvert.DeserializeObject<Announcement>((result));
            }
            else // düzenlenecek.
            {
                var input = $"https://www.harbisaha.com/api/Field/CreateNewSubscribe?fieldZoneId=" + fieldZoneId + "&dayOfWeek=" + dayOfWeek + "&time=" + time;
                //var client = await GetClient();
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                sub = JsonConvert.DeserializeObject<Announcement>((result));
            }
            try
            {

                return sub;

            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<List<Purchase>> getPurchases ()
        {
            List<Purchase> purchases = new List<Purchase>();
            Field field = App.Current.Properties["loggedField"] as Field;
            try
            {
                if (App.Current.Properties.ContainsKey("loggedField"))
                {
                    var input = $"https://www.harbisaha.com/api/Field/getPurchases?fieldId=" + field.Id;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    purchases = JsonConvert.DeserializeObject<List<Purchase>>((result));
                }
                else // düzenlenecek.
                {
                    var input = $"https://www.harbisaha.com/api/Field/getPurchases?fieldId=" + field.Id;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    purchases = JsonConvert.DeserializeObject<List<Purchase>>((result));
                }
                return purchases;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public async Task<Match> CancelDiscount( int day, int month, int year, int time)
        {
            Match responseMatch = new Match();
            //Field field = App.Current.Properties["loggedField"] as Field;
            FieldZone fieldZone = App.Current.Properties["currentFieldZone"] as FieldZone;
            int normalCost = fieldZone.PaymentModel1FullCost;
            try
            {
                if (App.Current.Properties.ContainsKey("loggedField"))
                {
                    var input = $"https://www.harbisaha.com/api/Field/CancelDiscount?fieldZoneId=" + fieldZone.Id + "&day=" + day + "&month=" + month + "&year=" + year + "&time=" + time + "&normalCost=" + normalCost; ;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    responseMatch = JsonConvert.DeserializeObject<Match>((result));
                }
                else // düzenlenecek.
                {
                    var input = $"https://www.harbisaha.com/api/Field/CancelDiscount?fieldZoneId=" + fieldZone.Id + "&day=" + day + "&month=" + month + "&year=" + year + "&time=" + time + "&normalCost=" + normalCost; ;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    responseMatch = JsonConvert.DeserializeObject<Match>((result));
                }
                return responseMatch;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<Match> SetRentedFromClose( int day, int month, int year, int time, int halfPrice, int fullPrice)
        {
            Match responseMatch = new Match();
            FieldZone fieldZone = App.Current.Properties["currentFieldZone"] as FieldZone;
            try
            {
                if (App.Current.Properties.ContainsKey("loggedField"))
                {
                    var input = $"https://www.harbisaha.com/api/Field/SetRentedFromClose?fieldZoneId=" + fieldZone.Id + "&day=" + day + "&month=" + month + "&year=" + year + "&time=" + time + "&halfPrice=" + halfPrice + "&fullPrice="+ fullPrice ;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    responseMatch = JsonConvert.DeserializeObject<Match>((result));
                }
                else // düzenlenecek.
                {
                    var input = $"https://www.harbisaha.com/api/Field/SetRentedFromClose?fieldZoneId=" + fieldZone.Id + "&day=" + day + "&month=" + month + "&year=" + year + "&time=" + time + "&halfPrice=" + halfPrice + "&fullPrice=" + fullPrice;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    responseMatch = JsonConvert.DeserializeObject<Match>((result));
                }
                return responseMatch;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<Match> MakeDiscount(int day, int month, int year, int time, int discountPercentage)
        {
            Match responseMatch = new Match();
            FieldZone fieldZone = App.Current.Properties["currentFieldZone"] as FieldZone;
            try
            {
                if (App.Current.Properties.ContainsKey("loggedField"))
                {
                    var input = $"https://www.harbisaha.com/api/Field/MakeDiscount?fieldZoneId=" + fieldZone.Id + "&day=" + day + "&month=" + month + "&year=" + year + "&time=" + time + "&discountPercentage=" + discountPercentage;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    responseMatch = JsonConvert.DeserializeObject<Match>((result));
                }
                else // düzenlenecek.
                {
                    var input = $"https://www.harbisaha.com/api/Field/MakeDiscount?fieldZoneId=" + fieldZone.Id + "&day=" + day + "&month=" + month + "&year=" + year + "&time=" + time + "&discountPercentage=" + discountPercentage;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    responseMatch = JsonConvert.DeserializeObject<Match>((result));
                }
                return responseMatch;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<Match> FixCost(int day, int month, int year, int time, int newHalfPrice, int newFullPrice)
        {

            Match responseMatch = new Match();
            FieldZone fieldZone = App.Current.Properties["currentFieldZone"] as FieldZone;
            try
            {
                if (App.Current.Properties.ContainsKey("loggedField"))
                {
                    var input = $"https://www.harbisaha.com/api/Field/FixCost?fieldZoneId=" + fieldZone.Id + "&day=" + day + "&month=" + month + "&year=" + year + "&time=" + time + "&newHalfPrice=" + newHalfPrice + "&newFullPrice=" + newFullPrice;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    responseMatch = JsonConvert.DeserializeObject<Match>((result));
                }
                else // düzenlenecek.
                {
                    var input = $"https://www.harbisaha.com/api/Field/FixCost?fieldZoneId=" + fieldZone.Id + "&day=" + day + "&month=" + month + "&year=" + year + "&time=" + time + "&newHalfPrice=" + newHalfPrice + "&newFullPrice=" + newFullPrice;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    responseMatch = JsonConvert.DeserializeObject<Match>((result));
                }
                return responseMatch;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<Match> SetCloseFromRented( int day, int month, int year, int time)
        {

            Match responseMatch = new Match();
            FieldZone fieldZone = App.Current.Properties["currentFieldZone"] as FieldZone;
            try
            {
                if (App.Current.Properties.ContainsKey("loggedField"))
                {
                    var input = $"https://www.harbisaha.com/api/Field/SetCloseFromRented?fieldZoneId=" + fieldZone.Id + "&day=" + day + "&month=" + month + "&year=" + year + "&time=" + time;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    responseMatch = JsonConvert.DeserializeObject<Match>((result));
                }
                else // düzenlenecek.
                {
                    var input = $"https://www.harbisaha.com/api/Field/SetCloseFromRented?fieldZoneId=" + fieldZone.Id + "&day=" + day + "&month=" + month + "&year=" + year + "&time=" + time;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    responseMatch = JsonConvert.DeserializeObject<Match>((result));
                }
                return responseMatch;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<Match> SetOpenFromRented( int day, int month, int year, int time)
        {
            
            FieldZone fieldZone = App.Current.Properties["currentFieldZone"] as FieldZone;
            int halfPrice = fieldZone.PaymentModel1SmallCost;
            int fullPrice = fieldZone.PaymentModel1FullCost;
            //int halfPrice = f
            try
            {
                Match responseMatch = new Match();
                if (App.Current.Properties.ContainsKey("loggedField"))
                {
                    var input = $"https://www.harbisaha.com/api/Field/SetOpenFromRented?fieldZoneId=" + fieldZone.Id + "&day=" + day + "&month=" + month + "&year=" + year + "&time=" + time + "&halfPrice=" + halfPrice + "&fullPrice=" + fullPrice;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    responseMatch = JsonConvert.DeserializeObject<Match>((result));
                }
                else // düzenlenecek.
                {
                    var input = $"https://www.harbisaha.com/api/Field/SetOpenFromRented?fieldZoneId=" + fieldZone.Id + "&day=" + day + "&month=" + month + "&year=" + year + "&time=" + time + "&halfPrice=" + halfPrice + "&fullPrice=" + fullPrice;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    responseMatch = JsonConvert.DeserializeObject<Match>((result));
                }
                return responseMatch;
            }
            catch (Exception ex)
            {

                string error = ex.Message;
                return null;
            }
        }

        public async Task<string> CancelSubscribe(int dayOfWeek,int time)
        {
            FieldZone fieldZone = App.Current.Properties["currentFieldZone"] as FieldZone;
            
            //int halfPrice = f
            try
            {
                //Match responseMatch = new Match();
                string response;
                if (App.Current.Properties.ContainsKey("loggedField"))
                {
                    var input = $"https://www.harbisaha.com/api/Field/CancelSubscribe?fieldZoneId=" + fieldZone.Id + "&dayOfWeek=" + dayOfWeek + "&time=" + time ;
                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    response = JsonConvert.DeserializeObject<string>((result));
                }
                else // düzenlenecek.
                {
                    var input = $"https://www.harbisaha.com/api/Field/CancelSubscribe?fieldZoneId=" + fieldZone.Id + "&dayOfWeek=" + dayOfWeek + "&time=" + time;                    //var client = await GetClient();
                    var client = await GetClient();
                    var result = await client.GetStringAsync(input);
                    response = JsonConvert.DeserializeObject<string>((result));
                }
                return response;
            }
            catch (Exception ex)
            {

                string error = ex.Message;
                return "Başarısız";
            }
        }




    }
}
