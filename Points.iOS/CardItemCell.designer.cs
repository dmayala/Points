// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Points.iOS
{
    [Register ("CardItemCell")]
    partial class CardItemCell
    {
        [Outlet]
        public UIKit.UIImageView ImageLabel { get; set; }

        [Outlet]
        public UIKit.UILabel NameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ImageLabel != null) {
                ImageLabel.Dispose ();
                ImageLabel = null;
            }

            if (NameLabel != null) {
                NameLabel.Dispose ();
                NameLabel = null;
            }
        }
    }
}