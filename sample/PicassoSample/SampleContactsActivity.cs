using Android.Provider;
using Android.Net;
using Android.OS;
using Android.Content;
using Android.Database;
using Android.Widget;
using Android.App;
using Android.Support.V4.App;

using LoaderManager = Android.Support.V4.App.LoaderManager;
using Loader = Android.Support.V4.Content.Loader;
using CursorLoader = Android.Support.V4.Content.CursorLoader;

using Square.Picasso;

namespace PicassoSample
{
    [Activity]
    public class SampleContactsActivity : PicassoSampleActivity, LoaderManager.ILoaderCallbacks
    {
        private SampleContactsAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.sample_contacts_activity);

            adapter = new SampleContactsAdapter(this);

            ListView lv = FindViewById<ListView>(Android.Resource.Id.List);
            lv.Adapter = adapter;
            lv.ScrollStateChanged += (sender, e) =>
            {
                Picasso picasso = Picasso.With(this);
                if (e.ScrollState == ScrollState.Idle || e.ScrollState == ScrollState.TouchScroll)
                {
                    picasso.ResumeTag(this);
                }
                else
                {
                    picasso.PauseTag(this);
                }
            };

            SupportLoaderManager.InitLoader(ContactsQuery.QueryId, null, this);
        }

        public Loader OnCreateLoader(int id, Bundle args)
        {
            if (id == ContactsQuery.QueryId)
            {
                return new CursorLoader(
                    this,
                    ContactsQuery.ContentUri,
                    ContactsQuery.Projection,
                    ContactsQuery.Selection,
                    null,
                    ContactsQuery.SortOrder);
            }
            return null;
        }

        public void OnLoadFinished(Loader loader, Java.Lang.Object data)
        {
            adapter.SwapCursor((ICursor)data);
        }

        public void OnLoaderReset(Loader loader)
        {
            adapter.SwapCursor(null);
        }

        public static class ContactsQuery
        {
            static ContactsQuery()
            {
                bool isHoneycomb = (int)Build.VERSION.SdkInt >= (int)BuildVersionCodes.Honeycomb;

                string displayName = isHoneycomb
                        ? ContactsContract.Contacts.InterfaceConsts.DisplayNamePrimary
                        : ContactsContract.Contacts.InterfaceConsts.DisplayName;
                ContentUri = ContactsContract.Contacts.ContentUri;
                Selection = string.Format("{0}<>'' AND {1}=1", displayName, ContactsContract.Contacts.InterfaceConsts.InVisibleGroup);
                SortOrder = isHoneycomb
                    ? ContactsContract.Contacts.InterfaceConsts.SortKeyPrimary
                    : ContactsContract.Contacts.InterfaceConsts.DisplayName;
                Projection = new string[] { 
                    ContactsContract.Contacts.InterfaceConsts.Id,
                    ContactsContract.Contacts.InterfaceConsts.LookupKey,
                    displayName,
                    isHoneycomb
                        ? ContactsContract.Contacts.InterfaceConsts.PhotoThumbnailUri 
                        : ContactsContract.Contacts.InterfaceConsts.Id,
                    SortOrder 
                };
            }

            public const int QueryId = 1;

            public const int Id = 0;
            public const int LookupKey = 1;
            public const int DisplayName = 2;

            public static readonly Uri ContentUri;
            public static readonly string Selection;
            public static readonly string SortOrder;
            public static readonly string[] Projection;
        }
    }
}
