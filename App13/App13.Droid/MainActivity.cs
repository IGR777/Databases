using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using App13.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App13.Droid
{
	[Activity (Label = "App13.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : ListActivity
	{
        IList<User> _users;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ArrayAdapter<User> adapter = null;

            Task.Run(() =>
            {
                _users = DbHelper.Instance.GetUsers();
                adapter = new ArrayAdapter<User>(this, Android.Resource.Layout.SimpleListItem1, _users);

                RunOnUiThread(() =>
                {
                    this.ListAdapter = adapter;
                    adapter.NotifyDataSetChanged();
                });
            });
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);

            var intent = new Intent(this, typeof(ProfileActivity));
            intent.PutExtra("id", _users[position].ID);
            StartActivity(intent);
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.mainmenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Resource.Id.im_action_new_user:
                    var intent = new Intent(this, typeof(ProfileActivity));
                    StartActivity(intent);
                    break;
            }
            return true;
        }

    }
}


