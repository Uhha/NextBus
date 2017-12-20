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
using Android.Support.V4.Widget;
using Android.Graphics;
using System.ComponentModel;
using Java.Lang;

namespace BusUI.Widget
{
    [BroadcastReceiver (Label = "WDSS")]
    [IntentFilter (new string [] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData ("android.appwidget.provider", Resource = "@xml/widgetproviderinfo")]
    public class ScheduleWidgetProvider : AppWidgetProvider
    {
        public static string TOAST_ACTION = "TOAST_ACTION";


        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            //not sure if we need base call here
            base.OnUpdate(context, appWidgetManager, appWidgetIds);

            ComponentName thisWidget = new ComponentName(context, Java.Lang.Class.FromType(typeof(ScheduleWidgetProvider)).Name);

            int[] allWidgetIds = appWidgetManager.GetAppWidgetIds(thisWidget);
            foreach (int widgetId in allWidgetIds)
            {
                RemoteViews remoteViews = new RemoteViews(context.PackageName, Resource.Layout.widgetlayoutgrid);

                Intent svcIntent = new Intent(context, typeof(WidgetService));
                svcIntent.SetPackage(context.PackageName);
                svcIntent.PutExtra(AppWidgetManager.ExtraAppwidgetId, widgetId);

                svcIntent.SetData(Android.Net.Uri.Parse(svcIntent.ToUri(Android.Content.IntentUriType.AndroidAppScheme)));

                remoteViews.SetEmptyView(Resource.Id.widgetList, Resource.Id.empty_view);
                remoteViews.SetRemoteAdapter(Resource.Id.widgetList, svcIntent);

                appWidgetManager.NotifyAppWidgetViewDataChanged(appWidgetIds, Resource.Id.widgetList);
                //appWidgetManager.UpdateAppWidget(widgetId, remoteViews);


                Intent intent = new Intent(context, typeof(ScheduleWidgetProvider));
                intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
                intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, widgetId);

                PendingIntent pendingIntent = PendingIntent.GetBroadcast(context,
                        0, intent, PendingIntentFlags.UpdateCurrent);

                remoteViews.SetPendingIntentTemplate(Resource.Id.widgetList, pendingIntent);
                
                //remoteViews.SetOnClickPendingIntent(Resource.Id.imageButton1, pendingIntent);
                appWidgetManager.UpdateAppWidget(widgetId, remoteViews);
            }

        }


        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);
            AppWidgetManager mgr = AppWidgetManager.GetInstance(context);
            int appWidgetId = intent.GetIntExtra(AppWidgetManager.ExtraAppwidgetId,
                    AppWidgetManager.InvalidAppwidgetId);
            mgr.NotifyAppWidgetViewDataChanged(appWidgetId, Resource.Id.widgetList);
        }

        //private string GetText()
        //{
        //    var schedules = NextBus.Operations.ScheduleForStop("302376");
        //    if (schedules == null) return "";
        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //    foreach (var sched in schedules)
        //    {
        //        TimeSpan ts = (sched.MonitoredVehicleJourney.MonitoredCall.ExpectedArrivalTime).Subtract(DateTime.Now);
        //        var arrivalTime = (sched.MonitoredVehicleJourney.MonitoredCall.ExpectedArrivalTime.Year == 1) ?
        //            sched.MonitoredVehicleJourney.MonitoredCall.Extensions.Distances.PresentableDistance :
        //            "arriving in " + ts.Minutes + ":" + ts.Seconds + "minutes";
        //        sb.Append(sched.MonitoredVehicleJourney.PublishedLineName + ": " +
        //            sched.MonitoredVehicleJourney.MonitoredCall.Extensions.Distances.StopsFromCall + " Stops away, " + arrivalTime + "\n");
        //    }
        //    return sb.ToString();

        //}

        //private void GetUpdate(RemoteViews remoteViews)
        //{


        //    GetInfo("302419", out string stopDetails, out string schedule);
        //    remoteViews.SetTextViewText(Resource.Id.stop1, stopDetails);
        //    remoteViews.SetTextViewText(Resource.Id.sche1, schedule);

        //    GetInfo("303575", out stopDetails, out schedule);
        //    remoteViews.SetTextViewText(Resource.Id.stop2, stopDetails);
        //    remoteViews.SetTextViewText(Resource.Id.sche2, schedule);

        //}
    }
}