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
        private IList<Valuation> _bestValuations;

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

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            // If the triggered segue is the "ShowPlace" segue
            if (segue.Identifier == "ShowPlace")
            {
                // Figure out which row was just tapped
                var row = TableView.IndexPathForSelectedRow.Row;

                // Get the place and card associated with this row and pass it along
                var place = _places[row];
                var bestValuation = _bestValuations.Where(v => place.Types.Contains(v.Category.GetSerializationName()))
                    .OrderByDescending(v => v.Points)
                    .First();
                var detailViewController = segue.DestinationViewController as PlaceDetailViewController;
                if (detailViewController != null)
                {
                    detailViewController.Place = place;
                    detailViewController.Valuation = bestValuation;
                }
            }
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
            var bestValuation = _bestValuations.Where(v => item.Types.Contains(v.Category.GetSerializationName()))
                .OrderByDescending(v => v.Points)
                .First();
            cell.NameLabel.Text = item.Name;
            cell.ImageLabel.Image = new UIImage(NSData.FromArray(bestValuation.Card.Image));

            return cell;
        }

        [Export("mapView:didUpdateUserLocation:")]
        public async void DidUpdateUserLocation(MKMapView mapView, MKUserLocation userLocation)
        {
            _currentLocation = userLocation;
            var coordinates = _currentLocation.Coordinate;
            var region = MKCoordinateRegion.FromDistance(coordinates, 1500, 1500);
            mapView.SetRegion(region, animated: true);
            await GetCardsAndPlaces(coordinates);

            TableView.ReloadData();
            AddPlaceAnnotations();
        }

        private async Task GetCardsAndPlaces(CLLocationCoordinate2D coordinates)
        {
            _places = await _placesService.FetchNearbyPlacesAsync(coordinates.Latitude, coordinates.Longitude);
            var placeTypes = _places.SelectMany(p => p.Types).Distinct().ToArray();
            _bestValuations = (await _pointsService.FetchBestValuationForCategoriesAsync(placeTypes)).ToList();
            await _pointsService.FetchCardImagesAsync(_bestValuations.Select(c => c.Card));
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