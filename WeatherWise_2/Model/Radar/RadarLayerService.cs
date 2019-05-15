using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWise_2.Core.Models;
using Windows.UI.Xaml.Controls.Maps;
using WeatherWise_2.Core.Models.Radar;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using System.Net.Http;

namespace WeatherWise_2.Model.Radar
{
    public class RadarLayerService : INotifyPropertyChanged
    {
        
        public List<RadarImage> RadarImageList = new List<RadarImage>();
        public RadarImage tempRadarImage;
        public Image tempImage = new Image();
        public static string urlTemplate = "http://mesonet.agron.iastate.edu/cache/tile.py/1.0.0/nexrad-n0q-{timestamp}/{zoomlevel}/{x}/{y}.png";

        private RadarImage _radarImageLoop;
        public RadarImage RadarImageLoop
        {
            get { return _radarImageLoop; }
            set { _radarImageLoop = value; }
        }

        private RadarImage _radarImageFrame;
        public RadarImage RadarImageFrame
        {
            get { return _radarImageFrame; }
            set
            {
                if (_radarImageFrame != value)
                {
                    _radarImageFrame = value;
                    RaisePropertyChanged("RadarImageFrame");
                }
            }
        }

        public RadarLayerService()
        {
            RadarImageLoop = new RadarImage();
            RadarImageFrame = new RadarImage();
        }

        public async Task<MapTileSource> AddAnimatedRadarTiles()
        {
            if (_radarImageLoop.MapHttpTileSource == null)
            {

                var httpMapTileDataSource = new HttpMapTileDataSource();
                //var httpMapTileDataSource = new HttpMapTileDataSource();
                //httpMapTileDataSource.AllowCaching = true;
                httpMapTileDataSource.UriRequested += HttpMapTileDataSourceUriRequested;

                _radarImageLoop.MapHttpTileSource = new MapTileSource
                {
                    DataSource = httpMapTileDataSource,
                    
                    FrameCount = _radarImageLoop.TimeStamps.Length,
                    FrameDuration = TimeSpan.FromMilliseconds(500),
                };

                return await Task.FromResult(_radarImageLoop.MapHttpTileSource);
            }
            return await Task.FromResult(_radarImageLoop.MapHttpTileSource);
        }

        public async Task<MapTileSource> AddIndividualRadarFrameTile(int frame)
        {
            var httpMapTileDataSource = new HttpMapTileDataSource(RadarImageFrame.UrlTemplate.Replace("{timestamp}",RadarImageFrame.TimeStamps[frame]));
           _radarImageFrame.MapHttpTileSource = new MapTileSource(httpMapTileDataSource);
            return await Task.FromResult(_radarImageFrame.MapHttpTileSource);
        }

        public async Task<HttpMapTileDataSource> GetMapTileSource()
        {
            var tempHttpMapTileSource = new HttpMapTileDataSource();
            tempHttpMapTileSource.AllowCaching = true;
            tempHttpMapTileSource.UriRequested += HttpMapTileDataSourceUriRequested;

            return tempHttpMapTileSource;
        }

        private void HttpMapTileDataSourceUriRequested(HttpMapTileDataSource sender, MapTileUriRequestedEventArgs args)
        {
            args.Request.Uri = new Uri(_radarImageLoop.UrlTemplate.Replace("{timestamp}", _radarImageLoop.TimeStamps[args.FrameIndex]));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
