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

namespace SocketRocketSample_TVOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton clearButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton connectButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField locationTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView logTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField messageTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton pingButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl secureSwitch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton sendButton { get; set; }

        [Action ("ClearButtonClicked:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ClearButtonClicked (UIKit.UIButton sender);

        [Action ("ConnectButtonClicked:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ConnectButtonClicked (UIKit.UIButton sender);

        [Action ("PingButtonClicked:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PingButtonClicked (UIKit.UIButton sender);

        [Action ("SecureSwitchChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SecureSwitchChanged (UIKit.UISegmentedControl sender);

        [Action ("SendButtonClicked:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SendButtonClicked (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (clearButton != null) {
                clearButton.Dispose ();
                clearButton = null;
            }

            if (connectButton != null) {
                connectButton.Dispose ();
                connectButton = null;
            }

            if (locationTextField != null) {
                locationTextField.Dispose ();
                locationTextField = null;
            }

            if (logTextField != null) {
                logTextField.Dispose ();
                logTextField = null;
            }

            if (messageTextField != null) {
                messageTextField.Dispose ();
                messageTextField = null;
            }

            if (pingButton != null) {
                pingButton.Dispose ();
                pingButton = null;
            }

            if (secureSwitch != null) {
                secureSwitch.Dispose ();
                secureSwitch = null;
            }

            if (sendButton != null) {
                sendButton.Dispose ();
                sendButton = null;
            }
        }
    }
}