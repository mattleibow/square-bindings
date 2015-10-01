// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SocketRocketSample.OSX
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton clearButton { get; set; }

		[Outlet]
		AppKit.NSButton connectButton { get; set; }

		[Outlet]
		AppKit.NSProgressIndicator connectionActivity { get; set; }

		[Outlet]
		AppKit.NSTextField locationTextField { get; set; }

		[Outlet]
		AppKit.NSTextView logTextField { get; set; }

		[Outlet]
		AppKit.NSTextField messageTextField { get; set; }

		[Outlet]
		AppKit.NSButton pingButton { get; set; }

		[Outlet]
		AppKit.NSButton secureSwitch { get; set; }

		[Outlet]
		AppKit.NSButton sendButton { get; set; }

		[Action ("ClearButtonClicked:")]
		partial void ClearButtonClicked (AppKit.NSButton sender);

		[Action ("ConnectButtonClicked:")]
		partial void ConnectButtonClicked (AppKit.NSButton sender);

		[Action ("PingButtonClicked:")]
		partial void PingButtonClicked (AppKit.NSButton sender);

		[Action ("SecureSwitchChanged:")]
		partial void SecureSwitchChanged (AppKit.NSButton sender);

		[Action ("SendButtonClicked:")]
		partial void SendButtonClicked (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (pingButton != null) {
				pingButton.Dispose ();
				pingButton = null;
			}

			if (clearButton != null) {
				clearButton.Dispose ();
				clearButton = null;
			}

			if (sendButton != null) {
				sendButton.Dispose ();
				sendButton = null;
			}

			if (connectButton != null) {
				connectButton.Dispose ();
				connectButton = null;
			}

			if (locationTextField != null) {
				locationTextField.Dispose ();
				locationTextField = null;
			}

			if (messageTextField != null) {
				messageTextField.Dispose ();
				messageTextField = null;
			}

			if (secureSwitch != null) {
				secureSwitch.Dispose ();
				secureSwitch = null;
			}

			if (logTextField != null) {
				logTextField.Dispose ();
				logTextField = null;
			}

			if (connectionActivity != null) {
				connectionActivity.Dispose ();
				connectionActivity = null;
			}
		}
	}
}
