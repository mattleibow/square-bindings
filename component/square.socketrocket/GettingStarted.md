# Getting Started with SocketRocket Web Sockets

> A conforming WebSocket (RFC 6455) client library.

## Connecting

We can connect to a web socket using the `WebSocket` type:

    // Create request for remote resource
	NSUrl url = new NSUrl("ws://echo.websocket.org/");
    WebSocket webSocket = new WebSocket(url);

## Listening

We can use an event-based listener:

    webSocket.WebSocketOpened += (sender, e) => {
		// the socket was opened, so we can start using it
	};
	webSocket.Open();
	
Or, we can implement a delegate:

    // attach the delegate and open the connection
    webSocket.Delegate = new SocketDelegate();
	webSocket.Open();
	
	// the delegate type
	class SocketDelegate : WebSocketDelegate
	{
		public override void WebSocketOpened(WebSocket webSocket)
		{
			// the socket was opened
		}
		public override void WebSocketClosed(WebSocket webSocket, StatusCode code, string reason, bool wasClean)
		{
			// the connection was closed
		}
		public override void WebSocketFailed(WebSocket webSocket, NSError error)
		{
			// there was an error
		}
		public override void ReceivedMessage(WebSocket webSocket, NSObject message)
		{
			// we received a message
		}
		public override void ReceivedPong(WebSocket webSocket, NSData pongPayload)
		{
			// respond to a ping
		}
	}
	
## Communicating

Using either the event-based listener or a custom implementation, we can access the open socket and start sending messages:

	webSocket.WebSocketOpened += (sender, e) => {
	    webSocket.Send ((NSString)"Hello World!");
	};

When a message comes in from a remote source, we can handle it using either the event-based listener or a custom implementation:

	webSocket.ReceivedMessage += (sender, e) => {
	    // read the contents
	    string payload = e.Message.ToString();
	};
