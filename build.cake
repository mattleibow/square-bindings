#addin nuget:?package=Cake.XCode&version=4.0.1
#addin nuget:?package=Cake.FileHelpers&version=3.2.0
#addin nuget:?package=Cake.Xamarin&version=3.0.1

using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("t", Argument("target", "Default"));
var configuration = Argument("c", Argument("configuration", "Release"));
var buildNumber = EnvironmentVariable ("BUILD_NUMBER") ?? "0";

if (!DirectoryExists ("./output")) {
    CreateDirectory ("./output");
}

public enum TargetOS {
    Windows,
    Mac,
    Android,
    iOS,
    tvOS,
}

//////////////////////////////////////////////////////////////////////
// VERSIONS
//////////////////////////////////////////////////////////////////////

var versions = new Dictionary<string, string> {
    { "Square.OkIO",                             "1.13.0"   },
    { "Square.OkHttp",                           "2.7.5"    },
    { "Square.OkHttp.WS",                        "2.7.5"    },
    { "Square.OkHttp3",                          "3.8.1"    },
    { "Square.OkHttp3.WS",                       "3.4.2"    },
    { "Square.OkHttp.UrlConnection",             "2.7.5"    },
    { "Square.Picasso",                          "2.5.2"    },
    { "Square.AndroidTimesSquare",               "1.7.3"    },
    { "Square.SocketRocket",                     "0.5.1"    },
    { "Square.Valet",                            "2.4.1"    },
    { "Square.Aardvark",                         "3.4.1"    },
    { "Square.CoreAardvark",                     "2.2.1"    },
    { "Square.Seismic",                          "1.0.2"    },
    { "Square.Pollexor",                         "2.0.4"    },
    { "Square.Retrofit",                         "1.9.0"    },
    { "Square.Retrofit2",                        "2.4.0"    },
    { "Square.Retrofit2.ConverterGson",          "2.4.0"    },
    { "Square.Retrofit2.AdapterRxJava2",         "2.4.0"    },
    { "JakeWharton.Picasso2OkHttp3Downloader",   "1.1.0"    },
};

var macOnly = new [] {
    "Square.SocketRocket",
    "Square.Valet",
    "Square.Aardvark",
    "Square.CoreAardvark",
};

////////////////////////////////////////////////////////////////////////////////////////////////////
// TOOLS & FUNCTIONS - the bits to make it all work
////////////////////////////////////////////////////////////////////////////////////////////////////

var NugetToolPath = Context.Tools.Resolve ("nuget.exe");

void RunLipo (DirectoryPath directory, FilePath output, FilePath[] inputs)
{
    if (!IsRunningOnUnix ()) {
        throw new InvalidOperationException ("lipo is only available on Unix.");
    }

    var dir = directory.CombineWithFilePath (output).GetDirectory ();
    if (!DirectoryExists (dir)) {
        CreateDirectory (dir);
    }

    var inputString = string.Join(" ", inputs.Select (i => string.Format ("\"{0}\"", i)));
    StartProcess ("lipo", new ProcessSettings {
        Arguments = string.Format("-create -output \"{0}\" {1}", output, inputString),
        WorkingDirectory = directory,
    });
}

