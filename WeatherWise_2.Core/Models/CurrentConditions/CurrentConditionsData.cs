using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherWise_2.Core.Models.CurrentConditions
{
    public class CurrentCondition
    {
        public int code { get; set; }
        public string day { get; set; }
        public string night { get; set; }
        public int icon { get; set; }
    }

    public class CurrentConditions
    {
        public List<CurrentCondition> currentConditions { get; set; }

        public CurrentConditions()
        {

        }
    }
}
