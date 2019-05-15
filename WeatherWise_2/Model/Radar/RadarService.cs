using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WeatherWise_2.Core.Models.Radar;
using Windows.Storage;
using Newtonsoft.Json;

namespace WeatherWise_2.Model.Radar
{
    public class RadarService : INotifyPropertyChanged
    {

        private Site_Information _radarSiteData;
        public Site_Information RadarSiteData
        {
            get { return _radarSiteData; }
            set
            {
                if (_radarSiteData != value)
                {
                    _radarSiteData = value;
                    RaisePropertyChanged("RadarSiteData");
                }
            }
        }

        private LocalRadarSites _listRadarSiteData;
        public LocalRadarSites ListRadarSiteData

        {
            get { return _listRadarSiteData; }
            set
            {
                if (_listRadarSiteData != value)
                {
                    _listRadarSiteData = value;
                    RaisePropertyChanged("ListRadarSiteData");
                }
            }
        }

        private StateRadars _stateRadarSites;
        public StateRadars StateRadarSites
        {
            get { return _stateRadarSites; }
            set
            {
                if (_stateRadarSites != value)
                {
                    _stateRadarSites = value;
                    RaisePropertyChanged("StateRadarSites");
                }
            }
        }

        private ConusRadars _conusRadarSites;
        public ConusRadars ConusRadarSites
        {
            get { return _conusRadarSites; }
            set
            {
                if (_conusRadarSites != value)
                {
                    _conusRadarSites = value;
                    RaisePropertyChanged("ConusRadarSites");
                }
            }
        }

        public ConusRadars SortRadarSite_Conus()
        {
            _radarSiteData = new Site_Information();
            _listRadarSiteData = new LocalRadarSites();
            _stateRadarSites = new StateRadars();
            _conusRadarSites = new ConusRadars();
            var _tempRadarSiteList = new LocalRadarSites();
            var unsortedConus = new ConusRadars();
            var radarSiteList_Raw = LoadRadarStations();
            string stateName = "";

            foreach (var item in radarSiteList_Raw)
            {
                if (_tempRadarSiteList.LocalRadars.Any(p => p.ST == item.ST) == false)
                {
                    stateName = item.ST;
                    _listRadarSiteData = new LocalRadarSites();

                    foreach (var site in radarSiteList_Raw)
                    {
                        if (stateName == site.ST)
                        {
                            {
                                _radarSiteData = site;
                                _radarSiteData.RadarSiteDisplayName = string.Format("{0} - {1}", _radarSiteData.ICAO, _radarSiteData.NAME);
                                
                                _tempRadarSiteList.LocalRadars.Add(_radarSiteData);
                                _listRadarSiteData.LocalRadars.Add(_radarSiteData);
                            }
                        }
                    }
                    _stateRadarSites.StateName = stateName;
                    _stateRadarSites.StateRadarSiteList = _listRadarSiteData;
                    _stateRadarSites.StateName = Core.Models.Radar.Helper.RadarHelper.ConvertStateABBRtoName(_stateRadarSites.StateName);
                    
                    unsortedConus.RadarSites_State.Add(_stateRadarSites);
                    _stateRadarSites = new StateRadars();
                }
            }

            var sortedConus = unsortedConus.RadarSites_State.OrderBy(x => x.StateName).ToList();
           _conusRadarSites.RadarSites_State = new ObservableCollection<StateRadars>(sortedConus as List<StateRadars>);
            return _conusRadarSites;
        }


        public static ObservableCollection<Site_Information> LoadRadarStations()
        {
            var radarSiteList = new ObservableCollection<Site_Information>();
            string fileName = string.Format("ms-appx:///Assets/radarsites.json");
            Uri appUri = new Uri(fileName);
            StorageFile jsonStorageFile = StorageFile.GetFileFromApplicationUriAsync(appUri).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            string jsonText = FileIO.ReadTextAsync(jsonStorageFile).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            radarSiteList = JsonConvert.DeserializeObject<ObservableCollection<Site_Information>>(jsonText);
            
            return radarSiteList;
        }


        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
