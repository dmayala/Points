using Android.App;
using Android.Content;
using Points.Droid.Fragments;

namespace Points.Droid.Activities
{
    [Activity(MainLauncher = true, Icon = "@drawable/icon")]
    public class PlacesActivity : SingleFragmentActivity
    {
        public static Intent NewIntent(Context context)
        {
            return new Intent(context, typeof(PlacesActivity));
        }

        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return PlacesFragment.NewInstance();
        }
    }
}