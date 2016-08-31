using CoreLocation;
using Foundation;
using MapKit;
using Microsoft.Practices.Unity;
using Points.Shared.Models;
using Points.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Points.Shared.Dtos;
using Points.Shared.Extensions;
using UIKit;

namespace Points.iOS
{
    public partial class PlacesViewController : UIViewController, IMKMapViewDelegate, IUITableViewDelegate,
        IUITableViewDataSource
    {
        private IList<Place> _places;
        private IList<Card> _bestCards;

        private readonly IPlacesService _placesService;
        private readonly IPointsService _pointsService;
        private MKUserLocation _currentLocation;

        public PlacesViewController(IntPtr handle) : base(handle)
        {
            _placesService = App.Container.Resolve<IPlacesService>();
            _pointsService = App.Container.Resolve<IPointsService>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            MapView.Delegate = this;
            MapView.ShowsUserLocation = true;

            TableView.Delegate = this;
            TableView.DataSource = this;

            TableView.RowHeight = 65;
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
            var cell = tableView.DequeueReusableCell("CardItemCell", indexPath) as CardItemCell;

            if (cell == null) return null;

            // Set the text on cell 
            var item = _places[indexPath.Row];
            var bestCard = _bestCards[indexPath.Row];
            cell.NameLabel.Text = item.Name;
            cell.ImageLabel.Image = new UIImage(NSData.FromArray(bestCard.Image));

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
            var cardTasks = _places.Select(async (p) => await _pointsService.FetchBestCardForCategoriesAsync(p.Types, true));
            _bestCards = await Task.WhenAll(cardTasks);

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
            }).ToArray<IMKAnnotation>();

            MapView.AddAnnotations(annotations);
        }
    }
}