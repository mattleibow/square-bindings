using System;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Square.CoreAardvark
{
	// @interface ARKDataArchive : NSObject
	[BaseType (typeof(NSObject), Name = "ARKDataArchive")]
	[DisableDefaultCtor]
	interface DataArchive
	{
		// -(instancetype _Nullable)initWithURL:(NSURL * _Nonnull)fileURL maximumObjectCount:(NSUInteger)maximumObjectCount trimmedObjectCount:(NSUInteger)trimmedObjectCount __attribute__((objc_designated_initializer));
		[Export ("initWithURL:maximumObjectCount:trimmedObjectCount:")]
		[DesignatedInitializer]
		IntPtr Constructor (NSUrl fileURL, nuint maximumObjectCount, nuint trimmedObjectCount);

		// @property (readonly, nonatomic) NSUInteger maximumObjectCount;
		[Export ("maximumObjectCount")]
		nuint MaximumObjectCount { get; }

		// @property (readonly, nonatomic) NSUInteger trimmedObjectCount;
		[Export ("trimmedObjectCount")]
		nuint TrimmedObjectCount { get; }

		// @property (readonly, copy, nonatomic) NSURL * _Nonnull archiveFileURL;
		[Export ("archiveFileURL", ArgumentSemantic.Copy)]
		NSUrl ArchiveFileUrl { get; }

		// -(void)appendArchiveOfObject:(id<NSSecureCoding> _Nonnull)object;
		[Export ("appendArchiveOfObject:")]
		void AppendArchive (INSSecureCoding obj);

		// -(void)readObjectsFromArchiveWithCompletionHandler:(void (^ _Nonnull)(NSArray * _Nonnull))completionHandler;
		[Export ("readObjectsFromArchiveWithCompletionHandler:")]
		void ReadObjects (Action<NSArray> completionHandler);

		// -(void)clearArchiveWithCompletionHandler:(dispatch_block_t _Nullable)completionHandler;
		[Export ("clearArchiveWithCompletionHandler:")]
		void ClearArchive ([NullAllowed] Action completionHandler);

		// -(void)saveArchiveAndWait:(BOOL)wait;
		[Export ("saveArchiveAndWait:")]
		void SaveArchive (bool wait);
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
	}

	// @interface Protected (ARKLogDistributor)
	[Category]
	[BaseType (typeof(LogDistributor))]
	interface LogDistributorExtensions
	{
		// -(void)waitUntilAllPendingLogsHaveBeenDistributed;
		[Export ("waitUntilAllPendingLogsHaveBeenDistributed")]
		void WaitUntilAllPendingLogsHaveBeenDistributed ();
	}

	// @interface ARKLogMessage : NSObject <NSCopying, NSSecureCoding>
	[BaseType (typeof(NSObject), Name = "ARKLogMessage")]
	[DisableDefaultCtor]
	interface LogMessage : INSCopying, INSSecureCoding
	{
		// -(instancetype _Nonnull)initWithText:(NSString * _Nonnull)text image:(UIImage * _Nullable)image type:(ARKLogType)type userInfo:(NSDictionary * _Nullable)userInfo;
		[Export ("initWithText:image:type:userInfo:")]
		IntPtr Constructor (string text, [NullAllowed] UIImage image, LogType type, [NullAllowed] NSDictionary userInfo);

		// - (instancetype)initWithText:(NSString *)text image:(nullable UIImage *)image type:(ARKLogType)type userInfo:(nullable NSDictionary *)userInfo date:(NSDate *)date NS_DESIGNATED_INITIALIZER;
		[Export ("initWithText:image:type:userInfo:date:")]
		IntPtr Constructor (string text, [NullAllowed] UIImage image, LogType type, [NullAllowed] NSDictionary userInfo, NSDate date);

		// @property (readonly, copy, nonatomic) NSDate * _Nonnull date;
		[Export ("date", ArgumentSemantic.Copy)]
		NSDate Date { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull text;
		[Export ("text")]
		string Text { get; }

		// @property (readonly, nonatomic) UIImage * _Nonnull image;
		[Export ("image")]
		[NullAllowed]
		UIImage Image { get; }

		// @property (readonly, nonatomic) ARKLogType type;
		[Export ("type")]
		LogType Type { get; }

		// @property (readonly, copy, nonatomic) NSDictionary * _Nonnull userInfo;
		[Export ("userInfo", ArgumentSemantic.Copy)]
		[NullAllowed]
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

		// -(void)waitUntilAllLogsAreConsumedAndArchiveSaved;
		[Export ("waitUntilAllLogsAreConsumedAndArchiveSaved")]
		void WaitUntilAllLogsAreConsumedAndArchiveSaved ();
	}
}
