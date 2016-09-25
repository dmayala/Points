using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Com.Lilarcor.Cheeseknife;
using Microsoft.Practices.Unity;
using Points.Droid.Utils;
using Points.Shared.Dtos;
using Points.Shared.Models;
using Points.Shared.Services;
using System.Threading.Tasks;

namespace Points.Droid.Fragments
{
    public class PlaceDetailFragment : Fragment
    {
        private IPlacesService _placesService;

        private Place _place;
        private Valuation _valuation;

        [InjectView(Resource.Id.CategoryImageView)]
        private ImageView _categoryImageView;
        [InjectView(Resource.Id.CardImageView)]
        private ImageView _cardImageView;
        [InjectView(Resource.Id.PlaceTextView)]
        private TextView _placeTextView;
        [InjectView(Resource.Id.ReasonTextView)]
        private TextView _reasonTextView;

        public static PlaceDetailFragment NewInstance(IParcelable place, IParcelable valuation)
        {
            var args = new Bundle();
            args.PutParcelable(typeof(Place).Name, place);
            args.PutParcelable(typeof(Valuation).Name, valuation);

            var fragment = new PlaceDetailFragment { Arguments = args };
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _placesService = App.Container.Resolve<IPlacesService>();
            _place = ParcelableUtil.Unwrap<Place>(Arguments.GetParcelable(typeof(Place).Name) as IParcelable);
            _valuation = ParcelableUtil.Unwrap<Valuation>(Arguments.GetParcelable(typeof(Valuation).Name) as IParcelable);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.fragment_place_detail, container, false);
            Cheeseknife.Inject(this, v);

            BindViewModels();

            return v;
        }

        private async Task BindViewModels()
        {
            var iconBytes = await _placesService.FetchByteImageAsync(_place.Icon);
            var categoryBitmap = BitmapFactory.DecodeByteArray(iconBytes, 0, iconBytes.Length);
            _categoryImageView.SetImageBitmap(categoryBitmap);

            var cardBitmap = BitmapFactory.DecodeByteArray(_valuation.Card.Image, 0, _valuation.Card.Image.Length);
            _cardImageView.SetImageBitmap(cardBitmap);

            _placeTextView.Text = _place.Name;
            _reasonTextView.Text = _valuation.Reason;
        }
    }
}