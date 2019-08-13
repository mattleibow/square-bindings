namespace Square.OkIO
{
    partial class Options
    {
        public unsafe ByteString GetOption(int i) => (ByteString)Get(i);
    }
}
