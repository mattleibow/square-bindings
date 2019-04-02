#addin nuget:https://nuget.org/api/v2/?package=Cake.XCode&version=2.0.13
#addin nuget:https://nuget.org/api/v2/?package=Cake.FileHelpers&version=1.0.4
#addin nuget:https://nuget.org/api/v2/?package=Cake.Xamarin&version=1.3.0.15

using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default").ToUpper();

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

const string okio_version                 = "1.13.0"; // OkIO
const string okhttp_version               = "2.7.5"; // OkHttp
const string okhttp3_version              = "3.11.0"; // OkHttp3
const string okhttpws_version             = "2.7.5"; // OkHttp-WS
const string okhttp3ws_version            = "3.4.2"; // OkHttp3-WS
const string okhttpurlconnection_version  = "2.7.5"; // OkHttp-UrlConnection
const string picasso_version              = "2.5.2"; // Picasso
const string androidtimessquare_version   = "1.7.3"; // AndroidTimesSquare
const string socketrocket_version         = "0.5.1"; // SocketRocket
const string valet_version                = "2.4.1"; // Valet
const string aardvark_version             = "3.1.0"; // Aardvark
const string coreaardvark_version         = "2.1.0"; // CoreAardvark
const string seismic_version              = "1.0.2"; // Seismic
const string pollexor_version             = "2.0.4"; // Pollexor
const string retrofit_version             = "1.9.0"; // Retrofit
const string retrofit2_version            = "2.4.0"; // Retrofit2
const string convertergson_version        = "2.4.0"; // Converter Gson
const string adapterrxjava2_version       = "2.4.0"; // Adapter RxJava2 
const string picassookhttp_version        = "1.1.0"; // Picasso 2 OkHttp 3 Downloader

////////////////////////////////////////////////////////////////////////////////////////////////////
// TOOLS & FUNCTIONS - the bits to make it all work
////////////////////////////////////////////////////////////////////////////////////////////////////

var NugetToolPath = File ("./tools/nuget.exe");

Information (MakeAbsolute (File (".")).ToString ());
Information (MakeAbsolute (File ("./tools/nuget.exe")).ToString ());
Information (FileExists (MakeAbsolute (File ("./tools/nuget.exe"))).ToString ());

var RunNuGetRestore = new Action<FilePath> ((solution) =>
{
    NuGetRestore (solution, new NuGetRestoreSettings { 
        ToolPath = NugetToolPath
    });
});
 
var PackageNuGet = new Action<FilePath, DirectoryPath> ((nuspecPath, outputPath) =>
{
    if (!DirectoryExists (outputPath)) {
        CreateDirectory (outputPath);
    }

    NuGetPack (nuspecPath, new NuGetPackSettings { 
        Verbosity = NuGetVerbosity.Detailed,
        OutputDirectory = outputPath,
        BasePath = "./",
        ToolPath = NugetToolPath
    });
});

var RunLipo = new Action<DirectoryPath, FilePath, FilePath[]> ((directory, output, inputs) =>
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
});

