using Microsoft.Practices.Unity;
using Points.iOS.Extensions;
using Points.Shared.Dtos;
using Points.Shared.Models;
using Points.Shared.Services;
using System;
using UIKit;

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
                PlaceImage.Image = placeByteImage.ToUIImage();
            }

            if (categoryIcon != null)
            {
                CategoryImage.Image = categoryIcon.ToUIImage();
            }

            TitleLabel.Text = Place.Name;
            ImageLabel.Image = Valuation.Card.Image.ToUIImage();
            ReasonLabel.Text = Valuation.Reason;
        }
    }
}