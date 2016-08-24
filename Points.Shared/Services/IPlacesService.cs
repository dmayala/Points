using Points.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Points.Shared.Services
{
    public interface IPlacesService
    {
        Task<IList<Place>> FetchNearbyPlacesAsync(double latitude, double longitude);
    }
}