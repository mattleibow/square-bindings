using System;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Square.Aardvark
{
	interface IBugReporter
	{

	}

	// @protocol ARKBugReporter <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject), Name = "ARKBugReporter")]
	interface BugReporter
	{
		// @required -(void)composeBugReport;
		[Abstract]
		[Export ("composeBugReport")]
		void ComposeBugReport ();

		// @required -(void)composeBugReportWithoutScreenshot;
		[Abstract]
		[Export ("composeBugReportWithoutScreenshot")]
		void ComposeBugReportWithoutScreenshot ();

		// @required -(void)addLogStores:(NSArray * _Nonnull)logStores;
		[Abstract]
		[Export ("addLogStores:")]
		void AddLogStores (LogStore[] logStores);

		// @required -(void)removeLogStores:(NSArray * _Nonnull)logStores;
		[Abstract]
		[Export ("removeLogStores:")]
		void RemoveLogStores (LogStore[] logStores);

		// @required -(NSArray * _Nonnull)logStores;
		[Abstract]
		[Export ("logStores")]
		LogStore[] LogStores { get; }
	}

	interface IEmailBodyAdditionsDelegate
	{

	}

	// @protocol ARKEmailBugReporterEmailBodyAdditionsDelegate <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject), Name = "ARKEmailBugReporterEmailBodyAdditionsDelegate")]
	interface EmailBodyAdditionsDelegate
	{
		// @required -(NSDictionary * _Nullable)emailBodyAdditionsForEmailBugReporter:(ARKEmailBugReporter * _Nonnull)emailBugReporter;
		[Abstract]
		[Export ("emailBodyAdditionsForEmailBugReporter:")]
		[return: NullAllowed]
		NSDictionary EmailBodyAdditions (EmailBugReporter emailBugReporter);
	}

	// @interface ARKEmailBugReporter : NSObject <ARKBugReporter>
	[BaseType (typeof(NSObject), Name = "ARKEmailBugReporter")]
	interface EmailBugReporter : BugReporter
	{
		// -(instancetype _Nonnull)initWithEmailAddress:(NSString * _Nonnull)emailAddress logStore:(ARKLogStore * _Nonnull)logStore;
		[Export ("initWithEmailAddress:logStore:")]
		IntPtr Constructor (string emailAddress, LogStore logStore);

		// @property (copy, nonatomic) NSString * _Nonnull bugReportRecipientEmailAddress;
		[Export ("bugReportRecipientEmailAddress")]
		string RecipientEmailAddress { get; set; }

		// @property (copy, nonatomic) NSString * _Nonnull prefilledEmailBody;
		[Export ("prefilledEmailBody")]
		string PrefilledEmailBody { get; set; }

		// @property (nonatomic, weak) id<ARKEmailBugReporterEmailBodyAdditionsDelegate> _Nullable emailBodyAdditionsDelegate;
		[NullAllowed, Export ("emailBodyAdditionsDelegate", ArgumentSemantic.Weak)]
		IEmailBodyAdditionsDelegate EmailBodyAdditionsDelegate { get; set; }

		// @property (nonatomic) id<ARKLogFormatter> _Nonnull logFormatter;
		[Export ("logFormatter", ArgumentSemantic.Assign)]
		ILogFormatter LogFormatter { get; set; }

		// @property (nonatomic) NSUInteger numberOfRecentErrorLogsToIncludeInEmailBodyWhenAttachmentsAreAvailable;
		[Export ("numberOfRecentErrorLogsToIncludeInEmailBodyWhenAttachmentsAreAvailable")]
		nuint NumberOfRecentErrorLogsToIncludeInEmailBodyWhenAttachmentsAreAvailable { get; set; }

		// @property (nonatomic) NSUInteger numberOfRecentErrorLogsToIncludeInEmailBodyWhenAttachmentsAreUnavailable;
		[Export ("numberOfRecentErrorLogsToIncludeInEmailBodyWhenAttachmentsAreUnavailable")]
		nuint NumberOfRecentErrorLogsToIncludeInEmailBodyWhenAttachmentsAreUnavailable { get; set; }

		// @property (nonatomic) UIWindowLevel emailComposeWindowLevel;
		[Export ("emailComposeWindowLevel")]
		double EmailComposeWindowLevel { get; set; }

		// -(NSData * _Nonnull)formattedLogMessagesAsData:(NSArray * _Nonnull)logMessages;
		[Export ("formattedLogMessagesAsData:")]
		NSData FormatAsData (LogMessage[] logMessages);

		// -(NSString * _Nonnull)formattedLogMessagesDataMIMEType;
		[Export ("formattedLogMessagesDataMIMEType")]
		string DataMimeType { get; }

		// -(NSString * _Nonnull)formattedLogMessagesAttachmentExtension;
		[Export ("formattedLogMessagesAttachmentExtension")]
		string AttachmentExtension { get; }
	}

	// @interface Aardvark : NSObject
	[BaseType (typeof(NSObject), Name = "Aardvark")]
	interface Aardvark
	{
		// +(ARKEmailBugReporter * _Nullable)addDefaultBugReportingGestureWithEmailBugReporterWithRecipient:(NSString * _Nonnull)emailAddress;
		[Static]
		[Export ("addDefaultBugReportingGestureWithEmailBugReporterWithRecipient:")]
		[return: NullAllowed]
		EmailBugReporter AddDefaultBugReportingGesture (string emailAddress);

		// +(id _Nullable)addBugReporter:(id<ARKBugReporter> _Nonnull)bugReporter triggeringGestureRecognizerClass:(Class _Nonnull)gestureRecognizerClass;
		[Static]
		[Export ("addBugReporter:triggeringGestureRecognizerClass:")]
		[return: NullAllowed]
		UIGestureRecognizer AddBugReporter (IBugReporter bugReporter, Class gestureRecognizerClass);
	}

	interface ILogFormatter
	{

	}

	// @protocol ARKLogFormatter <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject), Name = "ARKLogFormatter")]
	interface LogFormatter
	{
		// @required -(NSString * _Nonnull)formattedLogMessage:(ARKLogMessage * _Nonnull)logMessage;
		[Abstract]
		[Export ("formattedLogMessage:")]
		string Format (LogMessage logMessage);
	}

	// @interface ARKDefaultLogFormatter : NSObject <ARKLogFormatter>
	[BaseType (typeof(NSObject), Name = "ARKDefaultLogFormatter")]
	interface DefaultLogFormatter : LogFormatter
	{
		// @property (copy, nonatomic) NSString * _Nonnull errorLogPrefix;
		[Export ("errorLogPrefix")]
		string ErrorLogPrefix { get; set; }

		// @property (copy, nonatomic) NSString * _Nonnull separatorLogPrefix;
		[Export ("separatorLogPrefix")]
		string SeparatorLogPrefix { get; set; }
	}

	// @interface ARKIndividualLogViewController : UIViewController
	[BaseType (typeof(UIViewController), Name = "ARKIndividualLogViewController")]
	interface IndividualLogViewController
	{
		// -(instancetype _Nullable)initWithLogMessage:(ARKLogMessage * _Nonnull)logMessage;
		[Export ("initWithLogMessage:")]
		IntPtr Constructor (LogMessage logMessage);
	}

	// @interface ARKLogDistributor : NSObject
	[BaseType (typeof(NSObject), Name = "ARKLogDistributor")]
	interface LogDistributor
	{
		// +(instancetype _Nullable)defaultDistributor;
		[Static]
		[Export ("defaultDistributor")]
		LogDistributor DefaultDistributor { [return: NullAllowed] get; }

		// @property Class _Nonnull logMessageClass;
		[Export ("logMessageClass", ArgumentSemantic.Assign)]
		Class LogMessageClass { get; set; }

		// @property ARKLogStore * _Nonnull defaultLogStore;
		[Export ("defaultLogStore", ArgumentSemantic.Assign)]
		LogStore DefaultLogStore { get; set; }

		// @property (readonly, copy, atomic) NSSet * _Nonnull logStores;
		[Export ("logStores", ArgumentSemantic.Copy)]
		NSSet LogStores { get; }

		// -(void)addLogObserver:(id<ARKLogObserver> _Nonnull)logObserver;
		[Export ("addLogObserver:")]
		void AddLogObserver (ILogObserver logObserver);

		// -(void)removeLogObserver:(id<ARKLogObserver> _Nonnull)logObserver;
		[Export ("removeLogObserver:")]
		void RemoveLogObserver (ILogObserver logObserver);

		// -(void)distributeAllPendingLogsWithCompletionHandler:(dispatch_block_t _Nonnull)completionHandler;
		[Export ("distributeAllPendingLogsWithCompletionHandler:")]
		void DistributeAllPendingLogs (Action completionHandler);

		// -(void)logMessage:(ARKLogMessage * _Nonnull)logMessage;
		[Export ("logMessage:")]
		void Log (LogMessage logMessage);

		// -(void)logWithText:(NSString * _Nonnull)text image:(UIImage * _Nullable)image type:(ARKLogType)type userInfo:(NSDictionary * _Nullable)userInfo;
		[Export ("logWithText:image:type:userInfo:")]
		void Log (string text, [NullAllowed] UIImage image, LogType type, [NullAllowed] NSDictionary userInfo);

		// -(void)logWithType:(ARKLogType)type userInfo:(NSDictionary * _Nullable)userInfo format:(NSString * _Nonnull)format, ... __attribute__((format(NSString, 3, 4)));
		[Internal]
		[Export ("logWithType:userInfo:format:", IsVariadic = true)]
		void Log (LogType type, [NullAllowed] NSDictionary userInfo, string format, IntPtr varArgs);

		// -(void)logWithFormat:(NSString * _Nonnull)format, ... __attribute__((format(NSString, 1, 2)));
		[Internal]
		[Export ("logWithFormat:", IsVariadic = true)]
		void Log (string format, IntPtr varArgs);

		// -(void)logScreenshot;
		[Export ("logScreenshot")]
		void LogScreenshot ();
	}

	// @interface ARKLogMessage : NSObject <NSCopying, NSSecureCoding>
	[BaseType (typeof(NSObject), Name = "ARKLogMessage")]
	interface LogMessage : INSCopying, INSSecureCoding
	{
		// -(instancetype _Nonnull)initWithText:(NSString * _Nonnull)text image:(UIImage * _Nullable)image type:(ARKLogType)type userInfo:(NSDictionary * _Nullable)userInfo;
		[Export ("initWithText:image:type:userInfo:")]
		IntPtr Constructor (string text, [NullAllowed] UIImage image, LogType type, [NullAllowed] NSDictionary userInfo);

		// @property (readonly, copy, nonatomic) NSDate * _Nonnull creationDate;
		[Export ("creationDate", ArgumentSemantic.Copy)]
		NSDate CreationDate { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull text;
		[Export ("text")]
		string Text { get; }

		// @property (readonly, nonatomic) UIImage * _Nonnull image;
		[Export ("image")]
		UIImage Image { get; }

		// @property (readonly, nonatomic) ARKLogType type;
		[Export ("type")]
		LogType Type { get; }

		// @property (readonly, copy, nonatomic) NSDictionary * _Nonnull userInfo;
		[Export ("userInfo", ArgumentSemantic.Copy)]
		NSDictionary UserInfo { get; }
	}

	interface ILogObserver
	{

	}

	// @protocol ARKLogObserver <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject), Name = "ARKLogObserver")]
	interface LogObserver
	{
		// @required @property (weak) ARKLogDistributor * _Nullable logDistributor;
		[Abstract, NullAllowed, Export ("logDistributor", ArgumentSemantic.Weak)]
		LogDistributor LogDistributor { get; set; }

		// @required -(void)observeLogMessage:(ARKLogMessage * _Nonnull)logMessage;
		[Abstract]
		[Export ("observeLogMessage:")]
		void Observe (LogMessage logMessage);
	}

	// @interface ARKLogStore : NSObject <ARKLogObserver>
	[BaseType (typeof(NSObject), Name = "ARKLogStore")]
	[DisableDefaultCtor]
	interface LogStore : LogObserver
	{
		// -(instancetype _Nullable)initWithPersistedLogFileName:(NSString * _Nonnull)fileName maximumLogMessageCount:(NSUInteger)maximumLogMessageCount __attribute__((objc_designated_initializer));
		[Export ("initWithPersistedLogFileName:maximumLogMessageCount:")]
		[DesignatedInitializer]
		IntPtr Constructor (string fileName, nuint maximumLogMessageCount);

		// -(instancetype _Nullable)initWithPersistedLogFileName:(NSString * _Nonnull)fileName;
		[Export ("initWithPersistedLogFileName:")]
		IntPtr Constructor (string fileName);

		// @property (readonly, copy, nonatomic) NSURL * _Nonnull persistedLogFileURL;
		[Export ("persistedLogFileURL", ArgumentSemantic.Copy)]
		NSUrl PersistedLogFileUrl { get; }

		// @property (readonly, nonatomic) NSUInteger maximumLogMessageCount;
		[Export ("maximumLogMessageCount")]
		nuint MaximumLogMessageCount { get; }

		// @property (copy) NSString * _Nonnull name;
		[Export ("name")]
		string Name { get; set; }

		// @property BOOL printsLogsToConsole;
		[Export ("printsLogsToConsole")]
		bool PrintsLogsToConsole { get; set; }

		// @property BOOL prefixNameWhenPrintingToConsole;
		[Export ("prefixNameWhenPrintingToConsole")]
		bool PrefixNameWhenPrintingToConsole { get; set; }

		// @property (copy) BOOL (^ _Nonnull)(ARKLogMessage * _Nonnull) logFilterBlock;
		[Export ("logFilterBlock", ArgumentSemantic.Copy)]
		Func<LogMessage, bool> LogFilterBlock { get; set; }

		// -(void)retrieveAllLogMessagesWithCompletionHandler:(void (^ _Nonnull)(NSArray * _Nonnull))completionHandler;
		[Export ("retrieveAllLogMessagesWithCompletionHandler:")]
		void RetrieveAllLogMessages (Action<NSArray> completionHandler);

		// -(void)clearLogsWithCompletionHandler:(dispatch_block_t _Nullable)completionHandler;
		[Export ("clearLogsWithCompletionHandler:")]
		void ClearLogs ([NullAllowed] Action completionHandler);
	}

	// @interface ARKLogTableViewController : UITableViewController
	[BaseType (typeof(UITableViewController), Name = "ARKLogTableViewController")]
	interface LogTableViewController
	{
		// -(instancetype _Nonnull)initWithNibName:(NSString * _Nullable)nibNameOrNil bundle:(NSBundle * _Nullable)nibBundleOrNil;
		[Export ("initWithNibName:bundle:")]
		IntPtr Constructor ([NullAllowed] string nibNameOrNil, [NullAllowed] NSBundle nibBundleOrNil);

		// -(instancetype _Nullable)initWithLogStore:(ARKLogStore * _Nonnull)logStore logFormatter:(id<ARKLogFormatter> _Nonnull)logFormatter __attribute__((objc_designated_initializer));
		[Export ("initWithLogStore:logFormatter:")]
		[DesignatedInitializer]
		IntPtr Constructor (LogStore logStore, ILogFormatter logFormatter);

		// @property (readonly, nonatomic) ARKLogStore * _Nonnull logStore;
		[Export ("logStore")]
		LogStore LogStore { get; }

		// @property (readonly, nonatomic) id<ARKLogFormatter> _Nonnull logFormatter;
		[Export ("logFormatter")]
		ILogFormatter LogFormatter { get; }

		// @property (nonatomic) NSUInteger minutesBetweenTimestamps;
		[Export ("minutesBetweenTimestamps")]
		nuint MinutesBetweenTimestamps { get; set; }

		// -(NSArray * _Nonnull)contentForActivitySheet;
		[Export ("contentForActivitySheet")]
		NSObject[] ContentForActivitySheet { get; }
	}

	// @interface ARKScreenshotViewController : UIViewController
	[BaseType (typeof(UIViewController), Name = "ARKScreenshotViewController")]
	interface ScreenshotViewController
	{
		// -(instancetype _Nullable)initWithLogMessage:(ARKLogMessage * _Nonnull)logMessage;
		[Export ("initWithLogMessage:")]
		IntPtr Constructor (LogMessage logMessage);
	}

	// @interface ARKAdditions (UIApplication)
	[Category]
	[BaseType (typeof(UIApplication))]
	interface UIApplicationExtensions
	{
		// -(void)ARK_addTwoFingerPressAndHoldGestureRecognizerTriggerWithBugReporter:(id<ARKBugReporter> _Nonnull)bugReporter;
		[Export ("ARK_addTwoFingerPressAndHoldGestureRecognizerTriggerWithBugReporter:")]
		void AddTwoFingerPressAndHoldGestureRecognizerTrigger (IBugReporter bugReporter);

		// -(id _Nullable)ARK_addBugReporter:(id<ARKBugReporter> _Nonnull)bugReporter triggeringGestureRecognizerClass:(Class _Nonnull)gestureRecognizerClass;
		[Export ("ARK_addBugReporter:triggeringGestureRecognizerClass:")]
		[return: NullAllowed]
		UIGestureRecognizer AddBugReporter (IBugReporter bugReporter, Class gestureRecognizerClass);

		// -(void)ARK_removeBugReporter:(id<ARKBugReporter> _Nonnull)bugReporter;
		[Export ("ARK_removeBugReporter:")]
		void RemoveBugReporter (IBugReporter bugReporter);
	}
}
