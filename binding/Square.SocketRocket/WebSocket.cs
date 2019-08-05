using System;
using Foundation;

[assembly: LinkerSafe]

namespace Square.SocketRocket
{
	partial class WebSocket
	{
		public void SendPing ()
		{
			SendPing (null);
		}
	}
}

