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
    partial class OkBuffer
    {
        IBufferedSink IBufferedSink.EmitCompleteSegments()
        {
            return EmitCompleteSegments();
        }

        IBufferedSink IBufferedSink.Write(byte[] p0)
        {
            return Write(p0);
        }

        IBufferedSink IBufferedSink.Write(byte[] p0, int p1, int p2)
        {
            return Write(p0, p1, p2);
        }

        IBufferedSink IBufferedSink.Write(ByteString p0)
        {
            return Write(p0);
        }

        IBufferedSink IBufferedSink.WriteByte(int p0)
        {
            return WriteByte(p0);
        }

        IBufferedSink IBufferedSink.WriteDecimalLong(long p0)
        {
            return WriteDecimalLong(p0);
        }

        IBufferedSink IBufferedSink.WriteHexadecimalUnsignedLong(long p0)
        {
            return WriteHexadecimalUnsignedLong(p0);
        }

        IBufferedSink IBufferedSink.WriteInt(int p0)
        {
            return WriteInt(p0);
        }

        IBufferedSink IBufferedSink.WriteIntLe(int p0)
        {
            return WriteIntLe(p0);
        }

        IBufferedSink IBufferedSink.WriteLong(long p0)
        {
            return WriteLong(p0);
        }

        IBufferedSink IBufferedSink.WriteLongLe(long p0)
        {
            return WriteLongLe(p0);
        }

        IBufferedSink IBufferedSink.WriteShort(int p0)
        {
            return WriteShort(p0);
        }

        IBufferedSink IBufferedSink.WriteShortLe(int p0)
        {
            return WriteShortLe(p0);
        }

        IBufferedSink IBufferedSink.WriteString(string p0, int p1, int p2, Charset p3)
        {
            return WriteString(p0,p1, p2, p3);
        }

        IBufferedSink IBufferedSink.WriteString(string p0, Charset p1)
        {
            return WriteString(p0,p1);
        }

        IBufferedSink IBufferedSink.WriteUtf8(string p0)
        {
            return WriteUtf8(p0);
        }

        IBufferedSink IBufferedSink.WriteUtf8(string p0, int p1, int p2)
        {
            return WriteUtf8(p0, p1, p2);
        }

        IBufferedSink IBufferedSink.WriteUtf8CodePoint(int p0)
        {
            return WriteUtf8CodePoint(p0);
        }
    }
}