var BuildXCode = new Action<FilePath, string, DirectoryPath, TargetOS> ((project, libraryTitle, workingDirectory, os) =>
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
        // not supported anymore
        // buildArch ("macosx", "i386", workingDirectory.CombineWithFilePath (i386));
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
});
var BuildDynamicXCode = new Action<FilePath, string, DirectoryPath, TargetOS> ((project, libraryTitle, workingDirectory, os) =>
{
    if (!IsRunningOnUnix ()) {
        return;
    }
    
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
});
var DownloadPod = new Action<bool, DirectoryPath, string, string, IDictionary<string, string>> ((isDynamic, podfilePath, platform, platformVersion, pods) => 
{
    if (!IsRunningOnUnix ()) {
        return;
    }
    
    if (!FileExists (podfilePath.CombineWithFilePath ("Podfile.lock"))) {
        var builder = new StringBuilder ();
        builder.AppendFormat ("platform :{0}, '{1}'", platform, platformVersion);
        builder.AppendLine ();
        if (CocoaPodVersion (new CocoaPodSettings ()) >= new System.Version (1, 0)) {
            builder.AppendLine ("install! 'cocoapods', :integrate_targets => false");
        }
        if (isDynamic) {
            builder.AppendLine ("use_frameworks!");
        }
        builder.AppendLine ("target 'Xamarin' do");
        foreach (var pod in pods) {
            builder.AppendFormat ("  pod '{0}', '{1}'", pod.Key, pod.Value);
            builder.AppendLine ();
        }
        builder.AppendLine ("end");
        builder.AppendLine ("post_install do |installer|");
        builder.AppendLine ("  installer.pods_project.targets.each do |target|");
        builder.AppendLine ("    target.build_configurations.each do |config|");
        builder.AppendLine ("      config.build_settings['SWIFT_VERSION'] = '3.0'");
        builder.AppendLine ("    end");
        builder.AppendLine ("  end");
        builder.AppendLine ("end");

        if (!DirectoryExists (podfilePath)) {
            CreateDirectory (podfilePath);
        }
        
        System.IO.File.WriteAllText (podfilePath.CombineWithFilePath ("Podfile").ToString (), builder.ToString ());
    
        CocoaPodInstall (podfilePath, new CocoaPodInstallSettings {
            NoIntegrate = true
        });
    }
});
var CreatePod = new Action<bool, DirectoryPath, string, string, string, IDictionary<string, string>> ((isDynamic, path, osxVersion, iosVersion, tvosVersion, pods) => {
    var name = pods.Keys.First ();
    var build = isDynamic ? BuildDynamicXCode : BuildXCode;

    if (osxVersion != null) {
        DownloadPod (isDynamic, 
                     path.Combine("osx"), 
                     "osx", osxVersion, 
                     pods);
        build ("Pods/Pods.xcodeproj", 
               name,
               path.Combine ("osx"),
               TargetOS.Mac);
    }
    if (iosVersion != null) {
        DownloadPod (isDynamic, 
                     path.Combine("ios"), 
                     "ios", iosVersion, 
                     pods);
        build ("Pods/Pods.xcodeproj", 
               name,
               path.Combine ("ios"),
               TargetOS.iOS);
    }
    if (tvosVersion != null) {
        DownloadPod (isDynamic, 
                     path.Combine("tvos"), 
                     "tvos", tvosVersion, 
                     pods);
        build ("Pods/Pods.xcodeproj", 
               name,
               path.Combine ("tvos"),
               TargetOS.tvOS);
    }
});

var DownloadJar = new Action<string, string, string> ((source, destination, version) =>
{
    source = string.Format("http://search.maven.org/remotecontent?filepath=" + source, version);
    FilePath dest = string.Format(destination, version);
    DirectoryPath destDir = dest.GetDirectory ();
    if (!DirectoryExists (destDir))
        CreateDirectory (destDir);
    if (!FileExists (dest)) {
        DownloadFile (source, dest);
    }
});

var Build = new Action<FilePath> ((solution) =>
{
    MSBuild (solution, s => s.SetConfiguration ("Release").SetMSBuildPlatform (MSBuildPlatform.x86));
});

////////////////////////////////////////////////////////////////////////////////////////////////////
// EXTERNALS - 
////////////////////////////////////////////////////////////////////////////////////////////////////

