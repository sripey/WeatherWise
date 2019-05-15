using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace WeatherWise_2.Model.CurrentConditions
{
    public class Common
    {
        //******APIXU API KEY*****
        public static string API_Key = "key=b19a7a8dcb284ef1abb140442191901";


        //*****URL PATHS*****
        public static string API_Link_Current = "http://api.apixu.com/v1/current.json?";
        public static string API_Link_Forecast = "http://api.apixu.com/v1/forcast.json?";

        //*****LOCAL FILE PATHS******
        public static string RootFileLocation = "ms-appx:///Assets";
        public static string Conditions_File = "/Conditions.json";
        public static string ConditionsFolder = string.Format("{0}/CurrentConditions", RootFileLocation);
        public static string WeatherConditionIconsFolder = string.Format("{0}/Icons", ConditionsFolder);
        //public static string WeatherConditionIconFolder_Day = string.Format("{0}/Day", WeatherConditionIconsFolder);
        public static string WeatherConditionIconFolder_Day = "/Assets/CurrentConditons/Icons/Day/";

        public static string WeatherConditionIconFolder_Night = "/Assets/CurrentConditons/Icons/Night/";

        //*****WEB REQUESTS & FILE RETRIEVAL METHODS*****
        public static string APIRequestLocation(string lat, string lon)
        {
            StringBuilder stringBuilder = new StringBuilder(API_Link_Current);
            stringBuilder.AppendFormat("{0}&q={1},{2}", API_Key, lat, lon);
            return stringBuilder.ToString();
        }

        public static BasicGeoposition GetBasicGeoposition()
        {
            var basicGeoposition = new BasicGeoposition();
            return basicGeoposition;
        }

        public static string LocalFileRequest()
        {
            StringBuilder strBuilder = new StringBuilder(RootFileLocation);
            strBuilder.AppendFormat("{0}", Conditions_File);
            return strBuilder.ToString();
        }



    }
}
