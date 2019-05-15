using System;
using System.Collections.Generic;
using System.Text;
using WeatherWise_2.Services;
using WeatherWise_2.Core.Models;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherWise_2.Core.Models.CurrentConditions;
using Newtonsoft.Json;
using Windows.Storage;
using System.Diagnostics;
using Windows.Storage.Streams;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Services.Maps;
using Windows.Devices.Geolocation;


namespace WeatherWise_2.Model.CurrentConditions
{
    public class ApixuService
    {

        //private ApixuWeather _apixuWeatherData;
        //public ApixuWeather ApixuWeatherData
        //{
        //    get { return _apixuWeatherData; }
        //    set { _apixuWeatherData = value; }
        //}

        //public ApixuService()
        //{
        //    ApixuWeatherData = new ApixuWeather();
        //}
        public ApixuService()
        {
            Task.FromResult(PopulateCurrentConditionsData());
        }

        public  async Task<ApixuWeather> PopulateCurrentConditionsData()
        {
            MapService.ServiceToken = "Ee4W9ulYXwApQto88lkI~bQVSTATq7F6r8oZU8r_qOg~AhNCt9NzYTq15jnVNAfwPM9zRrcVGHswz4SCvwJCIcH54jCUAlTpH1coicJWYdKZ";

            var item = new ApixuWeather();

            item = await Helper.Helper.GetWeather();
            item.LastUpdated = DateTime.Parse(item.current.last_updated, new CultureInfo("en-US"));
            item.LastUpdated_ToString = string.Format("Updated  {0:MMM dd yyyy hh:mm}", item.LastUpdated);
            item.Location_ToString = string.Format("{0}, {1}", item.Town, item.State);
            item.Town = await Location.GetClosestTown();
            item.State = await Location.GetClosestState();
            item.Location_ToString = string.Format("{0}, {1}", item.Town, item.State);
            item.Windspeed_ToString = string.Format("{0}", item.current.wind_mph);
            item.WindDegree_ToString = string.Format("{0}°", item.current.wind_degree);
            item.WindDirection_ToString = item.current.wind_dir;
            item.Pressure_ToString = string.Format("{0}mb", item.current.pressure_mb);
            item.CloudCover_ToString = string.Format("{0}%", item.current.cloud);
            item.Humidity_ToString = string.Format("{0}%", Helper.Helper.ConvertToSingleDecimal(item.current.humidity));
            item.IconText = "/Assets/CurrentConditons/Icons/Night/179.png";
            item.Fah_ToString = string.Format("{0}°F", Helper.Helper.ConvertToSingleDecimal(item.current.temp_f));
            item.FeelsLike_ToString = string.Format("{0}°F", Helper.Helper.ConvertToSingleDecimal(item.current.feelslike_f));
            item.Dewpoint_ToString = string.Format("{0}°F", Helper.Helper.ConvertToSingleDecimal(item.GetDewpoint(item.current.humidity,item.current.temp_f)));
            item.Visibility_ToString = string.Format("{0} miles", Helper.Helper.ConvertToSingleDecimal(item.current.vis_miles));

            return await Task.FromResult(item);

        }




    }
}
