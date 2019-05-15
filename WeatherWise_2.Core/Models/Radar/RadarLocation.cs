using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WeatherWise_2.Core.Models.Radar
{
    public class RadarLocation :INotifyPropertyChanged
    {
        private string _state;

        public string State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    RaisePropertyChanged("State");
                }
            }
        }

        private string _site;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Site
        {
            get { return _site; }
            set { _site = value; }
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
