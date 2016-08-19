using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace Points.Droid.Fragments
{
    public class PlacesFragment : Fragment
    {
        public static PlacesFragment NewInstance()
        {
            return new PlacesFragment();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_places, container, false);
        }
    }
}