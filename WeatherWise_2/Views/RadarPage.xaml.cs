using System;
using GalaSoft.MvvmLight.Messaging;
using WeatherWise_2.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;

namespace WeatherWise_2.Views
{
    public sealed partial class RadarPage : Page
    {
        private RadarViewModel ViewModel
        {
            get { return ViewModelLocator.Current.RadarViewModel; }
        }

        /// <summary>
        /// E769
        /// </summary>
        public RadarPage()
        {
            InitializeComponent();
            
            ViewModel.InitializePage();
            ViewModel.RadarServiceLayerData = new Model.Radar.RadarLayerService();
            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            
            ViewModel.InitializePage();
            await ViewModel.InitializeAsync(mapControl);
            ViewModel.RadarAnimationTileSource = new MapTileSource();
            ViewModel.RadarAnimationTileSource = await ViewModel.RadarServiceLayerData.AddAnimatedRadarTiles();
            ViewModel.RadarTileFrameSource = await ViewModel.RadarServiceLayerData.AddIndividualRadarFrameTile(ViewModel.RadarAnimationTileSource.FrameCount - 1);
            mapControl.TileSources.Add(ViewModel.RadarTileFrameSource);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Cleanup();
        }

        private void RadarSiteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mapControl.TileSources.Clear();
        }

        private async void RadarFrameSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            int frame;
            Slider slider = sender as Slider;
            if (slider != null)
            {
                frame = Convert.ToInt32(slider.Value);
                play.Symbol = Symbol.Play;
                ViewModel.RadarTileFrameSource = new MapTileSource();
                ViewModel.RadarTileFrameSource = await ViewModel.RadarServiceLayerData.AddIndividualRadarFrameTile(frame - 1);
                mapControl.TileSources.Clear();
                mapControl.TileSources.Add(ViewModel.RadarTileFrameSource);
            }
        }

        private void AnimationButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (play.Symbol.Equals(Symbol.Play))
            {
                play.Symbol = Symbol.Pause;
                RadarFrameSlider.Value = 0;
                mapControl.TileSources.Add(ViewModel.RadarAnimationTileSource);
                ViewModel.RadarAnimationTileSource.Play();
                
            }
            else if (play.Symbol.Equals(Symbol.Pause))
            {
                play.Symbol = Symbol.Play;
                
                ViewModel.RadarAnimationTileSource.Pause();
            }
        }
        //&#xE768

    }
}
