using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Points.Droid.Fragments;
using Points.Droid.Utils;
using Points.Shared.Dtos;
using Points.Shared.Models;
using Fragment = Android.Support.V4.App.Fragment;

namespace Points.Droid.Activities
{
    [Activity(ParentActivity = typeof(PlacesActivity))]
    public class PlaceDetailActivity : SingleFragmentActivity
    {
        public static Intent NewIntent(Context context, Place place, Valuation valuation)
        {
            var i = new Intent(context, typeof(PlaceDetailActivity));
            i.PutExtra(typeof(Place).Name, ParcelableUtil.Wrap(place));
            i.PutExtra(typeof(Valuation).Name, ParcelableUtil.Wrap(valuation));
            return i;
        }
        protected override Fragment CreateFragment()
        {
            var place = Intent.GetParcelableExtra(typeof(Place).Name) as IParcelable;
            var valuation = Intent.GetParcelableExtra(typeof(Valuation).Name) as IParcelable;

            return PlaceDetailFragment.NewInstance(place, valuation);
        }
    }
}