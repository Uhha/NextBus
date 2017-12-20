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
using BusUI.Model;
using Android.Views.InputMethods;
using Android.Support.V4.Widget;
using Android.Graphics;
using System.ComponentModel;
using Java.Lang;

namespace BusUI
{
    [Activity(Label = "StartActivity")]
    public class StartActivity : Activity, ILocationListener
    {
        InputMethodManager imm;
        private System.Text.StringBuilder _sb;
        private LocationFinder _locationFinder;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            

            _sb = new System.Text.StringBuilder();
            var locationManager = (LocationManager)GetSystemService(LocationService);
            _locationFinder = new LocationFinder(locationManager, this);
            SBExtentions.SBAppended += SBExtentions_SBAppended;

            var stops = NextBus.Operations.StopsNearMe(40.682607f, -73.961773f, 0.005f);
            //var stops = Operations.StopsNearMe(40.767496, -73.691098, 0.015);
            string stopstext = "";
            foreach (var stop in stops)
            {
                stopstext += ($"{stop.id}: {stop.name}, {stop.direction} -- {string.Join(",", stop.routes.Select(o => o.shortName).ToArray())} \n");
            }
            _sb.AppendNotify(stopstext);

            Button button = FindViewById<Button>(Resource.Id.submitBut);
            button.Click += Button_Click;
            button = FindViewById<Button>(Resource.Id.addressButton);
            button.Click += AddressButton_OnClick;
            button = FindViewById<Button>(Resource.Id.stopsNear);
            button.Click += StopsNear_OnClick; 
        }

        private void StopsNear_OnClick(object sender, EventArgs e)
        {
            var stops = NextBus.Operations.StopsNearMe((float)_locationFinder.LocationCoordinates.Latitude, (float)_locationFinder.LocationCoordinates.Longitude, 0.005f);
            string stopstext = "";
            foreach (var stop in stops)
            {
                stopstext += ($"{stop.id}: {stop.name}, {stop.direction} -- {string.Join(",", stop.routes.Select(o => o.shortName).ToArray())} \n");
            }
            _sb.AppendNotify(stopstext);
        }

        protected override void OnResume()
        {
            base.OnResume();
            _locationFinder.OnResume(this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationFinder.OnPause(this);
        }

        private void SBExtentions_SBAppended(object sender, EventArgs e)
        {
            var view = FindViewById<TextView>(Resource.Id.textDisplay);
            view.Text = _sb.ToString();
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            imm.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, 0);
            var text = FindViewById<EditText>(Resource.Id.byId).Text;
            var schedules = NextBus.Operations.ScheduleForStop(text);
            if (schedules == null) return;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var sched in schedules)
            {
                TimeSpan ts = (sched.MonitoredVehicleJourney.MonitoredCall.ExpectedArrivalTime).Subtract(DateTime.Now);
                var arrivalTime = (sched.MonitoredVehicleJourney.MonitoredCall.ExpectedArrivalTime.Year == 1) ?
                    sched.MonitoredVehicleJourney.MonitoredCall.Extensions.Distances.PresentableDistance :
                    "arriving in " + ts.Minutes + ":" + ts.Seconds + "minutes";
                sb.Append(sched.MonitoredVehicleJourney.PublishedLineName + ": " +
                    sched.MonitoredVehicleJourney.MonitoredCall.Extensions.Distances.StopsFromCall + " Stops away, " + arrivalTime + "\n");
            }
            _sb.AppendNotify(sb.ToString());
        }

        


        private void AddressButton_OnClick(object sender, EventArgs eventArgs)
        {
            _sb.AppendNotify(_locationFinder.LocationAddress);
        }

        public void OnLocationChanged(Location location)
        {
            _locationFinder.OnLocationChanged(location, this);
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }
    }
}