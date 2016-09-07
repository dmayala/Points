using Points.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Points.Shared.Services
{
    public interface IPlacesService
    {
        Task<IList<Place>> FetchNearbyPlacesAsync(double latitude, double longitude);
        Task<byte[]> FetchByteImageAsync(string url);
        Task<byte[]> FetchPlaceImageAsync(Place place);
    }
}