using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using WeatherWise_2.Core.Models;
using WeatherWise_2.Model.CurrentConditions;
using Windows.Storage;
using System.Diagnostics;
using WeatherWise_2.Core.Models.CurrentConditions;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using Windows.Storage.Streams;

namespace WeatherWise_2.Model.CurrentConditions.Helper
{
    public class Helper
    {
        //*********************************************
        //**** RETRIEVE WEATHER INFO VIA LAT/LONG ******
        //*********************************************
        public static async Task<ApixuWeather> GetWeather()
        {
            using (var httpClient = new HttpClient())
            {
                var currentLocation = await Location.GetPosition();
                var response = await httpClient.GetAsync(Common.APIRequestLocation(currentLocation.Latitude.ToString(), currentLocation.Longitude.ToString()));
                var resultText = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        var users = JsonConvert.DeserializeObject<ApixuWeather>(resultText);
                        users.WeatherIconPath = GetWeatherIcon(users);
                        //users.CurrentWeatherIconBitmap = new BitmapImage();
                        //users.CurrentWeatherIconBitmap = await LoadWeatherIconImage(new Uri(users.WeatherIconPath, UriKind.Absolute));
                        return users;
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine(resultText);
                        return null;
                    }
                }
                return null;
            }
        }

        //*********************************************
        //**** LOAD LOCAL DATA ******
        //*********************************************


        //......CONDITIONS......//
        public static List<CurrentCondition> LoadCurrentConditionsList()
        {
            var currentConditions = new List<CurrentCondition>();
            string fileName = string.Format("ms-appx:///Assets/Conditions.json");
            Uri appURI = new Uri(fileName);
            StorageFile jsonStorageFile = StorageFile.GetFileFromApplicationUriAsync(appURI).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            string jsonText = FileIO.ReadTextAsync(jsonStorageFile).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            currentConditions = JsonConvert.DeserializeObject<List<CurrentCondition>>(jsonText);

            return currentConditions;
        }

        //*********************************************
        //**** LOAD LOCAL DATA ******
        //**** OUTPUTS LOCATION OF WEATHER ICON
        //*********************************************
        public static string GetWeatherIcon(ApixuWeather data)
        {
            ApixuWeather retrievedCurrentData = data;
            List<CurrentCondition> localConditionsAssets = Helper.LoadCurrentConditionsList();
            int currentStatusCode = data.current.condition.code;
            string icon = "";
            string iconLocation = "";

            foreach (var item in localConditionsAssets)
            {
                if (item.code == currentStatusCode)
                {
                    icon = item.icon.ToString();
                    break;
                }
            }

            if (retrievedCurrentData.current.is_day == 1)
            {
                iconLocation = Common.WeatherConditionIconFolder_Day;
            }
            else
            {
                iconLocation = Common.WeatherConditionIconFolder_Night;
            }

            iconLocation = string.Format("{0}{1}.png", iconLocation, icon);
            return iconLocation;
        }


        public static string ConvertToSingleDecimal(double value)
        {
            string result = "";
            result = string.Format("{0:0.0}", Math.Truncate(value * 10) / 10);
            return result;
        }

        public static string CleanURILocation(string uri)
        {
            var cleanUri = uri.Substring(2);
            return cleanUri;
        }

    }
}
