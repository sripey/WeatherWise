using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WeatherWise_2.Core.Models.Radar 
{
    public class Site_Information 
    {
        public int NCDCID { get; set; }
        public string ICAO { get; set; }
        public int WBAN { get; set; }
        public string NAME { get; set; }
        public string COUNTR { get; set; }
        public string Y { get; set; }
        public string ST { get; set; }
        public string COUNTY { get; set; }
        public float LAT { get; set; }
        public float LON { get; set; }
        public int ELEV { get; set; }
        public int UTC { get; set; }
        public string STNTYPE { get; set; }

        public string RadarSiteDisplayName { get; set; }

        public ObservableCollection<RadarProduct> RadarProducts { get; set; }

        public Site_Information()
        {
            RadarProducts = new ObservableCollection<RadarProduct>();
            RadarProducts.Add(new RadarProduct { ProductName = "Reflectivity", ProductAbbrev = "N0R" });
            RadarProducts.Add(new RadarProduct { ProductName = "Base Velocity", ProductAbbrev = "N0V" });
            RadarProducts.Add(new RadarProduct { ProductName = "Storm Relative Velocity", ProductAbbrev = "N0S" });
            RadarProducts.Add(new RadarProduct { ProductName = "Composite Reflectivity", ProductAbbrev = "NCR" });
        }
    }

    public class LocalRadarSites
    {
        public ObservableCollection<Site_Information> LocalRadars { get; set; }

        public LocalRadarSites()
        {
            LocalRadars = new ObservableCollection<Site_Information>();
        }
    }

    public class StateRadars
    {
        public string StateName { get; set; }
        
        public LocalRadarSites StateRadarSiteList { get; set; }

        public StateRadars()
        {
            StateName = "";
            StateRadarSiteList = new LocalRadarSites();
            
        }
    }

    public class ConusRadars
    {
        public ObservableCollection<StateRadars> RadarSites_State { get; set; }

        public ConusRadars()
        {
            RadarSites_State = new ObservableCollection<StateRadars>();
        }
    }
}
