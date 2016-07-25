using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Java.Nio.Charset;

namespace Square.OkIO
{
    partial class Options
    {
        public unsafe ByteString GetOption(int i)
        {
            return (ByteString)Get(i);
        }
    }
}