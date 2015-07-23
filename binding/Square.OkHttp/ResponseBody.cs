using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.Runtime;

namespace Square.OkHttp
{
	partial class ResponseBody
	{
		public virtual Task<byte[]> BytesAsync()
		{
			return Task.Run(() => Bytes());
		}

		public virtual Task<string> StringAsync()
		{
			return Task.Run(() => String());
		}
	}
}
