using UIKit;

namespace Points.iOS.Extensions
{
    public static class NavagationBarExtensions
    {
        public static void MakeNavBarTransparent(this UINavigationBar navBar)
        {
            navBar.Translucent = true;
            navBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            navBar.ShadowImage = new UIImage();
            navBar.BackgroundColor = UIColor.Clear;
            navBar.TintColor = UIColor.White;
        }
    }
}