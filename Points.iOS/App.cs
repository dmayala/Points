using CoreLocation;
using Microsoft.Practices.Unity;
using Points.Shared.Services;

namespace Points.iOS
{
    public static class App
    {
        public static UnityContainer Container { get; set; }

        public static void Initialize()
        {
            Container = new UnityContainer();
            Container.RegisterInstance<IPlacesService>(Container.Resolve<PlacesService>());
            Container.RegisterInstance(new CLLocationManager());
        }
    }
}
