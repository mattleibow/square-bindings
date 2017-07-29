using System;
using Foundation;

using Square.CoreAardvark;
using Square.Aardvark;

namespace AardvarkSample
{
	public class ConsoleLogObserver : NSObject, ILogObserver
	{
		public LogDistributor LogDistributor { get; set; }

		public void Observe (LogMessage logMessage)
		{
			Console.WriteLine ($"[{logMessage.Date}] ConsoleLogObserver: '{logMessage.Text}'");
		}
	}
}
