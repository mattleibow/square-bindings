using System;
using Foundation;
using ObjCRuntime;
using UIKit;

using Square.CoreAardvark;

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
	[DisableDefaultCtor]
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
	[BaseType (typeof(NSObject), Name = "_TtC8Aardvark8Aardvark")]
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

	// @interface UIAdditions (ARKLogDistributor)
	[Category]
	[BaseType (typeof(LogDistributor))]
	interface LogDistributorExtensions
	{
		// -(void)logScreenshot;
		[Export ("logScreenshot")]
		void LogScreenshot ();
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
}
