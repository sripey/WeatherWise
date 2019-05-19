using System;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using WeatherWise_2.Helpers;
using WeatherWise_2.Model.Radar;
using WeatherWise_2.Services;

using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using WeatherWise_2.Core.Models.Radar;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;

namespace WeatherWise_2.ViewModels
{
    public class RadarViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private bool _stateMenuVisible;
        public bool StateMenuVisible
        {
            get { return _stateMenuVisible; }
            set
            {
                if (_stateMenuVisible != value)
                    _stateMenuVisible = value;
                _stateMenuVisible = !_stateMenuVisible;
                RaisePropertyChanged("LocalMenuVisible");
            }
        }

        private bool _locaclMenuVisible;
        public bool LocalMenuVisible
        {
            get { return _locaclMenuVisible; }
            set
            {
                if (_locaclMenuVisible != value)
                    _locaclMenuVisible = value;
                //_locaclMenuVisible = !_locaclMenuVisible;
                RaisePropertyChanged("LocalMenuVisible");
            }
        }

        private bool _productMenuVisible;
        public bool ProductMenuVisible
        {
            get { return _productMenuVisible; }
            set
            {
                if (_productMenuVisible != value)
                    _productMenuVisible = value;
                RaisePropertyChanged("ProductMenuVisible");
            }
        }

        private bool _tiltMenuVisible;
        public bool TiltMenuVisible
        {
            get { return _tiltMenuVisible; }
            set
            {
                if (_tiltMenuVisible != value)
                {
                    _tiltMenuVisible = value;
                    RaisePropertyChanged("TiltMenuVisible");
                }
            }
        }

        public void ToggleMenuVisible()
        {
            //var MenuVisible = menuItemVisible;
            if (StateMenuVisible == false)
            {
                StateMenuVisible = true;
                RaisePropertyChanged("MenuVisible");
            }
        }


        //**********************//
        //*****RADAR DATA*******//
        //**********************//

        private RadarService _radarServiceData;
        public RadarService RadarServiceData
        {
            get { return _radarServiceData; }
            set { _radarServiceData = value; }
        }

        //*******COMBOBOX MENU********//

        private RadarProduct _selectedRadarProduct;
        public RadarProduct SelectedRadarProduct
        {
            get { return _selectedRadarProduct; }
            set
            {
                if (_selectedRadarProduct != value)
                {
                    _selectedRadarProduct = value;
                    RaisePropertyChanged("SelectedRadarProduct");
                    TiltMenuVisible = true;
                }
            }
        }

        private RadarTilt _selectedRadarTilt;
        public RadarTilt SelectedRadarTilt
        {
            get { return _selectedRadarTilt; }
            set
            {
                if(_selectedRadarTilt != value)
                {
                    _selectedRadarTilt = value;
                    RaisePropertyChanged("SelectedRadarTilt");
                }
            }
        }

        private StateRadars _selectedState;
        public StateRadars SelectedState
        {
            get { return _selectedState; }
            set
            {
                if (_selectedState != value)
                {
                    _selectedState = value;
                    RaisePropertyChanged("SelectedState");
                    LocalMenuVisible = true;
                }
            }
        }

        private Site_Information _selectedSiteInformation;
        public Site_Information SelectedSiteInformation
        {
            get { return _selectedSiteInformation; }
            set
            {
                if (_selectedSiteInformation != value)
                {
                    _selectedSiteInformation = value;
                    RaisePropertyChanged("SelectedSiteInformation");
                    ProductMenuVisible = !ProductMenuVisible;
                    if (_selectedSiteInformation != null)
                    {
                        SelectedSiteInformation_Buffer = value;
                    }
                }
            }
        }

        private Site_Information _selectedSiteInformation_Buffer;
        public Site_Information SelectedSiteInformation_Buffer
        {
            get { return _selectedSiteInformation_Buffer; }
            set
            {
                if (_selectedSiteInformation_Buffer != value)
                {
                    _selectedSiteInformation_Buffer = value;
                    RaisePropertyChanged("SelectedSiteInformation_Buffer");
                    Center = new Geopoint(new BasicGeoposition() { Latitude = _selectedSiteInformation_Buffer.LAT, Longitude = _selectedSiteInformation_Buffer.LON });
                    ZoomLevel = ZoomLevel_RadarSiteView;
                    
                    RaisePropertyChanged("Center");
                    ProductMenuVisible = true;
                }
            }
        }

