using AppKit;
using Foundation;

namespace SocketRocketSample_OSX
{
	public partial class AppDelegate : NSApplicationDelegate
	{
		MainWindowController mainWindowController;

		public AppDelegate ()
		{
		}

		public override void DidFinishLaunching (NSNotification notification)
		{
			mainWindowController = new MainWindowController ();
			mainWindowController.Window.MakeKeyAndOrderFront (this);
		}

		public override void WillTerminate (NSNotification notification)
		{
			// Insert code here to tear down your application
		}
	}
}
