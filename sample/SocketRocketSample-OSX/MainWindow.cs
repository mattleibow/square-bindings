using System;

using Foundation;
using AppKit;

namespace SocketRocketSample_OSX
{
	public partial class MainWindow : NSWindow
	{
		public MainWindow (IntPtr handle)
			: base (handle)
		{
		}

		[Export ("initWithCoder:")]
		public MainWindow (NSCoder coder)
			: base (coder)
		{
		}
	}
}
