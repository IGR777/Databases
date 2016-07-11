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
using App13.Models;
using System.Threading.Tasks;

namespace App13.Droid
{
    [Activity(Label = "ProfileActivity")]
    public class ProfileActivity : Activity
    {
        int _id;
        EditText _firstName;
        EditText _lastName;
        EditText _age;
        EditText _occupation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Main);

            Console.WriteLine("OnCreate started. {0}", DateTime.Now.ToString("o"));

            _firstName = FindViewById<EditText>(Resource.Id.firstName);
            _lastName = FindViewById<EditText>(Resource.Id.lastName);
            _age = FindViewById<EditText>(Resource.Id.age);
            _occupation = FindViewById<EditText>(Resource.Id.occupation);

            Task.Run(() =>
            {


                Console.WriteLine("OnCreate async task started. {0}", DateTime.Now.ToString("o"));
                _id = Intent.GetIntExtra("id", 0);
                if (_id != 0)
                {
                    var user = DbHelper.Instance.GetUser(_id);

                    RunOnUiThread(() =>
                    {
                        Console.WriteLine("OnCreate UI task started. {0}", DateTime.Now.ToString("o"));
                        _firstName.Text = user.FirstName;
                        _lastName.Text = user.LastName;
                        _age.Text = user.Age.ToString();
                        _occupation.Text = user.Occupation;
                        Console.WriteLine("OnCreate UI task finished. {0}", DateTime.Now.ToString("o"));
                    });

                }
            });

            Console.WriteLine("OnCreate finished. {0}", DateTime.Now.ToString("o"));

        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.profilemenu, menu);
            if(_id==0)
                menu.FindItem(Resource.Id.im_action_remove).SetVisible(false);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Resource.Id.im_action_save:
                    var ages = 0;
                    Int32.TryParse(_age.Text, out ages);
                    DbHelper.Instance.AddOrUpdateUser(
                    new User
                    {
                        FirstName = _firstName.Text,
                        LastName = _lastName.Text,
                        Age = ages,
                        Occupation = _occupation.Text
                    });
                    ReturnToMain();
                    break;

                case Resource.Id.im_action_remove:
                    DbHelper.Instance.RemoveUser(_id);

                    ReturnToMain();
                    break;
            }

            return true;
        }

        void ReturnToMain()
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTask);
            StartActivity(intent);

        }
    }
}