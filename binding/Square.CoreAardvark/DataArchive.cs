using System.Threading.Tasks;
using Foundation;

namespace Square.CoreAardvark
{
	partial class DataArchive
	{
		public Task<NSArray> ReadObjectsAsync()
		{
			var tcs = new TaskCompletionSource<NSArray>();
			ReadObjects(unarchivedObjects => tcs.SetResult(unarchivedObjects));
			return tcs.Task;
		}

		public Task ClearArchiveAsync()
		{
			var tcs = new TaskCompletionSource<bool>();
			ClearArchive(() => tcs.SetResult(true));
			return tcs.Task;
		}
	}
}
