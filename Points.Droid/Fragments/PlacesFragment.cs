using Android.Gms.Maps;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Locations;
using Android.Content;
using Com.Lilarcor.Cheeseknife;
using Android.Gms.Maps.Model;
using Android.Support.V4.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Gms.Common.Apis;

namespace Points.Droid.Fragments
{
    public class PlacesFragment : Fragment, IOnMapReadyCallback
    {
        const int RequestLocationId = 0;

        private SupportMapFragment _mapFragment;
        private GoogleMap _map;
        private LocationManager _locationManager;
        private Location _currentLocation;
        private GoogleApiClient _googleApiClient;

        public static PlacesFragment NewInstance()
        {
            return new PlacesFragment();
        }

        #region Life Cycle
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _locationManager = Activity.GetSystemService(Context.LocationService) as LocationManager;
             _googleApiClient = new GoogleApiClient
              .Builder(Context)
              .Build();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.fragment_places, container, false);
            Cheeseknife.Inject(this, v);
            return v;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            _mapFragment = (SupportMapFragment)ChildFragmentManager.FindFragmentById(Resource.Id.map);
            if (_mapFragment == null)
            {
                _mapFragment = SupportMapFragment.NewInstance();
                ChildFragmentManager.BeginTransaction()
                    .Replace(Resource.Id.map, _mapFragment)
                    .Commit();
                GetPermissions();
            }
        }
        #endregion Methods

        #region Permissions
        private void GetPermissions()
        {
            if (ContextCompat.CheckSelfPermission(Activity, Android.Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
            {
                RequestPermissions(new string[]{
                    Android.Manifest.Permission.AccessCoarseLocation,
                    Android.Manifest.Permission.AccessFineLocation}, RequestLocationId);
            }
            else
            {
                _mapFragment.GetMapAsync(this);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            //Permission Granted
                            var snack = Snackbar.Make(View, "Location permission is available, getting lat/long.", Snackbar.LengthShort);
                            snack.Show();
                            _mapFragment.GetMapAsync(this);
                        }
                        else
                        {
                            //Permission Denied :(
                            var snack = Snackbar.Make(View, "Location permission is denied.", Snackbar.LengthShort);
                            snack.Show();
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Map Methods
        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;
            _map.MyLocationEnabled = true;

            var criteria = new Criteria();
            var provider = _locationManager.GetBestProvider(criteria, true);
            _currentLocation = _locationManager.GetLastKnownLocation(provider);
            CenterCamera();
        }

        private void CenterCamera()
        {
            if (_currentLocation != null)
            {
                var target = new LatLng(_currentLocation.Latitude, _currentLocation.Longitude);

                var cameraUpdate = new CameraPosition.Builder()
                    .Zoom(15)
                    .Target(target)
                    .Build();

                _map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraUpdate));
            }
        }
        #endregion
    }
}