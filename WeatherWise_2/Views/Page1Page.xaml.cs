using System;
using WeatherWise_2.Core.Models;
using WeatherWise_2.Model;
using WeatherWise_2.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WeatherWise_2.Views
{
    public sealed partial class Page1Page : Page
    {
        private Page1ViewModel ViewModel
        {
            get { return ViewModelLocator.Current.Page1ViewModel; }
        }

        public string p
        {
            get
            {
                return ViewModel.Page1ServiceData.WelcomeString;
            }
        }

        public Person n
        {
            get
            {
                return ViewModel.Page1ServiceData.MyPerson;
            }
        }

        public Page1Page()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            //Person APerson = new Person();
            ViewModel.InitializePage();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Cleanup();
        }
    }
}
