﻿using System;
using Foundation;
using UIKit;

using Square.SocketRocket;

namespace SocketRocketSample_TVOS
{
	public partial class ViewController : UIViewController
	{
		private WebSocket webSocket;

		public ViewController (IntPtr handle) 
			: base (handle)
		{
		}

		partial void ClearButtonClicked (UIButton sender)
		{
			logTextField.Text = string.Empty;
		}

		partial void ConnectButtonClicked (UIButton sender)
		{
			var url = locationTextField.Text;

			if (webSocket == null || webSocket.ReadyState == ReadyState.Closed) {
				webSocket = new WebSocket (new NSUrl (url));
				webSocket.ReceivedMessage += (_, e) => {
					logTextField.Text += string.Format ("Received message: '{1}'{0}", Environment.NewLine, e.Message);
				};
				webSocket.ReceivedPong += (_, e) => {
					logTextField.Text += string.Format ("Received pong.{0}", Environment.NewLine);
				};
				webSocket.WebSocketClosed += (_, e) => {
					logTextField.Text += string.Format ("Disconnected: '{1}' ({2}).{0}", Environment.NewLine, e.Reason, e.Code);
					UpdateUI (webSocket.ReadyState);
				};
				webSocket.WebSocketFailed += (_, e) => {
					logTextField.Text += string.Format ("Failed to connect: {1}.{0}", Environment.NewLine, e.Error);
					UpdateUI (webSocket.ReadyState);
				};
				webSocket.WebSocketOpened += (_, e) => {
					logTextField.Text += string.Format ("Connected to '{1}'.{0}", Environment.NewLine, url);
					UpdateUI (webSocket.ReadyState);
				};
				logTextField.Text += string.Format ("Connecting to '{1}'...{0}", Environment.NewLine, url);
				webSocket.Open ();
			} else if (webSocket.ReadyState == ReadyState.Open) {
				logTextField.Text += string.Format ("Disconnecting...{0}", Environment.NewLine);
				webSocket.Close ();
			}
			UpdateUI (webSocket.ReadyState);
		}

		partial void PingButtonClicked (UIButton sender)
		{
			if (webSocket != null && webSocket.ReadyState == ReadyState.Open) {
				logTextField.Text += string.Format ("Pinging...{0}", Environment.NewLine);
				webSocket.SendPing ();
			}
		}

		partial void SecureSwitchChanged (UISegmentedControl sender)
		{
			var uri = new UriBuilder (locationTextField.Text);
			if (sender.SelectedSegment == 1) {
				uri.Scheme = "wss";
			} else {
				uri.Scheme = "ws";
			}
			locationTextField.Text = uri.ToString ();
		}

		partial void SendButtonClicked (UIButton sender)
		{
			if (webSocket != null && webSocket.ReadyState == ReadyState.Open) {
				var msg = messageTextField.Text;
				logTextField.Text += string.Format ("Sending message '{1}'...{0}", Environment.NewLine, msg);
				webSocket.Send ((NSString)msg);
			}
		}

		private void UpdateUI (ReadyState state)
		{
			messageTextField.Enabled = state == ReadyState.Open;
			sendButton.Enabled = state == ReadyState.Open;
			pingButton.Enabled = state == ReadyState.Open;

			var working = state == ReadyState.Connecting || state == ReadyState.Closing;
			connectButton.Enabled = !working;

			if (state == ReadyState.Open) {
				connectButton.SetTitle ("Disconnect", UIControlState.Normal);
			} else if (state == ReadyState.Closed) {
				connectButton.SetTitle ("Connect", UIControlState.Normal);
			}
		}
	}
}
