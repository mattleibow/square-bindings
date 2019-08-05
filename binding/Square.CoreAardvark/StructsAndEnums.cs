using System;
using System.Runtime.InteropServices;

namespace Square.CoreAardvark
{
	public enum LogType
	{
		Default,
		Separator,
		Error,
		Screenshot
	}

	internal static class CoreAardvarkFunctions
	{
		// extern void ARKLog (NSString * _Nonnull format, ...) __attribute__((format(NSString, 1, 2)));
		[DllImport ("__Internal")]
		internal static extern void ARKLog (IntPtr format, IntPtr varArgs);

		// extern void ARKLogWithType (ARKLogType type, NSDictionary * _Nullable userInfo, NSString * _Nonnull format, ...) __attribute__((format(NSString, 3, 4)));
		[DllImport ("__Internal")]
		internal static extern void ARKLogWithType (LogType type, IntPtr userInfo, IntPtr format, IntPtr varArgs);

		// extern void ARKEnableLogOnUncaughtException(void);
		[DllImport ("__Internal")]
		internal static extern void ARKEnableLogOnUncaughtException ();

		// extern void ARKEnableLogOnUncaughtExceptionToLogDistributor(ARKLogDistributor *_Nonnull logDistributor);
		[DllImport ("__Internal")]
		internal static extern void ARKEnableLogOnUncaughtExceptionToLogDistributor (IntPtr logDistributor);

		// extern void ARKDisableLogOnUncaughtException(void);
		[DllImport ("__Internal")]
		internal static extern void ARKDisableLogOnUncaughtException ();

		// extern void ARKDisableLogOnUncaughtExceptionToLogDistributor(ARKLogDistributor *_Nonnull logDistributor);
		[DllImport ("__Internal")]
		internal static extern void ARKDisableLogOnUncaughtExceptionToLogDistributor (IntPtr logDistributor);
	}
}
