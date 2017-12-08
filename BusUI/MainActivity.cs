using Android.App;
using Android.Widget;
using Android.OS;
using System.Linq;
using System;
using Android.Locations;
using Android.Runtime;
using System.Collections.Generic;
using Android.Util;
using System.Text;
using System.Threading.Tasks;
using BusUI.Model;
using System.Threading;

namespace BusUI
{
    [Activity(Label = "BusUI", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            

            StartActivity(typeof(StartActivity));

            //Thread.Sleep(100000);
        }

        






    }

    public static class SBExtentions 
    {
        public static event EventHandler SBAppended;
        public static void AppendNotify(this StringBuilder sb, string text)
        {
            sb.Insert(0, text + "\n");
            SBAppended.Invoke(sb, null);
        }
    }
}

