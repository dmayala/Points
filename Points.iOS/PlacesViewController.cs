using CoreLocation;
using Foundation;
using MapKit;
using Microsoft.Practices.Unity;
using Points.Shared.Models;
using Points.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace Points.iOS
{
    public partial class PlacesViewController : UIViewController, IMKMapViewDelegate, IUITableViewDelegate, IUITableViewDataSource
    {
        private IList<Place> _places;
        private IPlacesService _placesService;
        private MKUserLocation _currentLocation;

        public PlacesViewController (IntPtr handle) : base (handle)
        {
            _placesService = App.Container.Resolve<IPlacesService>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            MapView.Delegate = this;
            MapView.ShowsUserLocation = true;

            TableView.Delegate = this;
            TableView.DataSource = this;
        }

        [Export("tableView:numberOfRowsInSection:")]
        public nint RowsInSection(UITableView tableView, nint section)
        {
            return _places?.Count ?? 0;
        }

        [Export("tableView:cellForRowAtIndexPath:")]
        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // Get a new or recycled cel
            var cell = tableView.DequeueReusableCell("UITableViewCell", indexPath);

            // Set the text on cell 
            var item = _places[indexPath.Row];
            cell.TextLabel.Text = item.Name;

            return cell;
        }

        [Export("mapView:didUpdateUserLocation:")]
        public async void DidUpdateUserLocation(MKMapView mapView, MKUserLocation userLocation)
        {
            _currentLocation = userLocation;
            var coordinates = _currentLocation.Coordinate;
            var region = MKCoordinateRegion.FromDistance(coordinates, 1500, 1500);
            mapView.SetRegion(region, animated: true);
            _places = await _placesService.FetchNearbyPlacesAsync(coordinates.Latitude, coordinates.Longitude);
            TableView.ReloadData();
            AddPlaceAnnotations();
        }

        private void AddPlaceAnnotations()
        {
            var annotations = _places.Select(p =>
            {
                var coordinates = p.Geometry.Location;
                return new MKPointAnnotation()
                {
                    Coordinate = new CLLocationCoordinate2D(coordinates.Latitude, coordinates.Longitude)
                };
            }).ToArray();

            MapView.AddAnnotations(annotations);
        }
    }
}