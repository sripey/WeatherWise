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
using System.IO;
using System.Net;
using Windows.Storage.Streams;

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
                httpMapTileDataSource.AllowCaching = true;
                httpMapTileDataSource.UriRequested += HttpMapTileDataSourceUriRequested;

                _radarImageLoop.MapHttpTileSource = new MapTileSource
                {
                    DataSource = httpMapTileDataSource,
                    IsFadingEnabled = true,
                    
                    FrameCount = _radarImageLoop.TimeStamps.Length,
                    FrameDuration = TimeSpan.FromMilliseconds(300),
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

    public class TileDataSourceWithOpacity : CustomMapTileDataSource, INotifyPropertyChanged
    {
        private string _tileUrl;
        private byte _opacity;
        private RadarImage _radarImageFrame;

        private RadarImage _radarImageFrameWithOpacity;
        public RadarImage RadarImageFrameWithOpacity
        {
            get { return _radarImageFrameWithOpacity; }
            set
            {
                if(_radarImageFrameWithOpacity != value)
                {
                    _radarImageFrameWithOpacity = value;
                    RaisePropertyChanged("RadarImageFrameWithOpacity");
                }
            }
        }


        public TileDataSourceWithOpacity()
        {
            _radarImageFrameWithOpacity = new RadarImage();
        }

        public TileDataSourceWithOpacity(string tileUrl, byte opacity)
        {
            _tileUrl = tileUrl;
            _opacity = opacity;
            _radarImageFrameWithOpacity = new RadarImage();
            this.BitmapRequested += TileDataSourceWithOpacity_BitmapRequested;
        }

        public async Task<MapTileSource> AddIndividualRadarFrameTileWithOpacity(int frame)
        {

            _radarImageFrameWithOpacity = new RadarImage();
            var customMapTileDataSourceWithOpacity = new TileDataSourceWithOpacity(_radarImageFrame.UrlTemplate.Replace("{timestamp}", _radarImageFrame.TimeStamps[frame]),5);
            _radarImageFrameWithOpacity.MapHttpTileSource = new MapTileSource(customMapTileDataSourceWithOpacity);
            return await Task.FromResult(_radarImageFrameWithOpacity.MapHttpTileSource);
        }

        private async void TileDataSourceWithOpacity_BitmapRequested(CustomMapTileDataSource sender, MapTileBitmapRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();

            using (var imgStream = await GetTileAsStreamAsync(args.X, args.Y, args.ZoomLevel))
            {
                var memStream = imgStream.AsRandomAccessStream();
                var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(memStream);

                var pixelProvideer = await decoder.GetPixelDataAsync(
                    Windows.Graphics.Imaging.BitmapPixelFormat.Rgba8,
                    Windows.Graphics.Imaging.BitmapAlphaMode.Straight,
                    new Windows.Graphics.Imaging.BitmapTransform(),
                    Windows.Graphics.Imaging.ExifOrientationMode.RespectExifOrientation,
                    Windows.Graphics.Imaging.ColorManagementMode.ColorManageToSRgb);

                var pixels = pixelProvideer.DetachPixelData();

                var width = decoder.OrientedPixelWidth;
                var height = decoder.OrientedPixelHeight;

                Parallel.For(0, height, (i) =>
                {
                    for (var j = 0; j < width; j++)
                    {
                        var idx = (i * height + j) * 4 + 3; // Alpha channel Index (RGBA)

                        //Only change the opacity of a pixel if it isn't transparent
                        if (pixels[idx] != 0)
                        {
                            pixels[idx] = _opacity;
                        }
                    }
                });

                var randomAccessStream = new InMemoryRandomAccessStream();
                var outputStream = randomAccessStream.GetOutputStreamAt(0);
                var writer = new DataWriter(outputStream);
                writer.WriteBytes(pixels);
                await writer.StoreAsync();
                await writer.FlushAsync();

                args.Request.PixelData = RandomAccessStreamReference.CreateFromStream(randomAccessStream);
            }
            deferral.Complete();
        }

        private Task<MemoryStream> GetTileAsStreamAsync(int x, int y, int zoom)
        {
            var tcs = new TaskCompletionSource<MemoryStream>();

            var quadkey = TileXYZoomToQuadKey(x, y, zoom);
            var url = _tileUrl.Replace("{x}", x.ToString()).Replace("{y}", y.ToString()).Replace("{zoomlevel}", zoom.ToString()).Replace("{quadkey}", quadkey);
            var request = HttpWebRequest.Create(url);
            request.BeginGetResponse(async (a) =>
            {
                var r = (HttpWebRequest)a.AsyncState;
                HttpWebResponse response = (HttpWebResponse)r.EndGetResponse(a);

                using (var s = response.GetResponseStream())
                {
                    var ms = new MemoryStream();
                    await s.CopyToAsync(ms);
                    ms.Position = 0;
                    tcs.SetResult(ms);
                }
            }, request);
            return tcs.Task;
        }

        private string TileXYZoomToQuadKey(int tileX, int tileY, int zoom)
        {
            var quadKey = new StringBuilder();
            for (int i = zoom; i > 0; i--) 
            {
                char digit = '0';
                int mask = 1 << (i - 1);
                if ((tileX & mask) != 0)
                {
                    digit++;
                }

                if((tileY & mask) != 0)
                {
                    digit++;
                }
                quadKey.Append(digit);
            }
            return quadKey.ToString();
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