Task ("externals")
    .Does (() => 
{
    DownloadJar ("com/squareup/okio/okio/{0}/okio-{0}.jar",
                 "externals/OkIO/okio.jar",
                 okio_version);
    DownloadJar ("com/squareup/okhttp/okhttp/{0}/okhttp-{0}.jar",
                 "externals/OkHttp/okhttp.jar", 
                 okhttp_version);
    DownloadJar ("com/squareup/okhttp3/okhttp/{0}/okhttp-{0}.jar",
                 "externals/OkHttp3/okhttp3.jar",
                 okhttp3_version);
    DownloadJar ("com/squareup/okhttp/okhttp-ws/{0}/okhttp-ws-{0}.jar",
                 "externals/OkHttp.WS/okhttp-ws.jar",
                 okhttpws_version);
    DownloadJar ("com/squareup/okhttp3/okhttp-ws/{0}/okhttp-ws-{0}.jar",
                 "externals/OkHttp3.WS/okhttp3-ws.jar",
                 okhttp3ws_version);
    DownloadJar ("com/squareup/okhttp/okhttp-urlconnection/{0}/okhttp-urlconnection-{0}.jar",
                 "externals/OkHttp.UrlConnection/okhttp-urlconnection.jar", 
                 okhttpurlconnection_version);
    DownloadJar ("com/squareup/picasso/picasso/{0}/picasso-{0}.jar",
                 "externals/Picasso/picasso.jar", 
                 picasso_version);
    DownloadJar ("com/squareup/android-times-square/{0}/android-times-square-{0}.aar",
                 "externals/AndroidTimesSquare/android-times-square.aar", 
                 androidtimessquare_version);
    DownloadJar ("com/squareup/seismic/{0}/seismic-{0}.jar",
                 "externals/Seismic/seismic.jar", 
                 seismic_version);
    DownloadJar ("com/squareup/pollexor/{0}/pollexor-{0}.jar",
                 "externals/Pollexor/pollexor.jar", 
                 pollexor_version);
    DownloadJar ("com/squareup/retrofit/retrofit/{0}/retrofit-{0}.jar",
                 "externals/Retrofit/retrofit.jar", 
                 retrofit_version);
    DownloadJar ("com/squareup/retrofit2/retrofit/{0}/retrofit-{0}.jar",
                 "externals/Retrofit2/retrofit2.jar", 
                 retrofit2_version);
    DownloadJar ("com/squareup/retrofit2/converter-gson/{0}/converter-gson-{0}.jar",
                 "externals/Retrofit2/convertergson.jar", 
                 convertergson_version);
    DownloadJar ("com/squareup/retrofit2/adapter-rxjava2/{0}/adapter-rxjava2-{0}.jar",
                 "externals/Retrofit2/adapterrxjava2.jar", 
                 adapterrxjava2_version);
    DownloadJar ("com/jakewharton/picasso/picasso2-okhttp3-downloader/{0}/picasso2-okhttp3-downloader-{0}.jar",
                 "externals/Picasso2OkHttp3Downloader/picasso2-okhttp3-downloader.jar", 
                 picassookhttp_version);

    if (IsRunningOnUnix ()) {
        CreatePod (false, "externals/SocketRocket/", "10.8", "6.0", "9.0", new Dictionary<string, string> { { "SocketRocket", socketrocket_version } });
        CreatePod (false, "externals/Valet/", "10.10", "6.0", null, new Dictionary<string, string> { { "Valet", valet_version } });
        CreatePod (true, "externals/Aardvark/", null, "8.0", null, new Dictionary<string, string> { { "Aardvark", aardvark_version }, { "CoreAardvark", coreaardvark_version } });
        CreatePod (true, "externals/CoreAardvark/", null, "8.0", null, new Dictionary<string, string> { { "CoreAardvark", coreaardvark_version } });
    }
});

////////////////////////////////////////////////////////////////////////////////////////////////////
// LIBS - 
////////////////////////////////////////////////////////////////////////////////////////////////////

Task ("libs")
    .IsDependentOn ("externals")
    .Does (() => 
{
    FilePath solution = "./binding/Square.sln";
    if (IsRunningOnWindows ()) {
        solution = "./binding/Square.Windows.sln";
    }
    RunNuGetRestore (solution);
    Build (solution);
    
    var outputs = new List<string> {
        "Square.OkIO/bin/Release/Square.OkIO.dll",
        "Square.OkHttp/bin/Release/Square.OkHttp.dll",
        "Square.OkHttp3/bin/Release/Square.OkHttp3.dll",
        "Square.Picasso/bin/Release/Square.Picasso.dll",
        "Square.OkHttp.WS/bin/Release/Square.OkHttp.WS.dll",
        "Square.OkHttp3.WS/bin/Release/Square.OkHttp3.WS.dll",
        "Square.OkHttp.UrlConnection/bin/Release/Square.OkHttp.UrlConnection.dll",
        "Square.AndroidTimesSquare/bin/Release/Square.AndroidTimesSquare.dll",
        "Square.Seismic/bin/Release/Square.Seismic.dll",
        "Square.Pollexor/bin/Release/Square.Pollexor.dll",
        "Square.Retrofit/bin/Release/Square.Retrofit.dll",
        "Square.Retrofit2/bin/Release/Square.Retrofit2.dll",
        "Square.Retrofit2.ConverterGson/bin/Release/Square.Retrofit2.ConverterGson.dll",
        "Square.Retrofit2.AdapterRxJava2/bin/Release/Square.Retrofit2.AdapterRxJava2.dll",
        "JakeWharton.Picasso2OkHttp3Downloader/bin/Release/JakeWharton.Picasso2OkHttp3Downloader.dll",
    };
    if (IsRunningOnUnix ()) {
        outputs.Add ("Square.SocketRocket/bin/Release/Square.SocketRocket.dll");
        outputs.Add ("Square.SocketRocket-OSX/bin/Release/Square.SocketRocket.OSX.dll");
        outputs.Add ("Square.SocketRocket-TVOS/bin/Release/Square.SocketRocket.TVOS.dll");
        outputs.Add ("Square.Valet/bin/Release/Square.Valet.dll");
        outputs.Add ("Square.Aardvark/bin/Release/Square.Aardvark.dll");
        outputs.Add ("Square.CoreAardvark/bin/Release/Square.CoreAardvark.dll");
    }
    
    foreach (var output in outputs) {
        CopyFileToDirectory ("./binding/" + output, "./output/");
    }
    Zip("./output", "./output/dll.zip", "./output/*.dll");
    
    CopyFileToDirectory ("README.md", "./output/");
    CopyFileToDirectory ("LICENSE.txt", "./output/");
});

