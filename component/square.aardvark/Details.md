# Aardvark Details

> Aardvark is a library that makes it dead simple to create actionable bug reports.

## Usage

Once installed, Aardvark logging can be used with the `Aardvark` type:

    Arrdvark.Log("something happened");
    
We can also add the default two-finger, long-tap gesture to report a bug:

    Aardvark.AddDefaultBugReportingGesture("bugs@example.org");

*NOTE: It is best to do this when you load your application’s UI.*

## Reporting Bugs

Users can report a bug by making a two-finger long-press gesture. This gesture triggers a
`UIAlert` asking the user what went wrong. When the user enters this information, an email 
bug report is generated complete with an attached app screenshot and a text file containing 
the last 2000 logs. 

Screenshots are created and stored within Aardvark and do not require camera roll access.

## Bug Report Flow

To view logs on the device, push an instance of `LogTableViewController` onto the screen to view
the apps logs.

## Performance

Logs are distributed to loggers on an internal background queue that will never slow down your app. 
Logs observed by the log store are incrementally appended to disk and not stored in memory.

## Customize Aardvark

To customize how bug reports are filed, we pass our own object conforming to the `IBugReporter` 
interface and the desired subclass of `UIGestureRecognizer` to `Aardvark.AddBugReporter`:

    IBugReporter bugReporter = new CustomBugReporter();
    Aardvark.AddBugReporter<CustomGestureRecognizer>(bugReporter);

To change how logs are formatted, we set our own `ILogFormatter` on the `EmailBugReporter` type:

    bugReporter.LogFormatter = new CustomLogFormatter();

To log to the console, we set the `PrintsLogsToConsole` property to `true`:

    LogDistributor.DefaultDistributor.DefaultLogStore.PrintsLogsToConsole = true;

To create different log files for different features, we create a `LogStore` for each feature we
want to have its own log file and add them to the default log distributor:

    LogStore featureLogStore = new LogStore("FeatureLogs.data");
    LogDistributor.DefaultDistributor.AddLogObserver(featureLogStore);

Then, we set the `LogFilterBlock` on our `LogStore` to make sure only the logs we want are observed
by the `LogStore`. We can use the user info dictionary to specify to which feature a log pertains:

    featureLogStore.LogFilterBlock = logMessage => {
        return logMessage.UserInfo != null && 
               logMessage.UserInfo ["isFeature"] != null;
    };

To send your logs to third party services, we can easily distribute to multiple services by adding 
objects implementing the `ILogObserver` interface:

    ILogObserver observer = new CustomLogObserver();
    LogDistributor.DefaultDistributor.AddLogObserver(observer);

To log with Aardvark but don’t use Aardvark’s bug reporting tool, do not use the 
`Aardvark.AddDefaultBugReportingGesture` method. Rather manually add an implementation of
`ILogObserver` to the default `LogDistributor`:

    LogStore silentLogger = new LogStore("Logs.data");
    LogDistributor.DefaultDistributor.AddLogObserver(silentLogger);
