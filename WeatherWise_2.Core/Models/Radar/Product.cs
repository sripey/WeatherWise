using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WeatherWise_2.Core.Models.Radar
{
    public class RadarProduct : INotifyPropertyChanged
    {
        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set
            {
                if(_productName != value)
                {
                    _productName = value;
                    RaisePropertyChanged("ProductName");
                }
            }
        }

        private string _productAbbrev;
        public string ProductAbbrev
        {
            get { return _productAbbrev; }
            set
            {
                if (_productAbbrev != value)
                {
                    _productAbbrev = value;
                    RaisePropertyChanged("ProductAbbrev");
                }
            }
        }



        private ObservableCollection<RadarTilt> _radarTilts;
        public ObservableCollection<RadarTilt> RadarTilts
        {
            get { return _radarTilts; }
            set
            {
                if (_radarTilts != value)
                {
                    _radarTilts = value;
                    RaisePropertyChanged("RadarTilts");
                }
            }
        }


        public RadarProduct()
        {
            RadarTilts = new ObservableCollection<RadarTilt>();
            RadarTilts.Add(new RadarTilt { TiltName = "0.5" });
            RadarTilts.Add(new RadarTilt { TiltName = "0.9" });
            RadarTilts.Add(new RadarTilt { TiltName = "1.5" });
            RadarTilts.Add(new RadarTilt { TiltName = "1.8" });
            RadarTilts.Add(new RadarTilt { TiltName = "2.5" });
            RadarTilts.Add(new RadarTilt { TiltName = "3.5" });
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
