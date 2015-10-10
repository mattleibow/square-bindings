using Foundation;
using UIKit;

using Square.Aardvark;

namespace AardvarkSample
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public override UIWindow Window { get; set; }

		public EmailBugReporter BugReporter { get; private set; }

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			// This line is all you'll need to get started.
			BugReporter = Aardvark.AddDefaultBugReportingGestureWithEmailBugReporterWithRecipient ("fake-email@aardvarkbugreporting.src");

			// Log all ARKLog messages to Crashlytics to help debug crashes.
			LogDistributor.DefaultDistributor.AddLogObserver (new ConsoleLogObserver ());

			Aardvark.Log (LogType.Separator, null, @"Hello World");

			return true;
		}

		public override void WillEnterForeground (UIApplication application)
		{
			Aardvark.Log ("Application Will Enter Foreground");
		}

		public override void DidEnterBackground (UIApplication application)
		{
			Aardvark.Log ("Application Did Enter Background");
		}

		public override void WillTerminate (UIApplication application)
		{
			Aardvark.Log (LogType.Error, null, "Exiting Sample App");
		}
	}
}
