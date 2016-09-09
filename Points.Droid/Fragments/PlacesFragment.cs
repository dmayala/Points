using System.Collections.Generic;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Com.Lilarcor.Cheeseknife;
using Microsoft.Practices.Unity;
using Points.Droid.Adapters;
using Points.Shared.Services;
using Location = Android.Locations.Location;
using System.Linq;
using System.Threading.Tasks;
using Points.Shared.Dtos;

namespace Points.Droid.Fragments
{
    public class PlacesFragment : Fragment, IOnMapReadyCallback, ILocationListener
    {
        private const int RequestLocationId = 0;
    
        private Location _currentLocation;
        private LocationManager _locationManager;
        private GoogleMap _map;

        private IPlacesService _placesService;
        private IPointsService _pointsService;

        private SupportMapFragment _mapFragment;
        [InjectView(Resource.Id.PlacesRecyclerViewFragment)]
        private RecyclerView _recyclerView;

        #region Life Cycle Methods

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _locationManager = Activity.GetSystemService(Context.LocationService) as LocationManager;
            _placesService = App.Container.Resolve<IPlacesService>();
            _pointsService = App.Container.Resolve<IPointsService>();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.fragment_places, container, false);
            Cheeseknife.Inject(this, v);
            _recyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            return v;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            _mapFragment = (SupportMapFragment) ChildFragmentManager.FindFragmentById(Resource.Id.map);
            if (_mapFragment == null)
            {
                _mapFragment = SupportMapFragment.NewInstance();
                ChildFragmentManager.BeginTransaction()
                    .Replace(Resource.Id.map, _mapFragment)
                    .Commit();
                GetPermissions();
            }
        }

        #endregion

        #region Permissions

        private void GetPermissions()
        {
            if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.AccessFineLocation) !=
                (int) Permission.Granted)
                RequestPermissions(new[]
                {
                    Manifest.Permission.AccessCoarseLocation,
                    Manifest.Permission.AccessFineLocation
                }, RequestLocationId);
            else
                _mapFragment.GetMapAsync(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                {
                    if (grantResults[0] == Permission.Granted)
                    {
                        //Permission Granted
                        var snack = Snackbar.Make(View, "Location permission is available, getting lat/long.",
                            Snackbar.LengthShort);
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
            _locationManager.RequestLocationUpdates(provider, 1000*60, 10, this);
        }

        public async void OnLocationChanged(Location location)
        {
            CenterCamera();
            var places = await _placesService.FetchNearbyPlacesAsync(location.Latitude, location.Longitude);
            await SetCardsAndPlaces(places);
            AddMarkers(places);
        }

        private void AddMarkers(IEnumerable<Place> places)
        {
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

        private async Task SetCardsAndPlaces(IList<Place> places)
        {
            var placeTypes = places.SelectMany(p => p.Types).Distinct().ToArray();
            var bestValuations = (await _pointsService.FetchBestValuationForCategoriesAsync(placeTypes)).ToList();
            await _pointsService.FetchCardImagesAsync(bestValuations.Select(c => c.Card));
            _recyclerView.SetAdapter(new PlacesAdapter(places, bestValuations));
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