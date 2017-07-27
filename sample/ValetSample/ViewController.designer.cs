// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ValetSample
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView textView { get; set; }

        [Action ("OnGetItem:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnGetItem (UIKit.UIButton sender);

        [Action ("OnRemoveItem:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnRemoveItem (UIKit.UIButton sender);

        [Action ("OnRequirePrompt:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnRequirePrompt (UIKit.UIButton sender);

        [Action ("OnSetItem:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OnSetItem (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (textView != null) {
                textView.Dispose ();
                textView = null;
            }
        }
    }
}