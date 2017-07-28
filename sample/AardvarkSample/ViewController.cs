using System;
using Foundation;
using UIKit;

using Square.CoreAardvark;
using Square.Aardvark;

namespace AardvarkSample
{
	public partial class ViewController : UIViewController
	{
		private const string SampleTapLogKey = "SampleTapLog";

		private LogStore tapGestureLogStore;
		private UITapGestureRecognizer tapRecognizer;

		public ViewController (IntPtr handle)
			: base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			tapGestureLogStore = new LogStore ("SampleTapLogs.data");
			tapGestureLogStore.Name = "Taps";

			// Log screenshots
			var item = new UIBarButtonItem (UIBarButtonSystemItem.Camera);
			item.Clicked += delegate {
				Aardvark.LogScreenshot ();
			};
			NavigationItem.RightBarButtonItem = item;

			// Ensure that the tap log store will only store tap logs.
			tapGestureLogStore.LogFilterBlock = logMessage => {
				if (logMessage?.UserInfo == null)
					return false;

				var val = (NSNumber)logMessage.UserInfo [SampleTapLogKey];
				return val?.BoolValue == true;
			};

			// Do not log tap logs to the main tap log store.
			LogDistributor.DefaultDistributor.DefaultLogStore.LogFilterBlock = logMessage => {
				if (logMessage?.UserInfo == null)
					return true;
				
				var val = (NSNumber)logMessage.UserInfo [SampleTapLogKey];
				return val?.BoolValue != true;
			};

			LogDistributor.DefaultDistributor.AddLogObserver (tapGestureLogStore);

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			appDelegate.BugReporter.AddLogStores (new [] { tapGestureLogStore });
		}

		public override void ViewDidAppear (bool animated)
		{
			Aardvark.Log ("ViewDidAppear");

			tapRecognizer = new UITapGestureRecognizer (() => {
				if (tapRecognizer.State == UIGestureRecognizerState.Ended) {
					var userData = NSDictionary.FromObjectAndKey (NSNumber.FromBoolean (true), (NSString)SampleTapLogKey);
					Aardvark.Log (LogType.Default, userData, "Tapped {0}", tapRecognizer.LocationInView (null));
				}
			});
			tapRecognizer.CancelsTouchesInView = false;
			View.AddGestureRecognizer (tapRecognizer);

			base.ViewDidAppear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			Aardvark.Log ("ViewDidDisappear");

			tapRecognizer.View.RemoveGestureRecognizer (tapRecognizer);
			tapRecognizer = null;

			base.ViewDidDisappear (animated);
		}

		partial void OnViewAardvarkLogs (UIButton sender)
		{
			Aardvark.Log ("OnViewAardvarkLogs");

			var defaultLogsViewController = new LogTableViewController ();
			NavigationController.PushViewController (defaultLogsViewController, true);
		}

		partial void OnViewTapLogs (UIButton sender)
		{
			Aardvark.Log ("OnViewTapLogs");

			var tapLogsViewController = new LogTableViewController (tapGestureLogStore, new DefaultLogFormatter ());
			NavigationController.PushViewController (tapLogsViewController, true);
		}

		partial void OnBlueButtonPressed (UIButton sender)
		{
			Aardvark.Log ("Blue");
		}

		partial void OnRedButtonPressed (UIButton sender)
		{
			Aardvark.Log ("Red");
		}

		partial void OnGreenButtonPressed (UIButton sender)
		{
			Aardvark.Log ("Green");
		}

		partial void OnYellowButtonPressed (UIButton sender)
		{
			Aardvark.Log ("Yellow");
		}
	}
}
