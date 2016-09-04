using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Widget;
using Android.Views;
using Points.Droid.Holders;
using Points.Shared.Dtos;
using Points.Shared.Extensions;
using Points.Shared.Models;

namespace Points.Droid.Adapters
{
    public class PlacesAdapter : RecyclerView.Adapter
    {
        private readonly IList<Place> _places;
        private readonly IEnumerable<Valuation> _valuations;

        public PlacesAdapter(IList<Place> places, IEnumerable<Valuation> valuations)
        {
            _places = places;
            _valuations = valuations;
        }

        public override int ItemCount => _places.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var placesHolder = holder as PlacesHolder;
            var place = _places[position];
            var bestValuation = _valuations.Where(v => place.Types.Contains(v.Category.GetSerializationName()))
                .OrderByDescending(v => v.Points)
                .First();

            placesHolder?.BindPlaceAndValuation(place, bestValuation);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var layoutInflater = LayoutInflater.From(parent.Context);
            var view = layoutInflater.Inflate(Resource.Layout.listitem_place, parent, false);
            return new PlacesHolder(view);
        }
    }
}