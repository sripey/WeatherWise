using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WeatherWise_2.Core.Models.Radar
{
    public class LocalRadarSite :INotifyPropertyChanged
    {

        //private Site_Information _radarSiteInformation;

        //public Site_Information RadarSiteInformation

        //{
        //    get { return _radarSiteInformation; }
        //    set
        //    {
        //        if (_radarSiteInformation != value)
        //    }
        //}


        private string _localName;
        public string LocalName
        {
            get { return _localName; }
            set
            {
                if (_localName != value)
                {
                    _localName = value;
                    RaisePropertyChanged("LocalName");
                }
            }
        }

        private string _localAbbrev;
        public string LocalAbbrev
        {
            get { return _localAbbrev; }
            set
            {
                if(_localAbbrev != value)
                {
                    _localAbbrev = value;
                    RaisePropertyChanged("LocalAbbrev");
                }
            }
        }

        private ObservableCollection<RadarProduct> _radarProducts;
        public ObservableCollection<RadarProduct> RadarProducts
        {
            get { return _radarProducts; }
            set
            {
                if(_radarProducts != value)
                {
                    _radarProducts = value;
                    RaisePropertyChanged("RadarProducts");
                }
            }
        }

        private Geopoint _coordinates;

        public event PropertyChangedEventHandler PropertyChanged;

        public Geopoint Coordinates

        {
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        public LocalRadarSite()
        {
            RadarProducts = new ObservableCollection<RadarProduct>();
            
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
