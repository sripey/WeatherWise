using System;

using GalaSoft.MvvmLight.Ioc;

using WeatherWise_2.Services;
using WeatherWise_2.Views;

namespace WeatherWise_2.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;

        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        private ViewModelLocator()
        {
            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            SimpleIoc.Default.Register<ShellViewModel>();
            Register<MainViewModel, MainPage>();
            Register<Page1ViewModel, Page1Page>();
            Register<Page2ViewModel, Page2Page>();
            Register<RadarViewModel, RadarPage>();
            Register<CurrentConditionsViewModel, CurrentConditionsPage>();
        }

        public CurrentConditionsViewModel CurrentConditionsViewModel => SimpleIoc.Default.GetInstance<CurrentConditionsViewModel>();

        public RadarViewModel RadarViewModel => SimpleIoc.Default.GetInstance<RadarViewModel>();

        public Page2ViewModel Page2ViewModel => SimpleIoc.Default.GetInstance<Page2ViewModel>();

        public Page1ViewModel Page1ViewModel => SimpleIoc.Default.GetInstance<Page1ViewModel>();

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();

        public ShellViewModel ShellViewModel => SimpleIoc.Default.GetInstance<ShellViewModel>();

        public NavigationServiceEx NavigationService => SimpleIoc.Default.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