        private ObservableCollection<Site_Information> _radarSiteList;
        public ObservableCollection<Site_Information> RadarSiteList
        {
            get { return _radarSiteList; }
            set
            {
                if (_radarSiteList != value)
                {
                    _radarSiteList = value;
                    RaisePropertyChanged("RadarSiteList");
                }
            }
        }

        private ConusRadars _radarSites_Conus;
        public ConusRadars RadarSites_Conus
        {
            get { return _radarSites_Conus; }
            set
            {
                if (_radarSites_Conus != value)
                {
                    _radarSites_Conus = value;
                    RaisePropertyChanged("RadarSites_Conus");
                }
            }
        }

        //********** MAP LAYERS***********//

        private RadarLayerService _radarServiceLayerData;
        public RadarLayerService RadarServiceLayerData
        {
            get { return _radarServiceLayerData; }
            set { _radarServiceLayerData = value; }
        }

        private ObservableCollection<MapTileSource> _radarFrameList;
        public ObservableCollection<MapTileSource> RadarFrameList
        {
            get { return _radarFrameList; }
            set { _radarFrameList = value; }
        }

        private RadarImage _radarImageFrame;
        public RadarImage RadarImageFrame
        {
            get { return _radarImageFrame; }
            set { _radarImageFrame = value; }
        }

        private RadarImage[] _radarImageFrameArray;
        public RadarImage[] RadarImageFrameArray
        {
            get { return _radarImageFrameArray; }
            set { _radarImageFrameArray = value; }
        }



        //*****************************//
        //******Switch To Individual Radar Site
        //****************************//

        private bool _canViewSite;
        public bool CanViewSite
        {
            get { return _canViewSite; }
            set
            {
                if(_canViewSite != value)
                {
                    _canViewSite = value;
                    RaisePropertyChanged("CanViewSite");
                }
            }
        }

        private double _zoomLevel;
        public double ZoomLevel
        {
            get { return _zoomLevel; }
            set
            {
                if (_zoomLevel != value)
                {
                    _zoomLevel = value;
                    RaisePropertyChanged("ZoomLevel");
                }
            }
        }

        private Geopoint _center;
        public Geopoint Center
        {
            get { return _center; }
            set
            {
                if(_center != value)
                {
                    if (SelectedSiteInformation.LON != 0)
                    {
                        _center = new Geopoint(new BasicGeoposition() { Latitude = SelectedSiteInformation.LAT, Longitude = SelectedSiteInformation.LON });
                        
                        RaisePropertyChanged("Center");
                    }
                    else
                    {
                        _center = value;
                        RaisePropertyChanged("Center");
                    }
                }
            }
        }

        private int _sliderRadarFrame;
        public int SliderRadarFrame
        {
            get { return _sliderRadarFrame; }
            set
            {
                if(_sliderRadarFrame != value)
                {
                    _sliderRadarFrame = value;
                    RaisePropertyChanged("SliderRadarFrame");
                }
            }
        }

        private MapTileSource _radarAnimationTileSource;
        public MapTileSource RadarAnimationTileSource
        {
            get { return _radarAnimationTileSource; }
            set
            {
                if(_radarAnimationTileSource != value)
                {
                    
                    _radarAnimationTileSource = value;
                    RaisePropertyChanged("RadarAnimationTileSource");
                }
            }
        }

        private MapTileSource _radarFrameTileSource;
        public MapTileSource RadarTileFrameSource
        {
            get { return _radarFrameTileSource; }
            set
            {
                if (_radarFrameTileSource != value)
                {
                    _radarFrameTileSource = value;
                    RaisePropertyChanged("RadarTileFrameSource");
                }
            }
        }

        private readonly BasicGeoposition _defaultPosition = new BasicGeoposition()
        {
            Latitude = 40.32083,
            Longitude = -90.44167
        };
        private const double ZoomLevel_Conus = 5;
        private const double ZoomLevel_RadarSiteView = 10;
        private readonly LocationService _locationService;

        //************************************************

