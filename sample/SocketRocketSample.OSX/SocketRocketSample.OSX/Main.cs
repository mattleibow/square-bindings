using AppKit;
using System.IO;
using System;
using System.Reflection;

namespace SocketRocketSample.OSX
{
	static class MainClass
	{
		// http://stackoverflow.com/questions/52797/how-do-i-get-the-path-of-the-assembly-the-code-is-in
		static string GetCurrentExecutingDirectory ()
		{
			string filePath = new Uri (Assembly.GetExecutingAssembly().CodeBase).LocalPath;
			return Path.GetDirectoryName(filePath);
		}

		static string GetSocketRocketDirectory ()
		{
			return Path.Combine(GetCurrentExecutingDirectory(), "../Resources", "SocketRocket");
		}

		static void Main (string[] args)
		{
			ObjCRuntime.Dlfcn.dlopen(GetSocketRocketDirectory(), 0);

			NSApplication.Init();
			NSApplication.Main(args);
		}
	}
}