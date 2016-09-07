using System;
using Foundation;
using Points.iOS.Extensions;
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

            NavigationController.NavigationBar.MakeNavBarTransparent();

            var placeByteImage = await _placesService.FetchPlaceImageAsync(Place);
            var categoryIcon = await _placesService.FetchByteImageAsync(Place.Icon);

            if (placeByteImage != null)
            {
                PlaceImage.Image = new UIImage(NSData.FromArray(placeByteImage));
            }

            if (categoryIcon != null)
            {
                CategoryImage.Image = new UIImage(NSData.FromArray(categoryIcon));
            }

            TitleLabel.Text = Place.Name;
            ImageLabel.Image = new UIImage(NSData.FromArray(Valuation.Card.Image));
            ReasonLabel.Text = Valuation.Reason;
        }
    }
}