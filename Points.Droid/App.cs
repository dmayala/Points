using Android.App;
using Android.Runtime;
using Microsoft.Practices.Unity;
using Points.Shared.Services;
using System;

namespace Points.Droid
{
    [Application]
    public class App : Application
    {
        public static UnityContainer Container { get; set; }

        public App(IntPtr h, JniHandleOwnership jho) : base(h, jho)
        {
        }

        public override void OnCreate()
        {
            Container = new UnityContainer();
            Container.RegisterInstance<IPlacesService>(Container.Resolve<PlacesService>());

            base.OnCreate();
        }
    }
}