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
using Android.Widget;
using Com.Lilarcor.Cheeseknife;
using Microsoft.Practices.Unity;
using Points.Shared.Dtos;
using Points.Shared.Services;
using Location = Android.Locations.Location;

namespace Points.Droid.Fragments
{
    public class PlacesFragment : Fragment, IOnMapReadyCallback, ILocationListener
    {
        private const int RequestLocationId = 0;
        private Location _currentLocation;
        private LocationManager _locationManager;
        private GoogleMap _map;

        private IPlacesService _placesService;

        private SupportMapFragment _mapFragment;
        [InjectView(Resource.Id.PlacesRecyclerViewFragment)]
        private RecyclerView _recyclerView;

        public static PlacesFragment NewInstance()
        {
            return new PlacesFragment();
        }

        #region Life Cycle

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _locationManager = Activity.GetSystemService(Context.LocationService) as LocationManager;
            _placesService = App.Container.Resolve<IPlacesService>();
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

        #endregion Methods

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
            _recyclerView.SetAdapter(new PlacesAdapter(places));
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

    public class PlacesHolder : RecyclerView.ViewHolder
    {
        private readonly Context _context;
        private readonly TextView _nameTextView;

        public PlacesHolder(View itemView) : base(itemView)
        {
            _nameTextView = (TextView) itemView;
            _context = itemView.Context;
        }

        public void BindPlace(Place place)
        {
            _nameTextView.Text = place.Name;
        }
    }

    public class PlacesAdapter : RecyclerView.Adapter
    {
        private readonly IList<Place> _places;

        public PlacesAdapter(IList<Place> places)
        {
            _places = places;
        }

        public override int ItemCount => _places.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var placesHolder = holder as PlacesHolder;
            var place = _places[position];
            placesHolder?.BindPlace(place);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var layoutInflater = LayoutInflater.From(parent.Context);
            var view = layoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, parent, false);
            return new PlacesHolder(view);
        }
    }
}