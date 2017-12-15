//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;

//namespace BusUI.Widget
//{
//    class WidgetRemoveViewFactory : RemoteViewsService.IRemoteViewsFactory
//    {
//        private Context mContext;
//        private Cursor mCursor;

//        public WidgetRemoveViewFactory(Context applicationContext, Intent intent)
//        {
//            mContext = applicationContext;
//        }

//        public int Count
//        {
//            get { return mCursor == null ? 0 : mCursor.getCount(); }
//        }

//        public bool HasStableIds => throw new NotImplementedException();

//        public RemoteViews LoadingView => throw new NotImplementedException();

//        public int ViewTypeCount => throw new NotImplementedException();

//        public IntPtr Handle => throw new NotImplementedException();

//        public void Dispose()
//        {
//            throw new NotImplementedException();
//        }

//        public long? GetItemId(int position)
//        {
//            if (position == AdapterView.InvalidPosition ||
//               mCursor == null || !mCursor.moveToPosition(position))
//            {
//                return null;
//            }
//            RemoteViews rv = new RemoteViews(mContext.PackageName, R.layout.collection_widget_list_item);
//            rv.SetTextViewText(R.id.widgetItemTaskNameLabel, mCursor.getString(1));

//            return rv;
//        }

//        public RemoteViews GetViewAt(int position)
//        {
//            throw new NotImplementedException();
//        }

//        public void OnCreate()
//        {
//        }

//        public void OnDataSetChanged()
//        {
//            if (mCursor != null)
//            {
//                mCursor.close();
//            }

//            long identityToken = Binder.ClearCallingIdentity();
//            Uri uri = System.Diagnostics.Contracts.Contract.PATH_TODOS_URI;
//            mCursor = mContext.getContentResolver().query(uri,
//                    null,
//                    null,
//                    null,
//                    Contract._ID + " DESC");

//            Binder.RestoreCallingIdentity(identityToken);
//        }

//        public void OnDestroy()
//        {
//            if (mCursor != null)
//            {
//                mCursor.close();
//            }
//        }
//    }
//}