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
var packagesToBuild = Argument ("id", "")?.Split (new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);
if (packagesToBuild?.Length > 0) {
    Information ($"Building {packagesToBuild.Length} project(s): " + string.Join (", ", packagesToBuild));
} else {
    packagesToBuild = null;
    Information ($"Building all the projects.");
}

EnsureDirectoryExists ("./output");

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

var versions = new Dictionary<string, string[]> {
    { "JakeWharton.Picasso2OkHttp3Downloader",   new [] { "1.1.0"  , "1.1.0.1"  }  },
    { "Square.Aardvark",                         new [] { "3.4.1"  , "3.4.1"    }  },
    { "Square.AndroidTimesSquare",               new [] { "1.7.3"  , "1.7.3.1"  }  },
    { "Square.CoreAardvark",                     new [] { "2.2.1"  , "2.2.1"    }  },
    { "Square.OkHttp.UrlConnection",             new [] { "2.7.5"  , "2.7.5.1"  }  },
    { "Square.OkHttp.WS",                        new [] { "2.7.5"  , "2.7.5.1"  }  },
    { "Square.OkHttp",                           new [] { "2.7.5"  , "2.7.5.1"  }  },
    { "Square.OkHttp3.WS",                       new [] { "3.4.2"  , "3.4.2.1"  }  },
    { "Square.OkHttp3.UrlConnection",            new [] { "3.12.3" , "3.12.3"   }  },
    { "Square.OkHttp3",                          new [] { "3.14.2" , "3.14.2"   }  },
    { "Square.OkIO",                             new [] { "2.4.0"  , "2.4.0"    }  },
    { "Square.Picasso",                          new [] { "2.5.2"  , "2.5.2.2"  }  },
    { "Square.Pollexor",                         new [] { "2.0.4"  , "2.0.4.1"  }  },
    { "Square.Retrofit",                         new [] { "1.9.0"  , "1.9.0.1"  }  },
    { "Square.Retrofit2.AdapterRxJava2",         new [] { "2.4.0"  , "2.4.0.1"  }  },
    { "Square.Retrofit2.ConverterGson",          new [] { "2.4.0"  , "2.4.0.1"  }  },
    { "Square.Retrofit2",                        new [] { "2.4.0"  , "2.4.0.1"  }  },
    { "Square.Seismic",                          new [] { "1.0.2"  , "1.0.2.1"  }  },
    { "Square.SocketRocket",                     new [] { "0.5.1"  , "0.5.1.1"  }  },
    { "Square.Valet",                            new [] { "2.4.1"  , "2.4.1.1"  }  },
};

var macOnly = new [] {
    "Square.SocketRocket",
    "Square.Valet",
    "Square.Aardvark",
    "Square.CoreAardvark",

    "AardvarkSample",
    "SocketRocketSample",
    "SocketRocketSample-OSX",
    "SocketRocketSample-TVOS",
    "ValetSample",
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

        if (!DirectoryExists (podfilePath))
            CreateDirectory (podfilePath);

        System.IO.File.WriteAllText (podfilePath.CombineWithFilePath ("Podfile").ToString (), builder.ToString ());

        CocoaPodInstall (podfilePath);
    }
}

