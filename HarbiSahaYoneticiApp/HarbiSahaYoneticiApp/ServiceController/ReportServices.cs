using HarbiSahaYoneticiApp.Models;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HarbiSahaYoneticiApp.ServiceController
{
    public class ReportServices
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

        public async Task<List<Match>> GetMonthlyMatches(int year,int month)
        {
            Field currentField = App.Current.Properties["loggedField"] as Field;
            List<FieldZone> fieldZones = new List<FieldZone>();
            fieldZones = currentField.FieldZones;        
            int fz1Id = 0, fz2Id = 0, fz3Id = 0, fz4Id = 0, fz5Id = 0, countFz;
            countFz = fieldZones.Count;
            if (countFz > 0)
                fz1Id = fieldZones[0].Id;
            if (countFz > 1)
                fz2Id = fieldZones[1].Id;
            if (countFz > 2)
                fz3Id = fieldZones[2].Id;
            if (countFz > 3)
                fz4Id = fieldZones[3].Id;
            if (countFz > 4)
                fz5Id = fieldZones[4].Id;
            int fieldId = currentField.Id;
            List<Match> matches = new List<Match>();

            if (App.Current.Properties.ContainsKey("loggedField"))
            {
                FieldZone fz = App.Current.Properties["currentFieldZone"] as FieldZone;
                var input = $"https://www.harbisaha.com/api/Field/GetMonthlyMatches?year=" + year + "&month=" + month + "&fz1Id=" +
                    fz1Id + "&fz2Id=" + fz2Id + "&fz3Id=" + fz3Id + "&fz4Id=" + fz4Id + "&fz5Id=" + fz5Id;
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                matches = JsonConvert.DeserializeObject<List<Match>>((result));
            }
            else // düzenlenecek.
            {
                return null;
            }
            return matches;
        }

        public async Task<List<Match>> GetYearlyMatches(int year)
        {
            Field currentField = App.Current.Properties["loggedField"] as Field;
            List<FieldZone> fieldZones = new List<FieldZone>();
            fieldZones = currentField.FieldZones;
            int fz1Id = 0, fz2Id = 0, fz3Id = 0, fz4Id = 0, fz5Id = 0, countFz;
            countFz = fieldZones.Count;
            if (countFz > 0)
                fz1Id = fieldZones[0].Id;
            if (countFz > 1)
                fz2Id = fieldZones[1].Id;
            if (countFz > 2)
                fz3Id = fieldZones[2].Id;
            if (countFz > 3)
                fz4Id = fieldZones[3].Id;
            if (countFz > 4)
                fz5Id = fieldZones[4].Id;
            int fieldId = currentField.Id;
            List<Match> matches = new List<Match>();

            if (App.Current.Properties.ContainsKey("loggedField"))
            {
                FieldZone fz = App.Current.Properties["currentFieldZone"] as FieldZone;
                var input = $"https://www.harbisaha.com/api/Field/GetYearlyMatches?year=" + year + "&fz1Id=" +
                    fz1Id + "&fz2Id=" + fz2Id + "&fz3Id=" + fz3Id + "&fz4Id=" + fz4Id + "&fz5Id=" + fz5Id;
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                matches = JsonConvert.DeserializeObject<List<Match>>((result));
            }
            else // düzenlenecek.
            {
                return null;
            }
            return matches;
        }

        public async Task<List<Match>> GetDailyMatches(int year,int month,int day)
        {
            Field currentField = App.Current.Properties["loggedField"] as Field;
            List<FieldZone> fieldZones = new List<FieldZone>();
            fieldZones = currentField.FieldZones;
            int fz1Id = 0, fz2Id = 0, fz3Id = 0, fz4Id = 0, fz5Id = 0, countFz;
            countFz = fieldZones.Count;
            if (countFz > 0)
                fz1Id = fieldZones[0].Id;
            if (countFz > 1)
                fz2Id = fieldZones[1].Id;
            if (countFz > 2)
                fz3Id = fieldZones[2].Id;
            if (countFz > 3)
                fz4Id = fieldZones[3].Id;
            if (countFz > 4)
                fz5Id = fieldZones[4].Id;
            int fieldId = currentField.Id;
            List<Match> matches = new List<Match>();

            if (App.Current.Properties.ContainsKey("loggedField"))
            {
                FieldZone fz = App.Current.Properties["currentFieldZone"] as FieldZone;
                var input = $"https://www.harbisaha.com/api/Field/GetDailyMatches?year=" + year + "&month=" + month + "&day=" + day + "&fz1Id=" +
                   fz1Id + "&fz2Id=" + fz2Id + "&fz3Id=" + fz3Id + "&fz4Id=" + fz4Id + "&fz5Id=" + fz5Id;
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                matches = JsonConvert.DeserializeObject<List<Match>>((result));
            }
            else // düzenlenecek.
            {
                return null;
            }
            return matches;
        }

        public async Task<List<Match>> GetLastYearMatches()
        {
            Field currentField = App.Current.Properties["loggedField"] as Field;
            List<FieldZone> fieldZones = new List<FieldZone>();
            fieldZones = currentField.FieldZones;
            int fz1Id = 0, fz2Id = 0, fz3Id = 0, fz4Id = 0, fz5Id = 0, countFz;
            countFz = fieldZones.Count;
            if (countFz > 0)
                fz1Id = fieldZones[0].Id;
            if (countFz > 1)
                fz2Id = fieldZones[1].Id;
            if (countFz > 2)
                fz3Id = fieldZones[2].Id;
            if (countFz > 3)
                fz4Id = fieldZones[3].Id;
            if (countFz > 4)
                fz5Id = fieldZones[4].Id;
            int fieldId = currentField.Id;
            List<Match> matches = new List<Match>();

            if (App.Current.Properties.ContainsKey("loggedField"))
            {
                FieldZone fz = App.Current.Properties["currentFieldZone"] as FieldZone;
                var input = $"https://www.harbisaha.com/api/Field/GetLastYearMatches?fz1Id=" + fz1Id + "&fz2Id=" + fz2Id + "&fz3Id=" + fz3Id +
                    "&fz4Id=" + fz4Id + "&fz5Id=" + fz5Id;
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                matches = JsonConvert.DeserializeObject<List<Match>>((result));
            }
            else // düzenlenecek.
            {
                return null;
            }
            return matches;
        }

        public async Task<List<Match>> GetLastWeekMathes()
        {
            Field currentField = App.Current.Properties["loggedField"] as Field;
            List<FieldZone> fieldZones = new List<FieldZone>();
            fieldZones = currentField.FieldZones;
            int fz1Id = 0, fz2Id = 0, fz3Id = 0, fz4Id = 0, fz5Id = 0, countFz;
            countFz = fieldZones.Count;
            if (countFz > 0)
                fz1Id = fieldZones[0].Id;
            if (countFz > 1)
                fz2Id = fieldZones[1].Id;
            if (countFz > 2)
                fz3Id = fieldZones[2].Id;
            if (countFz > 3)
                fz4Id = fieldZones[3].Id;
            if (countFz > 4)
                fz5Id = fieldZones[4].Id;
            int fieldId = currentField.Id;
            List<Match> matches = new List<Match>();

            if (App.Current.Properties.ContainsKey("loggedField"))
            {
                FieldZone fz = App.Current.Properties["currentFieldZone"] as FieldZone;
                var input = $"https://www.harbisaha.com/api/Field/GetLastWeekMatches?fz1Id=" + fz1Id + "&fz2Id=" + fz2Id + "&fz3Id=" + fz3Id +
                    "&fz4Id=" + fz4Id + "&fz5Id=" + fz5Id;
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                matches = JsonConvert.DeserializeObject<List<Match>>((result));
            }
            else // düzenlenecek.
            {
                return null;
            }
            return matches;
        }

        public async Task<User> GetSingleUser(int userId)
        {

            User user = new User();
            if (App.Current.Properties.ContainsKey("loggedField"))
            {
                FieldZone fz = App.Current.Properties["currentFieldZone"] as FieldZone;
                var input = $"https://www.harbisaha.com/api/Field/GetSingleUser?userId=" + userId ;
                var client = await GetClient();
                var result = await client.GetStringAsync(input);
                user = JsonConvert.DeserializeObject<User>((result));
            }
            else // düzenlenecek.
            {
                return null;
            }
            return user;
        }


    }
}
