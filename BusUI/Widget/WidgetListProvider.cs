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
using NextBus;

namespace BusUI.Widget
{
    public class WidgetListProvider : Java.Lang.Object, RemoteViewsService.IRemoteViewsFactory
    {
        private List<ListItem> _listItemList = new List<ListItem>();
        private Context _context;
        private List<string> _stops = new List<string> { "302375", "303575", "302376", "302419", "400323", "400324", "400325", "400353" };

        public WidgetListProvider(Context contextNew)
        {
            _context = contextNew;
        }

        public int Count
        {
            get { return _listItemList.Count; }
        }

        public bool HasStableIds { get { return true; } }

        public RemoteViews LoadingView {  get { return null; } }

        public int ViewTypeCount { get { return 1; } }

        public long GetItemId(int position)
        {
            return position;
        }

        public RemoteViews GetViewAt(int position)
        {
            RemoteViews remoteView = new RemoteViews(_context.PackageName, Resource.Layout.widgetlistitem);
            ListItem listItem = _listItemList[position];
            remoteView.SetTextViewText(Resource.Id.stop, listItem.Stop);
            remoteView.SetTextViewText(Resource.Id.schedule, listItem.Schedule);
            return remoteView;
        }

        public void OnCreate()
        {
        }

        public void OnDataSetChanged()
        {
            _listItemList.Clear();
            foreach (var stop in _stops)
            {
                ListItem listItem = new ListItem(stop);
                _listItemList.Add(listItem);
            }
        }

        public void OnDestroy()
        {
        }

        
    }

    public class ListItem
    {
        public string Stop;
        public string Schedule;

        public ListItem(string stopid)
        {
            var schedules = NextBus.Operations.ScheduleForStop(stopid);
            if (schedules == null)
            {
                Stop = "";
                Schedule = "";
                return;
            }
            StringBuilder sb = new StringBuilder();
            int cnt = 5;
            foreach (var sched in schedules)
            {
                TimeSpan ts = (sched.MonitoredVehicleJourney.MonitoredCall.ExpectedArrivalTime).Subtract(DateTime.Now);
                var arrivalTime = (sched.MonitoredVehicleJourney.MonitoredCall.ExpectedArrivalTime.Year == 1) ?
                    sched.MonitoredVehicleJourney.MonitoredCall.Extensions.Distances.PresentableDistance :
                    ts.Minutes + ":" + ts.Seconds;

                sb.Append(sched.MonitoredVehicleJourney.PublishedLineName + ": " +
                    arrivalTime + " | ");
                cnt--;
                if (cnt == 0) break;
            }
            Schedule = sb.ToString();
            var stopObj = Operations.GetStopById(stopid);
            Stop = (stopObj != null) ? $"{stopObj.data.name} - {stopObj.data.direction} ({stopObj.data.routes.Select(o => o.shortName).Aggregate((a, b) => a + "," + b)})" : "";
        }
    }
}