using System;

using WeatherWise_2.ViewModels;

using Windows.UI.Xaml.Controls;

namespace WeatherWise_2.Views
{
    public sealed partial class Page2Page : Page
    {
        private Page2ViewModel ViewModel
        {
            get { return ViewModelLocator.Current.Page2ViewModel; }
        }

        public Page2Page()
        {
            InitializeComponent();
        }
    }
}
