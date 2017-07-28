using System;
using System.Threading.Tasks;
using Foundation;

namespace Square.CoreAardvark
{
	partial class LogDistributor
	{
		public Task DistributeAllPendingLogsAsync ()
		{
			var tcs = new TaskCompletionSource<bool> ();
			DistributeAllPendingLogs (() => tcs.SetResult (true));
			return tcs.Task;
		}

		public void Log (string format, params object[] args)
		{
			Log (string.Format (format, args), IntPtr.Zero);
		}

		public void Log (LogType type, NSDictionary userInfo, string format, params object[] args)
		{
			Log (type, userInfo, string.Format (format, args), IntPtr.Zero);
		}
	}
}