        public RadarViewModel()
        {
            RadarSiteList = new ObservableCollection<Site_Information>();
            _locationService = new LocationService();
            RadarServiceData = new RadarService();
            _radarServiceLayerData = new RadarLayerService();
            SelectedSiteInformation = new Site_Information();
            Center = new Geopoint(_defaultPosition);
            ZoomLevel = ZoomLevel_Conus;
            RadarServiceLayerData = new RadarLayerService();
            RadarFrameList = new ObservableCollection<MapTileSource>();
        }

        public void InitializePage()
        {
            RadarService tempRadarService = new RadarService();
            RadarSites_Conus = tempRadarService.SortRadarSite_Conus();
        }

        public async Task InitializeAsync(MapControl map)
        {
            if (_locationService != null)
            {
                _locationService.PositionChanged += LocationService_PositionChanged;
                var initializationSuccessful = await _locationService.InitializeAsync();

                if (initializationSuccessful)
                {
                    //Center = _locationService.CurrentPosition.Coordinate.Point;
                    await _locationService.StartListeningAsync();
                }

                if(initializationSuccessful && CanViewSite)
                {

                }

                //if (initializationSuccessful && _locationService.CurrentPosition != null)
                //{
                //    Center = new Geopoint(_defaultPosition);
                //}
                //else
                //{
                //    Center = new Geopoint(_defaultPosition);
                //}
            }

            if (map != null)
            {
                map.MapServiceToken = "Ee4W9ulYXwApQto88lkI~bQVSTATq7F6r8oZU8r_qOg~AhNCt9NzYTq15jnVNAfwPM9zRrcVGHswz4SCvwJCIcH54jCUAlTpH1coicJWYdKZ";

                if (SelectedSiteInformation.LAT == 0)
                {
                    map.Center = new Geopoint(_defaultPosition);
                    
                    map.ZoomLevel = ZoomLevel_Conus;
                    //AddMapIcon(map, Center, "Map_YourLocation".GetLocalized());
                    AddRadarLocationIcons(RadarSites_Conus, map);
                }
                else
                {
                    map.Center = new Geopoint(new BasicGeoposition() { Latitude = SelectedSiteInformation.LAT, Longitude = SelectedSiteInformation.LON });
                    RaisePropertyChanged("Center");
                }
            }
        }

        public async void GetRadarFrameList()
        {
            for (int i = 0; i < RadarAnimationTileSource.FrameCount; i++)
            {
                MapTileSource mapTileSource = new MapTileSource();
                mapTileSource = await _radarServiceLayerData.AddIndividualRadarFrameTile(i);
                RadarFrameList.Add(mapTileSource);
            }
        }

        public async Task DisplayIndividualRadarFrame(MapControl map)
        {
            ClearRadarImages();
            RadarAnimationTileSource = await _radarServiceLayerData.AddIndividualRadarFrameTile(SliderRadarFrame);
            map.TileSources.Add(RadarAnimationTileSource);
        }

        public async Task InitializeRadarImages(MapControl map)
        {
            RadarAnimationTileSource = await _radarServiceLayerData.AddAnimatedRadarTiles();
            GetRadarFrameList();
        }

        private void ClearRadarImages()
        {
            _radarAnimationTileSource = null;
            RaisePropertyChanged("RadarMapTileSourceData");
        }
       

        private void LocationService_PositionChanged(object sender, Geoposition geoposition)
        {
            if (geoposition != null)
            {
                //Center = geoposition.Coordinate.Point;
            }
        }
        private void AddRadarLocationIcons(ConusRadars conusRadars,MapControl map)
        {
            var radarsites = conusRadars;
            foreach (StateRadars item in radarsites.RadarSites_State)
            {
                foreach (Site_Information member in item.StateRadarSiteList.LocalRadars)
                {
                    var sitePosition = new Geopoint(new BasicGeoposition(){ Latitude = member.LAT,Longitude = member.LON});
                    AddMapIcon(map, sitePosition, member.ICAO);
                }
            }
        }

        private void AddMapIcon(MapControl map, Geopoint position, string title)
        {
            var mapIcon = new MapIcon()
            {
                Location = position,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = title,

                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Map/radarSitePositionIcon.png")),
                ZIndex = 0
                
            };
            map.MapElements.Add(mapIcon);
        }

        public override void Cleanup()
        {
            if (_locationService != null)
            {
                _locationService.PositionChanged -= LocationService_PositionChanged;
                _locationService.StopListening();
            }
            base.Cleanup();
        }
    }
}
