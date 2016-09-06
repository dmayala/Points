using System;
using Foundation;
using Points.Shared.Dtos;
using Points.Shared.Models;
using Points.Shared.Services;
using UIKit;
using Microsoft.Practices.Unity;

namespace Points.iOS
{
    public partial class PlaceDetailViewController : UIViewController
    {
        private readonly IPlacesService _placesService;

        public Place Place { get; set; }
        public Valuation Valuation { get; set; }

        public PlaceDetailViewController (IntPtr handle) : base (handle)
        {
            _placesService = App.Container.Resolve<IPlacesService>();
        }

        public override async void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var placeByteImage = await _placesService.FetchPlaceImageAsync(Place);

            if (placeByteImage != null)
            {
                PlaceImage.Image = new UIImage(NSData.FromArray(placeByteImage));
            }

            ImageLabel.Image = new UIImage(NSData.FromArray(Valuation.Card.Image));
            TextLabel.Text = Valuation.Reason;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationItem.Title = Place.Name;
        }
    }
}