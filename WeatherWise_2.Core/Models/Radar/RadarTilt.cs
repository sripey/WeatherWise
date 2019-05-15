using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media.Imaging;

namespace WeatherWise_2.Core.Models.Radar
{
    public class RadarTilt : INotifyPropertyChanged
    {

        private string _tiltName;
        public string TiltName
        {
            get { return _tiltName; }
            set
            {
                if(_tiltName != value)
                {
                    _tiltName = value;
                    RaisePropertyChanged("TiltName");
                }
            }
        }

        private ObservableCollection<RadarImage> _radarImageList;
        public ObservableCollection<RadarImage> RadarImageList
        {
            get { return _radarImageList; }
            set
            {
                if(_radarImageList != value)
                {
                    _radarImageList = value;
                    RaisePropertyChanged("RadarImageList");
                }
            }
        }

        public RadarTilt()
        {
            RadarImageList = new ObservableCollection<RadarImage>();
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

    public class RadarImage
    {
        private string _fileLocation;
        public string FileLocation
        {
            get { return _fileLocation; }
            set { _fileLocation = value; }
        }

        private Image _imageFile;
        public Image ImageFile
        {
            get { return _imageFile; }
            set { _imageFile = value; }
        }

        private string _urlTemplate;
        public string UrlTemplate
        {
            get { return _urlTemplate; }
            set { _urlTemplate = value; }
        }

        private string[] _timestamps;
        public string[] TimeStamps
        {
            get { return _timestamps; }
            set
            {
                if (_timestamps != value)
                {
                    _timestamps = value;
                    RaisePropertyChanged("TimeStamps");
                    
                }
            }
        }

        private string _timeStamp;

        public string TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }


        private MapTileSource _maphttpTileSource;
        public MapTileSource MapHttpTileSource
        {
            get { return _maphttpTileSource; }
            set
            {
                if(_maphttpTileSource != value)
                {
                    _maphttpTileSource = value;
                    RaisePropertyChanged("MapHttpTileSource");
                }
            }
        }

        public RadarImage(string urlTemplate, string[] timeStamps)
        {
            this._urlTemplate = urlTemplate;
            this._timestamps = timeStamps;
            MapHttpTileSource = null;
        }

        public RadarImage(string urlTemplate, string currentTimeStamps, int index)
        {
            this._urlTemplate = urlTemplate;
            this._timestamps[index] = currentTimeStamps;
            MapHttpTileSource = null;
        }

        public RadarImage()
        {
            this._urlTemplate = urlTemplate;
            this._timestamps = timeStamps;
            MapHttpTileSource = null;
        }

        public static string urlTemplate = "http://mesonet.agron.iastate.edu/cache/tile.py/1.0.0/nexrad-n0q-{timestamp}/{zoomlevel}/{x}/{y}.png";
        public static string[] timeStamps = { "900913-m50m", "900913-m45m", "900913-m40m", "900913-m35m", "900913-m30m", "900913-m25m", "900913-m20m", "900913-m15m", "900913-m10m", "900913-m05m", "900913" };

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