void BuildXCode (FilePath project, string libraryTitle, DirectoryPath workingDirectory, TargetOS os)
{
    if (!IsRunningOnUnix ()) {
        return;
    }

    var fatLibrary = string.Format("lib{0}.a", libraryTitle);

    var output = string.Format ("lib{0}.a", libraryTitle);
    var i386 = string.Format ("lib{0}-i386.a", libraryTitle);
    var x86_64 = string.Format ("lib{0}-x86_64.a", libraryTitle);
    var armv7 = string.Format ("lib{0}-armv7.a", libraryTitle);
    var armv7s = string.Format ("lib{0}-armv7s.a", libraryTitle);
    var arm64 = string.Format ("lib{0}-arm64.a", libraryTitle);

    var buildArch = new Action<string, string, FilePath> ((sdk, arch, dest) => {
        if (!FileExists (dest)) {
            XCodeBuild (new XCodeBuildSettings {
                Project = workingDirectory.CombineWithFilePath (project).ToString (),
                Target = libraryTitle,
                Sdk = sdk,
                Arch = arch,
                Configuration = "Release",
            });
            var outputPath = workingDirectory.Combine ("build").Combine (os == TargetOS.Mac ? "Release" : ("Release-" + sdk)).Combine (libraryTitle).CombineWithFilePath (output);
            CopyFile (outputPath, dest);
        }
    });

    if (os == TargetOS.Mac) {
        buildArch ("macosx", "x86_64", workingDirectory.CombineWithFilePath (x86_64));
        if (!FileExists (workingDirectory.CombineWithFilePath (fatLibrary))) {
            RunLipo (workingDirectory, fatLibrary, new [] {
                (FilePath)x86_64
            });
        }
    } else if (os == TargetOS.iOS) {
        buildArch ("iphonesimulator", "i386", workingDirectory.CombineWithFilePath (i386));
        buildArch ("iphonesimulator", "x86_64", workingDirectory.CombineWithFilePath (x86_64));
        buildArch ("iphoneos", "armv7", workingDirectory.CombineWithFilePath (armv7));
        buildArch ("iphoneos", "armv7s", workingDirectory.CombineWithFilePath (armv7s));
        buildArch ("iphoneos", "arm64", workingDirectory.CombineWithFilePath (arm64));
        if (!FileExists (workingDirectory.CombineWithFilePath (fatLibrary))) {
            RunLipo (workingDirectory, fatLibrary, new [] {
                (FilePath)i386,
                (FilePath)x86_64,
                (FilePath)armv7,
                (FilePath)armv7s,
                (FilePath)arm64
            });
        }
    } else if (os == TargetOS.tvOS) {
        buildArch ("appletvsimulator", "x86_64", workingDirectory.CombineWithFilePath (x86_64));
        buildArch ("appletvos", "arm64", workingDirectory.CombineWithFilePath (arm64));
        if (!FileExists (workingDirectory.CombineWithFilePath (fatLibrary))) {
            RunLipo (workingDirectory, fatLibrary, new [] {
                (FilePath)x86_64,
                (FilePath)arm64
            });
        }
    }
}

void BuildDynamicXCode (FilePath project, string libraryTitle, DirectoryPath workingDirectory, TargetOS os)
{
    if (!IsRunningOnUnix ())
        return;

    var fatLibrary = (DirectoryPath)string.Format("{0}.framework", libraryTitle);
    var fatLibraryPath = workingDirectory.Combine (fatLibrary);

    var output = (DirectoryPath)string.Format ("{0}.framework", libraryTitle);
    var i386 = (DirectoryPath)string.Format ("{0}-i386.framework", libraryTitle);
    var x86_64 = (DirectoryPath)string.Format ("{0}-x86_64.framework", libraryTitle);
    var armv7 = (DirectoryPath)string.Format ("{0}-armv7.framework", libraryTitle);
    var armv7s = (DirectoryPath)string.Format ("{0}-armv7s.framework", libraryTitle);
    var arm64 = (DirectoryPath)string.Format ("{0}-arm64.framework", libraryTitle);

    var buildArch = new Action<string, string, DirectoryPath> ((sdk, arch, dest) => {
        if (!DirectoryExists (dest)) {
            XCodeBuild (new XCodeBuildSettings {
                Project = workingDirectory.CombineWithFilePath (project).ToString (),
                Target = libraryTitle,
                Sdk = sdk,
                Arch = arch,
                Configuration = "Release",
            });
            var outputPath = workingDirectory.Combine ("build").Combine (os == TargetOS.Mac ? "Release" : ("Release-" + sdk)).Combine (libraryTitle).Combine (output);
            CopyDirectory (outputPath, dest);
        }
    });

    if (os == TargetOS.Mac) {
        buildArch ("macosx", "x86_64", workingDirectory.Combine (x86_64));
        if (!DirectoryExists (fatLibraryPath)) {
            CopyDirectory (workingDirectory.Combine (x86_64), fatLibraryPath);
            RunLipo (workingDirectory, fatLibrary.CombineWithFilePath (libraryTitle), new [] {
                x86_64.CombineWithFilePath (libraryTitle),
            });
        }
    } else if (os == TargetOS.iOS) {
        buildArch ("iphonesimulator", "i386", workingDirectory.Combine (i386));
        buildArch ("iphonesimulator", "x86_64", workingDirectory.Combine (x86_64));
        buildArch ("iphoneos", "armv7", workingDirectory.Combine (armv7));
        buildArch ("iphoneos", "armv7s", workingDirectory.Combine (armv7s));
        buildArch ("iphoneos", "arm64", workingDirectory.Combine (arm64));
        if (!DirectoryExists (fatLibraryPath)) {
            CopyDirectory (workingDirectory.Combine (arm64), fatLibraryPath);
            RunLipo (workingDirectory, fatLibrary.CombineWithFilePath (libraryTitle), new [] {
                i386.CombineWithFilePath (libraryTitle),
                x86_64.CombineWithFilePath (libraryTitle),
                armv7.CombineWithFilePath (libraryTitle),
                armv7s.CombineWithFilePath (libraryTitle),
                arm64.CombineWithFilePath (libraryTitle)
            });
        }
    } else if (os == TargetOS.tvOS) {
        buildArch ("appletvsimulator", "x86_64", workingDirectory.Combine (x86_64));
        buildArch ("appletvos", "arm64", workingDirectory.Combine (arm64));
        if (!DirectoryExists (fatLibraryPath)) {
            CopyDirectory (workingDirectory.Combine (arm64), fatLibraryPath);
            RunLipo (workingDirectory, fatLibrary.CombineWithFilePath (libraryTitle), new [] {
                x86_64.CombineWithFilePath (libraryTitle),
                arm64.CombineWithFilePath (libraryTitle)
            });
        }
    }
}

