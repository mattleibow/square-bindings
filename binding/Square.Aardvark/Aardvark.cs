using System;
using Foundation;

namespace Square.Aardvark
{
	partial class Aardvark
	{
		//public static void ARKLog (NSString format, params NSObject[] args)
		//{
		//	if (args == null) {
		//		throw new ArgumentNullException ("args");
		//	}
		//
		//	var pNativeArr = Marshal.AllocHGlobal (args.Length * IntPtr.Size);
		//	for (int i = 0; i < args.Length; ++i) {
		//		Marshal.WriteIntPtr (pNativeArr, i * IntPtr.Size, args [i].Handle);
		//	}
		//
		//	// Null termination
		//	Marshal.WriteIntPtr (pNativeArr, args.Length * IntPtr.Size, IntPtr.Zero);
		//
		//	// the signature for this method has gone from (IntPtr, IntPtr) to (Worker, IntPtr)
		//	AardvarkFunctions.ARKLog (format, pNativeArr);
		//	Marshal.FreeHGlobal (pNativeArr);
		//}

		//public static void ARKLog (LogType type, NSDictionary userInfo, NSString format, params NSObject[] args)
		//{
		//	if (args == null) {
		//		throw new ArgumentNullException ("args");
		//	}
		//
		//	var pNativeArr = Marshal.AllocHGlobal (args.Length * IntPtr.Size);
		//	for (int i = 0; i < args.Length; ++i) {
		//		Marshal.WriteIntPtr (pNativeArr, i * IntPtr.Size, args [i].Handle);
		//	}
		//
		//	// Null termination
		//	Marshal.WriteIntPtr (pNativeArr, args.Length * IntPtr.Size, IntPtr.Zero);
		//
		//	// the signature for this method has gone from (IntPtr, IntPtr) to (Worker, IntPtr)
		//	AardvarkFunctions.ARKLogWithType (type, userInfo, format, pNativeArr);
		//	Marshal.FreeHGlobal (pNativeArr);
		//}

		public static void Log (string format, params object[] args)
		{
			var formatted = string.Format (format, args);
			using (var ns = new NSString (formatted)) {
				AardvarkFunctions.ARKLog (ns.Handle, IntPtr.Zero);
			}
		}

		public static void Log (LogType type, NSDictionary userInfo, string format, params object[] args)
		{
			var formatted = string.Format (format, args);
			using (var ns = new NSString (formatted)) {
				AardvarkFunctions.ARKLogWithType (type, userInfo == null ? IntPtr.Zero : userInfo.Handle, ns.Handle, IntPtr.Zero);
			}
		}

		public static void LogScreenshot ()
		{
			AardvarkFunctions.ARKLogScreenshot ();
		}
	}
}
