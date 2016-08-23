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
using Points.Droid.Utils;

namespace Points.Droid.Fragments
{
    public class PlacesFragment : Fragment, IOnMapReadyCallback, ILocationListener
    {
        const int RequestLocationId = 0;

        private SupportMapFragment _mapFragment;
        private GoogleMap _map;
        private LocationManager _locationManager;
        private Location _currentLocation;
        private PlacesApi _placesApi = new PlacesApi();

        public static PlacesFragment NewInstance()
        {
            return new PlacesFragment();
        }

        #region Life Cycle
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _locationManager = Activity.GetSystemService(Context.LocationService) as LocationManager;
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
            _locationManager.RequestLocationUpdates(provider, 1000 * 60, 10, this);
        }

        public async void OnLocationChanged(Location location)
        {
            CenterCamera();
            var places = await _placesApi.FetchNearbyPlacesAsync(location.Latitude, location.Longitude);
            foreach (var place in places)
            {
                var loc = place.Geometry.Location;
                var markerOptions = new MarkerOptions();
                var latLng = new LatLng(loc.Latitude, loc.Longitude);
                markerOptions.SetPosition(latLng);
                markerOptions.SetTitle(place.Name);
                _map.AddMarker(markerOptions);
            }
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
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