void DownloadPod (bool isDynamic, DirectoryPath podfilePath, string platform, string platformVersion, IDictionary<string, string> pods)
{
    if (!IsRunningOnUnix ())
        return;

    if (!FileExists (podfilePath.CombineWithFilePath ("Podfile.lock"))) {
        var builder = new StringBuilder ();
        builder.AppendFormat ("platform :{0}, '{1}'", platform, platformVersion);
        builder.AppendLine ();
        builder.AppendLine ("install! 'cocoapods', :integrate_targets => false");
        if (isDynamic)
            builder.AppendLine ("use_frameworks!");
        builder.AppendLine ("target 'Xamarin' do");
        foreach (var pod in pods) {
            builder.AppendFormat ("  pod '{0}', '{1}'", pod.Key, pod.Value);
            builder.AppendLine ();
        }
        builder.AppendLine ("end");
        builder.AppendLine ("post_install do |installer|");
        builder.AppendLine ("  installer.pods_project.targets.each do |target|");
        builder.AppendLine ("    target.build_configurations.each do |config|");
        builder.AppendLine ("      config.build_settings['SWIFT_VERSION'] = '4.0'");
        builder.AppendLine ("    end");
        builder.AppendLine ("  end");
        builder.AppendLine ("end");

        if (!DirectoryExists (podfilePath))
            CreateDirectory (podfilePath);

        System.IO.File.WriteAllText (podfilePath.CombineWithFilePath ("Podfile").ToString (), builder.ToString ());

        CocoaPodInstall (podfilePath);
    }
}

void CreatePod (DirectoryPath path, bool isDynamic, string osxVersion, string iosVersion, string tvosVersion, params string[] podIds)
{
    var pods = new Dictionary<string, string> ();
    foreach (var id in podIds) {
        pods [id] = versions ["Square." + id].ToString ();
    }

    path = ((DirectoryPath)"./externals").Combine (path);
    var name = pods.Keys.First ();
    var build = isDynamic
        ? new Action<FilePath, string, DirectoryPath, TargetOS> (BuildDynamicXCode)
        : new Action<FilePath, string, DirectoryPath, TargetOS> (BuildXCode);

    if (osxVersion != null) {
        DownloadPod (isDynamic, path.Combine("osx"), "osx", osxVersion, pods);
        build ("Pods/Pods.xcodeproj", name, path.Combine ("osx"), TargetOS.Mac);
    }
    if (iosVersion != null) {
        DownloadPod (isDynamic, path.Combine("ios"), "ios", iosVersion, pods);
        build ("Pods/Pods.xcodeproj", name, path.Combine ("ios"), TargetOS.iOS);
    }
    if (tvosVersion != null) {
        DownloadPod (isDynamic, path.Combine("tvos"), "tvos", tvosVersion, pods);
        build ("Pods/Pods.xcodeproj", name, path.Combine ("tvos"), TargetOS.tvOS);
    }
}

