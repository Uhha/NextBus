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

namespace BusUI.Widget
{
    [Service(Permission = "android.permission.BIND_REMOTEVIEWS", Exported = false)]
    public class WidgetService : RemoteViewsService
    {
        public override IRemoteViewsFactory OnGetViewFactory(Intent intent)
        {
            WidgetListProvider lp = new WidgetListProvider(this.ApplicationContext);
            return lp;
        }
    }
}