using System;
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
		[Obsolete]
		CodeNormal = 1000L,
		Normal = 1000L,

		[Obsolete]
		CodeGoingAway = 1001L,
		GoingAway = 1001L,

		[Obsolete]
		CodeProtocolError = 1002L,
		ProtocolError = 1002L,

		[Obsolete]
		CodeUnhandledType = 1003L,
		UnhandledType = 1003L,

		NoStatusReceived = 1005L,

		CodeAbnormal = 1006L,

		[Obsolete]
		CodeInvalidUTF8 = 1007L,
		InvalidUTF8 = 1007L,

		[Obsolete]
		CodePolicyViolated = 1008L,
		PolicyViolated = 1008L,

		[Obsolete]
		CodeMessageTooBig = 1009L,
		MessageTooBig = 1009L,
		
		MissingExtension = 1010L,

		InternalError = 1011L,

		ServiceRestart = 1012L,

		TryAgainLater = 1013L,

		// 1014: Reserved for future use by the WebSocket standard.

		TLSHandshake = 1015L,

		// 1016–1999: Reserved for future use by the WebSocket standard.

		// 2000–2999: Reserved for use by WebSocket extensions.

		// 3000–3999: Available for use by libraries and frameworks. May not be used by applications. Available for registration at the IANA via first-come, first-serve.

		// 4000–4999: Available for use by applications.
	}
}
