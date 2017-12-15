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
using Android.Appwidget;
using Android.Locations;
using NextBus;

namespace BusUI.Model
{
    [BroadcastReceiver (Label = "WDSS")]
    [IntentFilter (new string [] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData ("android.appwidget.provider", Resource = "@xml/widgetproviderinfo")]
    public class MyWidgetProvider : AppWidgetProvider
    {
        private static string ACTION_CLICK = "ACTION_CLICK";
        private List<string> _stops = new List<string> { "302375", "303575", "302376", "302419" };

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            ComponentName thisWidget = new ComponentName(context, Java.Lang.Class.FromType(typeof(MyWidgetProvider)).Name);

            int[] allWidgetIds = appWidgetManager.GetAppWidgetIds(thisWidget);
            foreach (int widgetId in allWidgetIds)
            {
                RemoteViews remoteViews = new RemoteViews(context.PackageName, Resource.Layout.widgetlayoutgrid);
                
                
                //var updatedText = GetText();
                //remoteViews.SetTextViewText(Resource.Id.update, updatedText);
                GetUpdate(remoteViews);


                //(ProgressDialogStyle.Horizontal)

                //Bundle options = appWidgetManager.GetAppWidgetOptions(widgetId);
                //layout = RemoteViewsFactory.CreateLayout(context, id, options);


                Intent intent = new Intent(context, typeof(MyWidgetProvider));

                intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
                intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);

                PendingIntent pendingIntent = PendingIntent.GetBroadcast(context,
                        0, intent, PendingIntentFlags.UpdateCurrent);
                remoteViews.SetOnClickPendingIntent(Resource.Id.sche1, pendingIntent);
                remoteViews.SetOnClickPendingIntent(Resource.Id.sche2, pendingIntent);
                appWidgetManager.UpdateAppWidget(widgetId, remoteViews);
            }

        }

        private string GetText()
        {
            var schedules = NextBus.Operations.ScheduleForStop("302376");
            if (schedules == null) return "";
            StringBuilder sb = new StringBuilder();
            foreach (var sched in schedules)
            {
                TimeSpan ts = (sched.MonitoredVehicleJourney.MonitoredCall.ExpectedArrivalTime).Subtract(DateTime.Now);
                var arrivalTime = (sched.MonitoredVehicleJourney.MonitoredCall.ExpectedArrivalTime.Year == 1) ?
                    sched.MonitoredVehicleJourney.MonitoredCall.Extensions.Distances.PresentableDistance :
                    "arriving in " + ts.Minutes + ":" + ts.Seconds + "minutes";
                sb.Append(sched.MonitoredVehicleJourney.PublishedLineName + ": " +
                    sched.MonitoredVehicleJourney.MonitoredCall.Extensions.Distances.StopsFromCall + " Stops away, " + arrivalTime + "\n");
            }
            return sb.ToString();
            
        }

        private void GetUpdate(RemoteViews remoteViews)
        {


            GetInfo("302419", out string stopDetails, out string schedule);
            remoteViews.SetTextViewText(Resource.Id.stop1, stopDetails);
            remoteViews.SetTextViewText(Resource.Id.sche1, schedule);

            GetInfo("303575", out stopDetails, out schedule);
            remoteViews.SetTextViewText(Resource.Id.stop2, stopDetails);
            remoteViews.SetTextViewText(Resource.Id.sche2, schedule);
            
        }

        private void GetInfo(string stopid, out string stopDetails, out string schedule)
        {
            var schedules = NextBus.Operations.ScheduleForStop(stopid);
            if (schedules == null)
            {
                stopDetails = "";
                schedule = "";
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
            schedule = sb.ToString();
            var stopObj = Operations.GetStopById("302375");
            stopDetails = (stopObj != null) ? $"{stopObj.data.name} - {stopObj.data.direction} ({stopObj.data.routes.Select(o => o.shortName).Aggregate((a, b) => a + "," + b)})" : "";
        }

        private void BuildView(RemoteViews remoteViews, Context context)
        {
            

            GridLayout gv = new GridLayout(context)
            {
                ColumnCount = 1,
                RowCount = 2
            };
            TextView tv = new TextView(context);
            tv.TextSize = 6;
            TextView tv2 = new TextView(context);
            tv2.TextSize = 12;
            gv.AddView(tv);
            gv.AddView(tv2);

            


        }


    }



}