void CreatePod (string packageId, bool isDynamic, string osxVersion, string iosVersion, string tvosVersion, params string[] podIds)
{
    if (packagesToBuild != null && packagesToBuild.All (x => !x.Equals (packageId, StringComparison.OrdinalIgnoreCase)))
        return;

    var pods = new Dictionary<string, string> ();
    foreach (var id in podIds) {
        pods [id] = versions ["Square." + id] [0];
    }

    var path = ((DirectoryPath)"./externals").Combine (packageId);
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
    var packageId = destination.GetDirectory ().GetDirectoryName ();
    if (packagesToBuild != null && packagesToBuild.All (x => !x.Equals (packageId, StringComparison.OrdinalIgnoreCase)))
        return;

    destination = ((DirectoryPath)"./externals").CombineWithFilePath (destination);

    var version = versions [packageId] [0];
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
    DownloadJar ("com/squareup/okhttp3/okhttp-urlconnection/{0}/okhttp-urlconnection-{0}.jar",
                 "Square.OkHttp3.UrlConnection/okhttp-urlconnection.jar");
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
    foreach (var file in GetFiles ("./binding/*/*.csproj")) {
        var id = file.GetFilenameWithoutExtension ().ToString ();
        if (packagesToBuild != null && packagesToBuild.All (x => !x.Equals (id, StringComparison.OrdinalIgnoreCase)))
            continue;

        var version = Version.Parse (versions [id] [0]);
        var assemblyVersion = $"{version.Major}.0.0.0";
        var fileVersion     = $"{version.Major}.{version.Minor}.{version.Build}.0";
        var infoVersion     = versions [id] [1];
        var packageVersion  = versions [id] [1];

        XmlPoke(file, "/Project/PropertyGroup/Version", assemblyVersion);
        XmlPoke(file, "/Project/PropertyGroup/FileVersion", fileVersion);
        XmlPoke(file, "/Project/PropertyGroup/InformationalVersion", fileVersion);
        XmlPoke(file, "/Project/PropertyGroup/PackageVersion", packageVersion);
    }

    foreach (var file in GetFiles ("./binding/*/*.csproj")) {
        var id = file.GetFilenameWithoutExtension ().ToString ();
        if (packagesToBuild != null && packagesToBuild.All (x => !x.Equals (id, StringComparison.OrdinalIgnoreCase)))
            continue;

        if (IsRunningOnWindows () && macOnly.Contains (id))
            continue;

        var settings = new MSBuildSettings ()
            .SetConfiguration (configuration)
            .SetVerbosity (Verbosity.Minimal)
            .WithRestore ()
            // .WithProperty ("IncludeSymbols", "true")
            .WithProperty ("DesignTimeBuild", "false")
            .WithProperty ("PackageOutputPath", MakeAbsolute ((DirectoryPath)"./output/").FullPath)
            .WithTarget ("Pack");

        MSBuild (file, settings);
    }
});

////////////////////////////////////////////////////////////////////////////////////////////////////
// PACKAGING -
////////////////////////////////////////////////////////////////////////////////////////////////////

Task ("nuget")
    .IsDependentOn ("libs")
    .Does (() =>
{
    EnsureDirectoryExists ("./output/new/");

    var newCount = 0;

    foreach (var package in versions) {
        var id = package.Key;
        if (packagesToBuild != null && packagesToBuild.All (x => !x.Equals (id, StringComparison.OrdinalIgnoreCase)))
            continue;

        var version = package.Value [1];

        Information ($"Checking for {version} of {id}...");
        try {
            DownloadFile ($"https://api.nuget.org/v3/registration3/{id.ToLower ()}/{version}.json");
            Information ($"Version {version} already published.");
        } catch {
            Information ($"No matching versions found, copying...");
            newCount++;
            CopyFileToDirectory ($"./output/{id}.{version}.nupkg", "./output/new/");
        }
    }

    if (newCount == 0)
        Warning ("No new package versions built.");
});

Task ("component")
    .IsDependentOn ("nuget");

Task ("diff")
    .IsDependentOn ("nuget")
    .Does (() =>
{
    var res = StartProcess("api-tools",
        "nuget-diff ./output --latest --group-ids --ignore-unchanged --output ./output/api-diff --cache ./externals/package_cache");

    if (res != 0)
        throw new Exception($"Process api-tools exited with code {res}.");
});

////////////////////////////////////////////////////////////////////////////////////////////////////
// SAMPLES -
////////////////////////////////////////////////////////////////////////////////////////////////////

Task ("samples")
    .Does (() =>
{
    foreach (var file in GetFiles ("./sample/*/*.sln")) {
        var id = file.GetFilenameWithoutExtension ().ToString ();

        if (IsRunningOnWindows () && macOnly.Contains (id))
            continue;

        var settings = new MSBuildSettings ()
            .SetConfiguration (configuration)
            .SetVerbosity (Verbosity.Minimal)
            .WithRestore ()
            .WithProperty ("DesignTimeBuild", "false");

        MSBuild (file, settings);
    }
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
    .IsDependentOn("nuget")
    .IsDependentOn("diff");

Task("Default")
    .IsDependentOn("externals")
    .IsDependentOn("libs")
    .IsDependentOn("nuget")
    .IsDependentOn("diff")
    .IsDependentOn("samples");

RunTarget (target);
