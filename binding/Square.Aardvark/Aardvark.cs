using System;
using Foundation;
using ObjCRuntime;
using UIKit;

using Square.CoreAardvark;

[assembly: LinkerSafe]

namespace Square.Aardvark
{
	partial class Aardvark
	{
		public static void Log (string format, params object[] args)
		{
			Square.CoreAardvark.CoreAardvark.Log (format, args);
		}

		public static void Log (LogType type, NSDictionary userInfo, string format, params object[] args)
		{
			Square.CoreAardvark.CoreAardvark.Log (type, userInfo, format, args);
		}

		public static void LogScreenshot ()
		{
			AardvarkFunctions.ARKLogScreenshot ();
		}

		public static UIGestureRecognizer AddBugReporter (IBugReporter bugReporter, Type gestureRecognizerType)
		{
			return AddBugReporter (bugReporter, new Class (gestureRecognizerType));
		}

		public static UIGestureRecognizer AddBugReporter<T> (IBugReporter bugReporter)
			where T : UIGestureRecognizer
		{
			return AddBugReporter (bugReporter, typeof(T));
		}
	}
}
