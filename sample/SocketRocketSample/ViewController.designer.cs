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

namespace SocketRocketSample
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton clearButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton connectButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIActivityIndicatorView connectionActivity { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField locationTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView logTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField messageTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton pingButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch secureSwitch { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton sendButton { get; set; }

		[Action ("ClearButtonClicked:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ClearButtonClicked (UIButton sender);

		[Action ("ConnectButtonClicked:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ConnectButtonClicked (UIButton sender);

		[Action ("PingButtonClicked:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void PingButtonClicked (UIButton sender);

		[Action ("SecureSwitchChanged:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void SecureSwitchChanged (UISwitch sender);

		[Action ("SendButtonClicked:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void SendButtonClicked (UIButton sender);

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
			if (connectionActivity != null) {
				connectionActivity.Dispose ();
				connectionActivity = null;
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
