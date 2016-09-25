using Android.App;
using Android.Content.PM;
using Points.Droid.Fragments;

namespace Points.Droid.Activities
{
    [Activity(MainLauncher = true, Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleTop)]
    public class PlacesActivity : SingleFragmentActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return new PlacesFragment();
        }
    }
}