void DownloadJar (string source, FilePath destination)
{
    destination = ((DirectoryPath)"./externals").CombineWithFilePath (destination);

    var id = destination.GetDirectory ().GetDirectoryName ();
    var version = versions [id];
    var url = string.Format("http://search.maven.org/remotecontent?filepath=" + source, version);

    EnsureDirectoryExists (destination.GetDirectory ());
    if (!FileExists (destination))
        DownloadFile (url, destination);
}

////////////////////////////////////////////////////////////////////////////////////////////////////
// EXTERNALS -
////////////////////////////////////////////////////////////////////////////////////////////////////

Task ("externals")
    .Does (() =>
{
    DownloadJar ("com/squareup/okio/okio/{0}/okio-{0}.jar",
                 "Square.OkIO/okio.jar");
    DownloadJar ("com/squareup/okhttp/okhttp/{0}/okhttp-{0}.jar",
                 "Square.OkHttp/okhttp.jar");
    DownloadJar ("com/squareup/okhttp3/okhttp/{0}/okhttp-{0}.jar",
                 "Square.OkHttp3/okhttp3.jar");
    DownloadJar ("com/squareup/okhttp/okhttp-ws/{0}/okhttp-ws-{0}.jar",
                 "Square.OkHttp.WS/okhttp-ws.jar");
    DownloadJar ("com/squareup/okhttp3/okhttp-ws/{0}/okhttp-ws-{0}.jar",
                 "Square.OkHttp3.WS/okhttp3-ws.jar");
    DownloadJar ("com/squareup/okhttp/okhttp-urlconnection/{0}/okhttp-urlconnection-{0}.jar",
                 "Square.OkHttp.UrlConnection/okhttp-urlconnection.jar");
    DownloadJar ("com/squareup/picasso/picasso/{0}/picasso-{0}.jar",
                 "Square.Picasso/picasso.jar");
    DownloadJar ("com/squareup/android-times-square/{0}/android-times-square-{0}.aar",
                 "Square.AndroidTimesSquare/android-times-square.aar");
    DownloadJar ("com/squareup/seismic/{0}/seismic-{0}.jar",
                 "Square.Seismic/seismic.jar");
    DownloadJar ("com/squareup/pollexor/{0}/pollexor-{0}.jar",
                 "Square.Pollexor/pollexor.jar");
    DownloadJar ("com/squareup/retrofit/retrofit/{0}/retrofit-{0}.jar",
                 "Square.Retrofit/retrofit.jar");
    DownloadJar ("com/squareup/retrofit2/retrofit/{0}/retrofit-{0}.jar",
                 "Square.Retrofit2/retrofit2.jar");
    DownloadJar ("com/squareup/retrofit2/converter-gson/{0}/converter-gson-{0}.jar",
                 "Square.Retrofit2.ConverterGson/convertergson.jar");
    DownloadJar ("com/squareup/retrofit2/adapter-rxjava2/{0}/adapter-rxjava2-{0}.jar",
                 "Square.Retrofit2.AdapterRxJava2/adapterrxjava2.jar");
    DownloadJar ("com/jakewharton/picasso/picasso2-okhttp3-downloader/{0}/picasso2-okhttp3-downloader-{0}.jar",
                 "JakeWharton.Picasso2OkHttp3Downloader/picasso2-okhttp3-downloader.jar");

    if (IsRunningOnUnix ()) {
        CreatePod ("Square.SocketRocket", false, "10.8",  "6.0",  "9.0", new [] { "SocketRocket" });
        CreatePod ("Square.Valet",        false, "10.10", "6.0",  null,  new [] { "Valet" });
        CreatePod ("Square.Aardvark",     true,  null,    "8.0",  null,  new [] { "Aardvark", "CoreAardvark" });
        CreatePod ("Square.CoreAardvark", true,  null,    "8.0",  null,  new [] { "CoreAardvark" });
    }
});

////////////////////////////////////////////////////////////////////////////////////////////////////
// LIBS -
////////////////////////////////////////////////////////////////////////////////////////////////////

