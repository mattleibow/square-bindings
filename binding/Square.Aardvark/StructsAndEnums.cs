using System;
using System.Runtime.InteropServices;

namespace Square.Aardvark
{
	internal static class AardvarkFunctions
	{
		// extern void ARKLogScreenshot ();
		[DllImport ("__Internal")]
		internal static extern void ARKLogScreenshot ();
	}
}
