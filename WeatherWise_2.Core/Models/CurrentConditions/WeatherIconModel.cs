using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherWise_2.Core.Models.CurrentConditions
{
    public class WeatherIconModel
    {
        private int _code;
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }

        private string _day;
        public string Day
        {
            get { return _day; }
            set { _day = value; }
        }

        private string _night;
        public string Night
        {
            get { return _night; }
            set { _night = value; }
        }

        private int _icon;
        public int Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        private string _currentIconPath;
        public string CurrentIconPath
        {
            get { return _currentIconPath; }
            set { _currentIconPath = value; }
        }

    }
}
