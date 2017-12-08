using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Android.Util;
using System.Threading.Tasks;

namespace BusUI.Model
{
    [Activity(Label = "BusUI", MainLauncher = false, Icon = "@mipmap/icon")]
    internal class LocationFinder
    {
        static readonly string TAG = "X:" + typeof(LocationFinder).Name;
        public Location LocationCoordinates { get; private set; }
        private LocationManager _locationManager;
        public string LocationAddress { get; private set; }
        private string _locationProvider;

        public LocationFinder(LocationManager locationManager, StartActivity startActivity)
        {
            _locationManager = locationManager;
            InitializeLocationManager();
        }

        private void InitializeLocationManager()
        {
            
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
            Log.Debug(TAG, "Using " + _locationProvider + ".");
        }

        internal void OnResume(StartActivity startActivity)
        {
            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, startActivity);
            OnLocationChanged(_locationManager.GetLastKnownLocation(_locationProvider), startActivity);
        }

        internal void OnPause(StartActivity startActivity)
        {
            _locationManager.RemoveUpdates(startActivity);
        }

        public async void OnLocationChanged(Location location, StartActivity startActivity)
        {
            LocationCoordinates = location;
            if (LocationCoordinates == null)
            {
                LocationAddress = "Unable to determine your location. Try again in a short while.";
            }
            else
            {
                //_addressText = string.Format("{0:f6},{1:f6}", _currentLocation.Latitude, _currentLocation.Longitude);
                Address address = await ReverseGeocodeCurrentLocationAsync(startActivity);
                DisplayAddress(address);
            }
        }

        private async Task<Address> ReverseGeocodeCurrentLocationAsync(StartActivity startActivity)
        {
            try
            {
                Geocoder geocoder = new Geocoder(startActivity);
                IList<Address> addressList =
                    await geocoder.GetFromLocationAsync(LocationCoordinates.Latitude, LocationCoordinates.Longitude, 10);

                Address address = addressList.FirstOrDefault();
                return address;
            }
            catch (Exception e)
            {
                Console.WriteLine(TAG, "ERROR: " + e.Message + " " + e.InnerException?.Message + ".");
                Log.Debug(TAG, "ERROR: " + e.Message + " " + e.InnerException?.Message + ".");
                return null;
            }
        }

        void DisplayAddress(Address address)
        {
            if (address != null)
            {
                StringBuilder deviceAddress = new StringBuilder();
                for (int i = 0; i <= address.MaxAddressLineIndex; i++)
                {
                    deviceAddress.AppendLine(address.GetAddressLine(i));
                }

                LocationAddress = deviceAddress.ToString();
            }
            else
            {
                LocationAddress = "Unable to determine the address. Try again in a few minutes.";
            }
        }
    }
}