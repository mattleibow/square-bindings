using Java.Nio.Charset;

namespace Square.OkIO
{
    partial class OkBuffer
    {
        IBufferedSink IBufferedSink.Emit() => Emit();

        IBufferedSink IBufferedSink.EmitCompleteSegments() => EmitCompleteSegments();

        IBufferedSink IBufferedSink.Write(byte[] p0) => Write(p0);

        IBufferedSink IBufferedSink.Write(byte[] p0, int p1, int p2) => Write(p0, p1, p2);

        IBufferedSink IBufferedSink.Write(ByteString p0) => Write(p0);

        IBufferedSink IBufferedSink.WriteByte(int p0) => WriteByte(p0);

        IBufferedSink IBufferedSink.WriteDecimalLong(long p0) => WriteDecimalLong(p0);

        IBufferedSink IBufferedSink.WriteHexadecimalUnsignedLong(long p0) => WriteHexadecimalUnsignedLong(p0);

        IBufferedSink IBufferedSink.WriteInt(int p0) => WriteInt(p0);

        IBufferedSink IBufferedSink.WriteIntLe(int p0) => WriteIntLe(p0);

        IBufferedSink IBufferedSink.WriteLong(long p0) => WriteLong(p0);

        IBufferedSink IBufferedSink.WriteLongLe(long p0) => WriteLongLe(p0);

        IBufferedSink IBufferedSink.WriteShort(int p0) => WriteShort(p0);

        IBufferedSink IBufferedSink.WriteShortLe(int p0) => WriteShortLe(p0);

        IBufferedSink IBufferedSink.WriteString(string p0, int p1, int p2, Charset p3) => WriteString(p0,p1, p2, p3);

        IBufferedSink IBufferedSink.WriteString(string p0, Charset p1) => WriteString(p0,p1);

        IBufferedSink IBufferedSink.WriteUtf8(string p0) => WriteUtf8(p0);

        IBufferedSink IBufferedSink.WriteUtf8(string p0, int p1, int p2) => WriteUtf8(p0, p1, p2);

        IBufferedSink IBufferedSink.WriteUtf8CodePoint(int p0) => WriteUtf8CodePoint(p0);
    }
}
