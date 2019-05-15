using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WeatherWise_2.ViewModels;
using WeatherWise_2.Core.Models.CurrentConditions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherWise_2.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CurrentConditionsPage : Page
    {
        private CurrentConditionsViewModel ViewModel
        {
            get { return ViewModelLocator.Current.CurrentConditionsViewModel; }
        }

        public ApixuWeather WeatherData
        {
            get;set;
        }



        public CurrentConditionsPage()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {

            };
            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //WeatherData = new ApixuWeather();
            //ViewModel.ApixuServiceData.ApixuWeatherData = await ViewModel.ApixuServiceData.PopulateCurrentConditionsData();
            // await ViewModel.ApixuServiceData.PopulateCurrentConditionsData();
            //ViewModel.ApixuServiceData.ApixuWeatherData.Fah_ToString = "richmond";
            await ViewModel.InitializePage();
            
           


        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Cleanup();
        }



        public async void PopulateData(object sender,RoutedEventArgs e)
        {
            await ViewModel.InitializePage();
        }

       

        private void ContentArea_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateData(sender, e);
        }
    }
}
