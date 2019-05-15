using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using WeatherWise_2.Core.Models.CurrentConditions;
using WeatherWise_2.Helpers;
using WeatherWise_2.Model.CurrentConditions;
using WeatherWise_2.Services;

using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace WeatherWise_2.ViewModels
{
    /// <summary>
    /// ****GET LOCATION/ GET CURRENT WEATHER/ GET ICONS
    /// </summary>
    public class CurrentConditionsViewModel : ViewModelBase
    {
        //37.5407 -77.43365

        private ApixuService _apixuServiceData;
        public ApixuService ApixuServiceData
        {
            get { return _apixuServiceData; }
            set { Set(ref _apixuServiceData, value); }
        }

        private ApixuWeather _currentWeatherData;
        public ApixuWeather CurrentWeatherData
        {
            get { return _currentWeatherData; }
            set
            {
                Set(ref _currentWeatherData, value);
            }
        }


        public CurrentConditionsViewModel()
        {
            _apixuServiceData = new ApixuService();
    
            _ = InitializePage();
        }

        public async Task InitializePage()
        {
            try
            {
                var data = await _apixuServiceData.PopulateCurrentConditionsData();
                CurrentWeatherData = data;
            }
            catch (Exception ex)
            {
                CurrentWeatherData.Location_ToString = "Could not retrieve data";
            }

        }
    }
}