////////////////////////////////////////////////////////////////////////////////////////////////////
// PACKAGING - 
////////////////////////////////////////////////////////////////////////////////////////////////////

Task ("nuget")
    .IsDependentOn ("libs")
    .Does (() => 
{
    DeleteFiles ("./output/*.nupkg");
    var nugets = new List<string> {
        "./nuget/Square.AndroidTimesSquare.nuspec",
        "./nuget/Square.OkHttp.nuspec",
        "./nuget/Square.OkHttp.UrlConnection.nuspec",
        "./nuget/Square.OkHttp.WS.nuspec",
        "./nuget/Square.OkHttp3.nuspec",
        "./nuget/Square.OkHttp3.WS.nuspec",
        "./nuget/Square.OkIO.nuspec",
        "./nuget/Square.Picasso.nuspec",
        "./nuget/Square.Pollexor.nuspec",
        "./nuget/Square.Retrofit.nuspec",
        "./nuget/Square.Retrofit2.nuspec",
        "./nuget/Square.Retrofit2.ConverterGson.nuspec",
        "./nuget/Square.Retrofit2.AdapterRxJava2.nuspec",
        "./nuget/Square.Seismic.nuspec",
        "./nuget/JakeWharton.Picasso2OkHttp3Downloader.nuspec",
    };
    if (IsRunningOnUnix ()) {
        nugets.Add ("./nuget/Square.Aardvark.nuspec");
        nugets.Add ("./nuget/Square.CoreAardvark.nuspec");
        nugets.Add ("./nuget/Square.SocketRocket.nuspec");
        nugets.Add ("./nuget/Square.Valet.nuspec");
    }
    foreach (var nuget in nugets) {
        PackageNuGet (nuget, "./output/");
    }
    Zip("./output", "./output/nupkg.zip", "./output/*.nupkg");
});

Task ("component").IsDependentOn ("nuget");

////////////////////////////////////////////////////////////////////////////////////////////////////
// SAMPLES - 
////////////////////////////////////////////////////////////////////////////////////////////////////

Task ("samples")
    .IsDependentOn ("libs")
    .Does (() => 
{
    var samples = new List<string> {
        "./sample/AndroidTimesSquareSample/AndroidTimesSquareSample.sln",
        "./sample/OkHttp3Sample/OkHttp3Sample.sln",
        "./sample/OkHttp3WSSample/OkHttp3WSSample.sln",
        "./sample/OkHttpSample/OkHttpSample.sln",
        "./sample/OkHttpWSSample/OkHttpWSSample.sln",
        "./sample/PicassoSample/PicassoSample.sln",
        "./sample/PollexorSample/PollexorSample.sln",
        "./sample/SeismicSample/SeismicSample.sln",
    };
    if (IsRunningOnUnix ()) {
        samples.Add ("./sample/AardvarkSample/AardvarkSample.sln");
        samples.Add ("./sample/SocketRocketSample/SocketRocketSample.sln");
        samples.Add ("./sample/SocketRocketSample-OSX/SocketRocketSample-OSX.sln");
        samples.Add ("./sample/ValetSample/ValetSample.sln");
    }
    foreach (var sample in samples) {
        RunNuGetRestore (sample);
        Build (sample);
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
    .IsDependentOn("nuget");

Task("Default")
    .IsDependentOn("externals")
    .IsDependentOn("libs")
    .IsDependentOn("nuget")
    .IsDependentOn("samples");

RunTarget (target);
