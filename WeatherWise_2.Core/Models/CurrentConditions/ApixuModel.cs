using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WeatherWise_2.Core.Models.CurrentConditions
{
    public class Location
    {
        public string name { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string tz_id { get; set; }
        public int localtime_epoch { get; set; }
        public string localtime { get; set; }
    }

    public class Condition
    {
        public string text { get; set; }
        public string icon { get; set; }
        public int code { get; set; }
    }

    public class Current
    {
        public int last_updated_epoch { get; set; }
        public string last_updated { get; set; }
        public double temp_c { get; set; }
        public double temp_f { get; set; }
        public int is_day { get; set; }
        public Condition condition { get; set; }
        public double wind_mph { get; set; }
        public double wind_kph { get; set; }
        public int wind_degree { get; set; }
        public string wind_dir { get; set; }
        public double pressure_mb { get; set; }
        public double pressure_in { get; set; }
        public double precip_mm { get; set; }
        public double precip_in { get; set; }
        public int humidity { get; set; }
        public int cloud { get; set; }
        public double feelslike_c { get; set; }
        public double feelslike_f { get; set; }
        public double vis_km { get; set; }
        public double vis_miles { get; set; }
        public double uv { get; set; }
    }

    public class ApixuWeather
    {
        public Location location { get; set; }
        public Current current { get; set; }
        public string WeatherIconPath { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string LastUpdated_ToString { get; set; }

        private string _location_ToString;
        public string Location_ToString
        {
            get { return _location_ToString; }
            set { _location_ToString = $"{Town}, {State}"; }
        }
        public string IconText { get; set; }
        public DateTime LastUpdated { get; set; }

        public string Dewpoint_ToString { get; set; }
        private string _fah_ToString;
        public string Fah_ToString
        {
            get { return _fah_ToString; }
            set { _fah_ToString = value; }
        }
        public string FeelsLike_ToString { get; set; }
        public string Humidity_ToString { get; set; }
        public string Windspeed_ToString { get; set; }
        public string WindDirection_ToString { get; set; }
        public string WindDegree_ToString { get; set; }
        public string Pressure_ToString { get; set; }
        public string CloudCover_ToString { get; set; }
        public string Visibility_ToString { get; set; }




        public double GetDewpoint(double humidity, double temp)
        {

            double hum = humidity;
            double tmp = temp;

            double VapourPressure = hum * 0.01 * 6.112 * Math.Exp((17.62 * tmp) / (temp + 243.12));
            double Numerator = 243.13 * Math.Log(VapourPressure) - 440.1;
            double Denominator = 19.43 - (Math.Log(VapourPressure));
            double dewpoint = Numerator / Denominator;

            return dewpoint;
        }
    }
}
