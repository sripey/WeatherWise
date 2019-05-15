using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;

namespace WeatherWise_2.Model.CurrentConditions
{
    public class Location
    {
        public async static Task<BasicGeoposition> GetPosition()
        {
            MapService.ServiceToken = "Ee4W9ulYXwApQto88lkI~bQVSTATq7F6r8oZU8r_qOg~AhNCt9NzYTq15jnVNAfwPM9zRrcVGHswz4SCvwJCIcH54jCUAlTpH1coicJWYdKZ";

            var accessStatus = await Geolocator.RequestAccessAsync();

            if (accessStatus != GeolocationAccessStatus.Allowed) throw new Exception();

            var geolocator = new Geolocator();

            Geoposition pos = await geolocator.GetGeopositionAsync();
            var lat = pos.Coordinate.Latitude;
            var lon = pos.Coordinate.Longitude;

            var position = new BasicGeoposition() { Latitude = lat, Longitude = lon };
            Geopoint point = new Geopoint(position);


            return position;


        }

        public async static Task<MapLocationFinderResult> ReversePositionLookup(BasicGeoposition CurrentLocation)
        {
            var currentLocation = CurrentLocation;

            Geopoint pointToReverseGeocode = new Geopoint(currentLocation);
            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);
            if (result.Status == MapLocationFinderStatus.Success)
            {
                return result;
            }
            else
            {
                return result;
            }
        }

        public static double GetDistanceBetweenTwoLocations(string lat1, string lon1, string lat2, string lon2)
        {
            double distance = 0;
            const double PIx = 3.141592653589793;
            const double radius = 6371;

            double latitude1 = Convert.ToDouble(lat1);
            double latitude2 = Convert.ToDouble(lat2);
            double longitude1 = Convert.ToDouble(lon1);
            double longitude2 = Convert.ToDouble(lon2);

            double dlat = Radians(latitude2 - latitude1);
            double dlon = Radians(longitude2 - longitude1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2))
                + Math.Cos(Radians(latitude1)) * Math.Cos(Radians(latitude2))
                * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));

            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            distance = angle * radius;
            return distance;

        }

        public async static Task<string> GetClosestTown()
        {
            string town = "";
            var position = await GetPosition();
            var locations = await ReversePositionLookup(position);

            foreach (var location in locations.Locations)
            {
                town = location.Address.Town;
            }

            return town;
        }

        public async static Task<string> GetClosestState()
        {
            string state = "";
            var position = await GetPosition();
            var locations = await ReversePositionLookup(position);

            foreach (var location in locations.Locations)
            {
                state = location.Address.Region;
            }

            return state;
        }

        public static double Radians(double x)
        {
            const double PIx = 3.141592653589793;

            return x * PIx / 180;
        }

    }
}
