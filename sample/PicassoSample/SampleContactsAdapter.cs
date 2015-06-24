using Android.Content;
using Android.Database;
using Android.Net;
using Android.Provider;
using Android.Views;
using Android.Widget;
using Square.Picasso;

namespace PicassoSample
{
    public class SampleContactsAdapter : CursorAdapter
    {
        private readonly LayoutInflater inflater;

        public SampleContactsAdapter(Context context)
            : base(context, null, 0)
        {
            inflater = LayoutInflater.From(context);
        }

        public override View NewView(Context context, ICursor cursor, ViewGroup viewGroup)
        {
            View itemLayout = inflater.Inflate(Resource.Layout.sample_contacts_activity_item, viewGroup, false);
            itemLayout.Tag = new ViewHolder
            {
                text1 = itemLayout.FindViewById<TextView>(Android.Resource.Id.Text1),
                icon = itemLayout.FindViewById<QuickContactBadge>(Android.Resource.Id.Icon)
            };
            return itemLayout;
        }

        public override void BindView(View view, Context context, ICursor cursor)
        {
            Uri contactUri = ContactsContract.Contacts.GetLookupUri(
                cursor.GetLong(SampleContactsActivity.ContactsQuery.Id),
                cursor.GetString(SampleContactsActivity.ContactsQuery.LookupKey));

            ViewHolder holder = (ViewHolder)view.Tag;
            holder.text1.Text = cursor.GetString(SampleContactsActivity.ContactsQuery.DisplayName);
            holder.icon.AssignContactUri(contactUri);

            Picasso.With(context)
                   .Load(contactUri)
                   .Placeholder(Resource.Drawable.contact_picture_placeholder)
                   .Tag(context)
                   .Into(holder.icon);
        }

        public override int Count
        {
            get { return Cursor == null ? 0 : base.Count; }
        }

        private class ViewHolder : Java.Lang.Object
        {
            internal TextView text1;
            internal QuickContactBadge icon;
        }
    }
}
