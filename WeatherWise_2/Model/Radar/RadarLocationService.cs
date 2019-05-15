using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWise_2.Core.Models.Radar;
using Windows.Storage;
using Newtonsoft.Json;


namespace WeatherWise_2.Model.Radar
{
    public class RadarLocationService : INotifyPropertyChanged
    {
        private ObservableCollection<RadarLocation> _radarLocations;
        

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<RadarLocation> RadarLocations
        {
            get { return _radarLocations; }
            set { _radarLocations = value;
                RaisePropertyChanged("RadarLocations"); }
        }


        public static  ObservableCollection<Site_Information> LoadRadarStations()
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

    }
}
