using System;
using System.Threading.Tasks;

using WeatherWise_2.Helpers;

using Windows.ApplicationModel.Core;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.Services.Maps;

namespace WeatherWise_2.Services
{
    public class LocationService
    {
        private Geolocator _geolocator;

        public event EventHandler<Geoposition> PositionChanged;

        public Geoposition CurrentPosition { get; private set; }

        

        private MapLocationFinderResult _reversePositionLocationResult;
        public MapLocationFinderResult ReversePositionLocationResult
        {
            get { return _reversePositionLocationResult; }
            private set { _reversePositionLocationResult = value; }
        }


        private string _closestTown;
        public string ClosestTown
        {
            get { return _closestTown; }
            set { _closestTown = value; }
        }

        private string _closestState;
        public string ClosestState
        {
            get { return _closestState; }
            set { _closestState = value; }
        }


        public Task<bool> InitializeAsync()
        {
            return InitializeAsync(100);
        }

        public Task<bool> InitializeAsync(uint desiredAccuracyInMeters)
        {
            return InitializeAsync(desiredAccuracyInMeters, (double)desiredAccuracyInMeters / 2);
        }

        public async Task<bool> InitializeAsync(uint desiredAccuracyInMeters, double movementThreshold)
        {
            // More about getting location at https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/get-location
            if (_geolocator != null)
            {
                _geolocator.PositionChanged -= Geolocator_PositionChanged;
                _geolocator = null;
            }

            var access = await Geolocator.RequestAccessAsync();

            bool result;

            switch (access)
            {
                case GeolocationAccessStatus.Allowed:
                    _geolocator = new Geolocator
                    {
                        DesiredAccuracyInMeters = desiredAccuracyInMeters,
                        MovementThreshold = movementThreshold
                    };
                    //ReversePositionLookUp(CurrentPosition);
                    

                    result = true;
                    break;
                case GeolocationAccessStatus.Unspecified:
                case GeolocationAccessStatus.Denied:
                default:
                    result = false;
                    break;
            }

            return result;
        }

        public async Task<BasicGeoposition> GetPosition()
        {
            MapService.ServiceToken = "Ee4W9ulYXwApQto88lkI~bQVSTATq7F6r8oZU8r_qOg~AhNCt9NzYTq15jnVNAfwPM9zRrcVGHswz4SCvwJCIcH54jCUAlTpH1coicJWYdKZ";

            var accessStatus = await Geolocator.RequestAccessAsync();

            if (accessStatus != GeolocationAccessStatus.Allowed) throw new Exception();
            var geolocator = new Geolocator();

            Geoposition pos = await geolocator.GetGeopositionAsync();
            var position = new BasicGeoposition() { Latitude = pos.Coordinate.Latitude, Longitude = pos.Coordinate.Longitude };
           
            return position;
            

        }

        public void ReversePositionLookUp(Geoposition geoposition)
        {
            var thisGeoPosition = geoposition;
            _closestState = thisGeoPosition.CivicAddress.State;
            _closestTown = thisGeoPosition.CivicAddress.City;
            
        }

        public async Task StartListeningAsync()
        {
            if (_geolocator == null)
            {
                throw new InvalidOperationException("ExceptionLocationServiceStartListeningCanNotBeCalled".GetLocalized());
            }

            _geolocator.PositionChanged += Geolocator_PositionChanged;

            CurrentPosition = await _geolocator.GetGeopositionAsync();
        }

        public void StopListening()
        {
            if (_geolocator == null)
            {
                return;
            }

            _geolocator.PositionChanged -= Geolocator_PositionChanged;
        }

        private async void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (args == null)
            {
                return;
            }

            CurrentPosition = args.Position;

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PositionChanged?.Invoke(this, CurrentPosition);
            });
        }
    }
}
