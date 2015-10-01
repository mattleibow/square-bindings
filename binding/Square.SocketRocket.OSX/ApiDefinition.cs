using System;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace Square.SocketRocket
{
	partial interface WebSocketConstants
	{
		// extern NSString *const SRWebSocketErrorDomain;
		[Field("SRWebSocketErrorDomain", "__Internal")]
		NSString WebSocketErrorDomain { get; }

		// extern NSString *const SRHTTPResponseErrorKey;
		[Field("SRHTTPResponseErrorKey", "__Internal")]
		NSString HTTPResponseErrorKey { get; }
	}

	// @interface SRWebSocket : NSObject <NSStreamDelegate>
	[BaseType(typeof(NSObject), Name = "SRWebSocket", Delegates = new [] { "Delegate" }, Events = new [] { typeof(WebSocketDelegate) })]
	interface WebSocket : INSStreamDelegate
	{
		// @property (nonatomic, weak) id<SRWebSocketDelegate> delegate;
		[NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
		IWebSocketDelegate Delegate { get; set; }

		// @property (readonly, nonatomic) SRReadyState readyState;
		[Export("readyState")]
		ReadyState ReadyState { get; }

		// @property (readonly, retain, nonatomic) NSURL * url;
		[Export("url", ArgumentSemantic.Retain)]
		NSUrl Url { get; }

		// @property (readonly, copy, nonatomic) NSString * protocol;
		[Export("protocol")]
		string Protocol { get; }

		// -(id)initWithURLRequest:(NSURLRequest *)request protocols:(NSArray *)protocols;
		[Export("initWithURLRequest:protocols:")]
		// TODO [Verify (StronglyTypedNSArray)]
		IntPtr Constructor (NSUrlRequest request, NSObject[] protocols);

		// -(id)initWithURLRequest:(NSURLRequest *)request;
		[Export("initWithURLRequest:")]
		IntPtr Constructor (NSUrlRequest request);

		// -(id)initWithURL:(NSURL *)url protocols:(NSArray *)protocols;
		[Export("initWithURL:protocols:")]
		// TODO [Verify (StronglyTypedNSArray)]
		IntPtr Constructor (NSUrl url, NSObject[] protocols);

		// -(id)initWithURL:(NSURL *)url;
		[Export("initWithURL:")]
		IntPtr Constructor (NSUrl url);

		// -(void)setDelegateOperationQueue:(NSOperationQueue *)queue;
		[Export("setDelegateOperationQueue:")]
		void SetDelegateOperationQueue (NSOperationQueue queue);

		// -(void)setDelegateDispatchQueue:(dispatch_queue_t)queue;
		[Export("setDelegateDispatchQueue:")]
		void SetDelegateDispatchQueue (DispatchQueue queue);

		// -(void)scheduleInRunLoop:(NSRunLoop *)aRunLoop forMode:(NSString *)mode;
		[Export("scheduleInRunLoop:forMode:")]
		void ScheduleInRunLoop (NSRunLoop aRunLoop, string mode);

		// -(void)unscheduleFromRunLoop:(NSRunLoop *)aRunLoop forMode:(NSString *)mode;
		[Export("unscheduleFromRunLoop:forMode:")]
		void UnscheduleFromRunLoop (NSRunLoop aRunLoop, string mode);

		// -(void)open;
		[Export("open")]
		void Open ();

		// -(void)close;
		[Export("close")]
		void Close ();

		// -(void)closeWithCode:(NSInteger)code reason:(NSString *)reason;
		[Export("closeWithCode:reason:")]
		void Close (StatusCode code, string reason);

		// -(void)send:(id)data;
		[Export("send:")]
		void Send (NSObject data);

		// -(void)sendPing:(NSData *)data;
		[Export("sendPing:")]
		void SendPing ([NullAllowed] NSData data);
	}

	interface IWebSocketDelegate
	{

	}

	// @protocol SRWebSocketDelegate <NSObject>
	[Protocol, Model]
	[BaseType(typeof(NSObject), Name = "SRWebSocketDelegate")]
	interface WebSocketDelegate
	{
		// @required -(void)webSocket:(SRWebSocket *)webSocket didReceiveMessage:(id)message;
		[Abstract]
		[Export("webSocket:didReceiveMessage:")]
		[EventArgs("WebSocketReceivedMessage")]
		void ReceivedMessage (WebSocket webSocket, NSObject message);

		// @optional -(void)webSocketDidOpen:(SRWebSocket *)webSocket;
		[Export("webSocketDidOpen:")]
		[EventArgs("WebSocketOpened")]
		void WebSocketOpened (WebSocket webSocket);

		// @optional -(void)webSocket:(SRWebSocket *)webSocket didFailWithError:(NSError *)error;
		[Export("webSocket:didFailWithError:")]
		[EventArgs("WebSocketFailed")]
		void WebSocketFailed (WebSocket webSocket, NSError error);

		// @optional -(void)webSocket:(SRWebSocket *)webSocket didCloseWithCode:(NSInteger)code reason:(NSString *)reason wasClean:(BOOL)wasClean;
		[Export("webSocket:didCloseWithCode:reason:wasClean:")]
		[EventArgs("WebSocketClosed")]
		void WebSocketClosed (WebSocket webSocket, StatusCode code, string reason, bool wasClean);

		// @optional -(void)webSocket:(SRWebSocket *)webSocket didReceivePong:(NSData *)pongPayload;
		[Export("webSocket:didReceivePong:")]
		[EventArgs("WebSocketReceivedPong")]
		void ReceivedPong (WebSocket webSocket, NSData pongPayload);
	}

	// @interface CertificateAdditions (NSURLRequest)
	[Category]
	[BaseType(typeof(NSUrlRequest))]
	interface NSUrlRequestExtensions
	{
		// @property (readonly, retain, nonatomic) NSArray * SR_SSLPinnedCertificates;
		[Export("SR_SSLPinnedCertificates", ArgumentSemantic.Retain)]
		// TODO [Verify (StronglyTypedNSArray)]
		NSObject[] GetSSLPinnedCertificates ();
	}

	// @interface CertificateAdditions (NSMutableURLRequest)
	[Category]
	[BaseType(typeof(NSMutableUrlRequest))]
	interface NSMutableUrlRequestExtensions
	{
		// @property (retain, nonatomic) NSArray * SR_SSLPinnedCertificates;
		[Export("SR_SSLPinnedCertificates", ArgumentSemantic.Retain)]
		// TODO [Verify (StronglyTypedNSArray)]
		NSObject[] GetSSLPinnedCertificates ();

		// @property (retain, nonatomic) NSArray * SR_SSLPinnedCertificates;
		[Export("setSR_SSLPinnedCertificates:")]
		// TODO [Verify (StronglyTypedNSArray)]
		void SetSSLPinnedCertificates (NSObject[] value);
	}

	// @interface SRWebSocket (NSRunLoop)
	[Category]
	[BaseType(typeof(NSRunLoop))]
	interface NSRunLoopExtensions
	{
		// +(NSRunLoop *)SR_networkRunLoop;
		[Static]
		[Export("SR_networkRunLoop")]
		NSRunLoop GetNetworkRunLoop ();
	}
}