using System.Threading.Tasks;
using Foundation;

namespace Square.CoreAardvark
{
	partial class LogStore
	{
		public Task<NSArray> RetrieveAllLogMessagesAsync ()
		{
			var tcs = new TaskCompletionSource<NSArray> ();
			RetrieveAllLogMessages (logMessages => tcs.SetResult (logMessages));
			return tcs.Task;
		}

		public Task ClearLogsAsync ()
		{
			var tcs = new TaskCompletionSource<bool> ();
			ClearLogs (() => tcs.SetResult (true));
			return tcs.Task;
		}
	}
}
