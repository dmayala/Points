using Foundation;
using UIKit;

namespace Points.iOS.Extensions
{
    public static class ByteExtensions
    {
        public static UIImage ToUIImage(this byte[] buffer)
        {
            return new UIImage(NSData.FromArray(buffer));
        }
    }
}
