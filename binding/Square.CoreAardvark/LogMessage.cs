using System;
using Foundation;

namespace Square.CoreAardvark
{
	partial class LogMessage
	{
		[Obsolete ("Use Date instead.")]
		public NSDate CreationDate => Date;
	}
}
