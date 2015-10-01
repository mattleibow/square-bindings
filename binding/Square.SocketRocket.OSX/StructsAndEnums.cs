using ObjCRuntime;

namespace Square.SocketRocket
{
	[Native]
	public enum ReadyState : long
	{
		Connecting = 0L,
		Open = 1L,
		Closing = 2L,
		Closed = 3L
	}

	[Native]
	public enum StatusCode : long
	{
		CodeNormal = 1000L,
		CodeGoingAway = 1001L,
		CodeProtocolError = 1002L,
		CodeUnhandledType = 1003L,
		NoStatusReceived = 1005L,
		CodeInvalidUTF8 = 1007L,
		CodePolicyViolated = 1008L,
		CodeMessageTooBig = 1009L
	}
}