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
using Android.Support.V4.Widget;
using Android.Graphics;
using System.ComponentModel;
using Java.Lang;

namespace BusUI
{
    [Activity(Label = "TestActivity")]
    public class TestActivity : Activity
    {

        private SwipeRefreshLayout refreshLayout;
        private ListView listitem;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.widgetlayoutgrid);

            //listitem = FindViewById<ListView>(Resource.Id.listView1);
            //List<string> items = new List<string>();
            //items.Add("C#");
            //items.Add("ASP.NET");
            //items.Add("JAVA");
            //items.Add("MVC");
            //items.Add("Swift");
            //items.Add("JQuery");
            //items.Add("ASP");
            //items.Add("JSP");
            //items.Add("C");
            //items.Add("PHP");
            //items.Add("Ruby");
            //ArrayAdapter<string> liststring = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleExpandableListItem1, items);
            //listitem.Adapter = liststring;

            //refreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.refreshlay);
            //refreshLayout.SetColorSchemeColors(Color.Red, Color.Green, Color.Blue, Color.Yellow);
            //refreshLayout.Refresh += RefreshLayout_Refresh;

        }

        private void RefreshLayout_Refresh(object sender, EventArgs e)
        {
            //Data Refresh Place  
            BackgroundWorker work = new BackgroundWorker();
            work.DoWork += Work_DoWork;
            work.RunWorkerCompleted += Work_RunWorkerCompleted;
            work.RunWorkerAsync();
        }
        private void Work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            refreshLayout.Refreshing = false;
        }
        private void Work_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
        }
    }
}