Task ("libs")
    .IsDependentOn ("externals")
    .Does (() =>
{
    foreach (var file in GetFiles ("./binding/*/*.csproj").OrderBy (f => f.GetDirectory ().FullPath)) {
        var id = file.GetFilenameWithoutExtension ().ToString ();

        if (!IsRunningOnUnix () && macOnly.Contains (id))
            continue;

        var version = Version.Parse (versions [id]);
        var assemblyVersion = $"{version.Major}.0.0.0";
        var fileVersion     = $"{version.Major}.{version.Minor}.{version.Build}.0";
        var infoVersion     = $"{version.Major}.{version.Minor}.{version.Build}.{buildNumber}";
        var packageVersion  = $"{version.Major}.{version.Minor}.{version.Build}";
        if (!string.IsNullOrEmpty (buildNumber))
            packageVersion += "." + buildNumber;

        var settings = new MSBuildSettings ()
            .SetConfiguration (configuration)
            .SetVerbosity (Verbosity.Minimal)
            .WithRestore ()
            // .WithProperty ("IncludeSymbols", "true")
            .WithProperty ("DesignTimeBuild", "false")
            .WithProperty ("Version", assemblyVersion)
            .WithProperty ("FileVersion", fileVersion)
            .WithProperty ("InformationalVersion", infoVersion)
            .WithProperty ("PackageOutputPath", MakeAbsolute ((DirectoryPath)"./output/").FullPath)
            .WithTarget ("Pack");

        settings.WithProperty ("PackageVersion", packageVersion);
        MSBuild (file, settings);
    }
});

////////////////////////////////////////////////////////////////////////////////////////////////////
// PACKAGING -
////////////////////////////////////////////////////////////////////////////////////////////////////

Task ("nuget")
    .IsDependentOn ("libs");

Task ("component")
    .IsDependentOn ("nuget");

////////////////////////////////////////////////////////////////////////////////////////////////////
// SAMPLES -
////////////////////////////////////////////////////////////////////////////////////////////////////

Task ("samples")
    .IsDependentOn ("libs")
    .Does (() =>
{
    // var samples = new List<string> {
    //     "./sample/AndroidTimesSquareSample/AndroidTimesSquareSample.sln",
    //     "./sample/OkHttp3Sample/OkHttp3Sample.sln",
    //     "./sample/OkHttp3WSSample/OkHttp3WSSample.sln",
    //     "./sample/OkHttpSample/OkHttpSample.sln",
    //     "./sample/OkHttpWSSample/OkHttpWSSample.sln",
    //     "./sample/PicassoSample/PicassoSample.sln",
    //     "./sample/PollexorSample/PollexorSample.sln",
    //     "./sample/SeismicSample/SeismicSample.sln",
    // };
    // if (IsRunningOnUnix ()) {
    //     samples.Add ("./sample/AardvarkSample/AardvarkSample.sln");
    //     samples.Add ("./sample/SocketRocketSample/SocketRocketSample.sln");
    //     samples.Add ("./sample/SocketRocketSample-OSX/SocketRocketSample-OSX.sln");
    //     samples.Add ("./sample/ValetSample/ValetSample.sln");
    // }
    // foreach (var sample in samples) {
    //     RunNuGetRestore (sample);
    //     Build (sample);
    // }
});

////////////////////////////////////////////////////////////////////////////////////////////////////
// CLEAN -
////////////////////////////////////////////////////////////////////////////////////////////////////

Task ("clean")
    .Does (() =>
{
    CleanDirectories ("./binding/*/bin");
    CleanDirectories ("./binding/*/obj");
    CleanDirectories ("./binding/packages");

    CleanDirectories ("./sample/*/bin");
    CleanDirectories ("./sample/*/obj");
    CleanDirectories ("./sample/packages");

    CleanDirectories ("./output");
});

Task ("clean-native")
    .IsDependentOn ("clean")
    .Does (() =>
{
    CleanDirectories("./externals");
});

////////////////////////////////////////////////////////////////////////////////////////////////////
// START -
////////////////////////////////////////////////////////////////////////////////////////////////////

Task("Fast")
    .IsDependentOn("externals")
    .IsDependentOn("libs")
    .IsDependentOn("nuget");

Task("Default")
    .IsDependentOn("externals")
    .IsDependentOn("libs")
    .IsDependentOn("nuget")
    .IsDependentOn("samples");

RunTarget (target);
