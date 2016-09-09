using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Points.Droid.Activities;
using Points.Shared.Dtos;
using Points.Shared.Models;

namespace Points.Droid.Holders
{
    public class PlacesHolder : RecyclerView.ViewHolder
    {
        private readonly Context _context;
        private readonly ImageView _imageView;
        private readonly TextView _textView;
        private Place _place;
        private Valuation _valuation;


        public PlacesHolder(View itemView) : base(itemView)
        {
            _imageView = itemView.FindViewById<ImageView>(Resource.Id.PlaceListItemImageView);
            _textView = itemView.FindViewById<TextView>(Resource.Id.PlaceListItemTitleTextView);
            _context = itemView.Context;

            itemView.Click += (sender, e) =>
            {
                var i = PlaceDetailActivity.NewIntent(_context, _place, _valuation);
                _context.StartActivity(i);
            };
        }

        public void BindPlaceAndValuation(Place place, Valuation valuation)
        {
            _place = place;
            _valuation = valuation;
            var cardImage = valuation.Card.Image;
            var cardBitmap = BitmapFactory.DecodeByteArray(cardImage, 0, cardImage.Length);

            _imageView.SetImageBitmap(cardBitmap);
            _textView.Text = place.Name;
        }
    }
}