using System;

using Foundation;
using AppKit;
using Square.SocketRocket;

namespace SocketRocketSample_OSX
{
	public partial class MainWindowController : NSWindowController
	{
		private WebSocket webSocket;

		public MainWindowController (IntPtr handle)
			: base (handle)
		{
		}

		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder)
			: base (coder)
		{
		}

		public MainWindowController ()
			: base ("MainWindow")
		{
		}

		public new MainWindow Window {
			get { return (MainWindow)base.Window; }
		}

		partial void SecureSwitchChanged (NSButton sender)
		{
			var uri = new UriBuilder (locationTextField.StringValue);
			if (sender.State == NSCellStateValue.On) {
				uri.Scheme = "wss";
			} else {
				uri.Scheme = "ws";
			}
			locationTextField.StringValue = uri.ToString ();
		}

		partial void ConnectButtonClicked (NSButton sender)
		{
			var url = locationTextField.StringValue;

			if (webSocket == null || webSocket.ReadyState == ReadyState.Closed) {
				webSocket = new WebSocket (new NSUrl (url));
				webSocket.ReceivedMessage += (_, e) => {
					logTextField.Value += string.Format ("Received message: '{1}'{0}", Environment.NewLine, e.Message);
				};
				webSocket.ReceivedPong += (_, e) => {
					logTextField.Value += string.Format ("Received pong.{0}", Environment.NewLine);
				};
				webSocket.WebSocketClosed += (_, e) => {
					logTextField.Value += string.Format ("Disconnected: '{1}' ({2}).{0}", Environment.NewLine, e.Reason, e.Code);
					UpdateUI (webSocket.ReadyState);
				};
				webSocket.WebSocketFailed += (_, e) => {
					logTextField.Value += string.Format ("Failed to connect: {1}.{0}", Environment.NewLine, e.Error);
					UpdateUI (webSocket.ReadyState);
				};
				webSocket.WebSocketOpened += (_, e) => {
					logTextField.Value += string.Format ("Connected to '{1}'.{0}", Environment.NewLine, url);
					UpdateUI (webSocket.ReadyState);
				};
				logTextField.Value += string.Format ("Connecting to '{1}'...{0}", Environment.NewLine, url);
				webSocket.Open ();
			} else if (webSocket.ReadyState == ReadyState.Open) {
				logTextField.Value += string.Format ("Disconnecting...{0}", Environment.NewLine);
				webSocket.Close ();
			}
			UpdateUI (webSocket.ReadyState);
		}

		partial void SendButtonClicked (NSButton sender)
		{
			if (webSocket != null && webSocket.ReadyState == ReadyState.Open) {
				var msg = messageTextField.StringValue;
				logTextField.Value += string.Format ("Sending message '{1}'...{0}", Environment.NewLine, msg);
				webSocket.Send ((NSString)msg);
			}
		}

		partial void PingButtonClicked (NSButton sender)
		{
			if (webSocket != null && webSocket.ReadyState == ReadyState.Open) {
				logTextField.Value += string.Format ("Pinging...{0}", Environment.NewLine);
				webSocket.SendPing ();
			}
		}

		partial void ClearButtonClicked (NSButton sender)
		{
			logTextField.Value = string.Empty;
		}

		private void UpdateUI (ReadyState state)
		{
			messageTextField.Enabled = state == ReadyState.Open;
			sendButton.Enabled = state == ReadyState.Open;
			pingButton.Enabled = state == ReadyState.Open;

			var working = state == ReadyState.Connecting || state == ReadyState.Closing;
			connectButton.Enabled = !working;
			if (working) {
				connectionActivity.StartAnimation (this);
			} else {
				connectionActivity.StopAnimation (this);
			}

			if (state == ReadyState.Open) {
				connectButton.Title = "Disconnect";
			} else if (state == ReadyState.Closed) {
				connectButton.Title = "Connect";
			}
		}
	}
}
