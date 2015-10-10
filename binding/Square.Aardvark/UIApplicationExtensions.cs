using System;
using ObjCRuntime;
using UIKit;

namespace Square.Aardvark
{
	partial class UIApplicationExtensions
	{
		public static UIGestureRecognizer AddBugReporter (this UIApplication application, IBugReporter bugReporter, Type gestureRecognizerType)
		{
			return application.AddBugReporter (bugReporter, new Class (gestureRecognizerType));
		}

		public static UIGestureRecognizer AddBugReporter<T> (this UIApplication application, IBugReporter bugReporter)
			where T : UIGestureRecognizer
		{
			return application.AddBugReporter (bugReporter, typeof(T));
		}
	}
}

