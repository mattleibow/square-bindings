#addin "Cake.Xamarin"
#addin "Cake.XCode"

#load "common.cake"


const string okio_version                 = "1.6.0"; // OkIO
const string okhttp_version               = "2.7.4"; // OkHttp
const string okhttp3_version              = "3.1.2"; // OkHttp3
const string okhttpws_version             = "2.7.4"; // OkHttp-WS
const string okhttp3ws_version            = "3.1.2"; // OkHttp3-WS
const string okhttpurlconnection_version  = "2.7.4"; // OkHttp-UrlConnection
const string picasso_version              = "2.5.2"; // Picasso
const string androidtimessquare_version   = "1.6.5"; // AndroidTimesSquare
const string socketrocket_version         = "0.4.2"; // SocketRocket
const string valet_version                = "2.2.0"; // Valet
const string aardvark_version             = "1.4.0"; // Aardvark
const string seismic_version              = "1.0.2"; // Seismic
const string pollexor_version             = "2.0.4"; // Pollexor
const string retrofit_version             = "1.9.0"; // Retrofit


CakeSpec.Libs = new ISolutionBuilder [] { 
    new IOSSolutionBuilder {
        IsWindowsCompatible = true, 
        Platform = "\"Any CPU\"",
        SolutionPath = "binding/Square.sln",
        OutputFiles = new [] { 
            new OutputFileCopy { FromFile = "binding/Square.OkIO/bin/Release/Square.OkIO.dll", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "binding/Square.OkHttp/bin/Release/Square.OkHttp.dll", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "binding/Square.OkHttp3/bin/Release/Square.OkHttp3.dll", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "binding/Square.Picasso/bin/Release/Square.Picasso.dll", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "binding/Square.OkHttp.WS/bin/Release/Square.OkHttp.WS.dll", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "binding/Square.OkHttp3.WS/bin/Release/Square.OkHttp3.WS.dll", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "binding/Square.OkHttp.UrlConnection/bin/Release/Square.OkHttp.UrlConnection.dll", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "binding/Square.SocketRocket/bin/Release/Square.SocketRocket.dll", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "binding/Square.AndroidTimesSquare/bin/Release/Square.AndroidTimesSquare.dll", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "binding/Square.Valet/bin/Release/Square.Valet.dll", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "binding/Square.Aardvark/bin/Release/Square.Aardvark.dll", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "binding/Square.Seismic/bin/Release/Square.Seismic.dll", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "binding/Square.Pollexor/bin/Release/Square.Pollexor.dll", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "README.md", ToDirectory = "output" },
            new OutputFileCopy { FromFile = "LICENSE.txt", ToDirectory = "output" },
        }
    },
};


CakeSpec.NuSpecs = new [] { 
	"nuget/Square.OkIO.nuspec",
	"nuget/Square.OkHttp.nuspec",
	"nuget/Square.OkHttp3.nuspec",
	"nuget/Square.Picasso.nuspec",
	"nuget/Square.OkHttp.WS.nuspec",
	"nuget/Square.OkHttp3.WS.nuspec",
	"nuget/Square.OkHttp.UrlConnection.nuspec",
	"nuget/Square.SocketRocket.nuspec",
	"nuget/Square.AndroidTimesSquare.nuspec",
	"nuget/Square.Valet.nuspec",
	"nuget/Square.Aardvark.nuspec",
	"nuget/Square.Seismic.nuspec",
	"nuget/Square.Pollexor.nuspec",
	"nuget/Square.Retrofit.nuspec",
};


CakeSpec.Samples = new ISolutionBuilder [] {
	new DefaultSolutionBuilder { IsWindowsCompatible = true, SolutionPath = "./sample/AndroidTimesSquareSample.sln" },
	new DefaultSolutionBuilder { IsWindowsCompatible = true, SolutionPath = "./sample/OkHttp3Sample.sln" },
	new DefaultSolutionBuilder { IsWindowsCompatible = true, SolutionPath = "./sample/OkHttpSample.sln" },
	new DefaultSolutionBuilder { IsWindowsCompatible = true, SolutionPath = "./sample/OkHttpWSSample.sln" },
	new DefaultSolutionBuilder { IsWindowsCompatible = true, SolutionPath = "./sample/OkHttp3WSSample.sln" },
	new DefaultSolutionBuilder { IsWindowsCompatible = true, SolutionPath = "./sample/PicassoSample.sln" },
	new DefaultSolutionBuilder { IsWindowsCompatible = true, SolutionPath = "./sample/PollexorSample.sln", RestoreComponents = true },
	new DefaultSolutionBuilder { IsWindowsCompatible = true, SolutionPath = "./sample/SeismicSample.sln" },
	new IOSSolutionBuilder { IsWindowsCompatible = true, SolutionPath = "./sample/AardvarkSample.sln" },
	new IOSSolutionBuilder { IsWindowsCompatible = true, SolutionPath = "./sample/SocketRocketSample.sln" },
	new IOSSolutionBuilder { IsWindowsCompatible = true, SolutionPath = "./sample/ValetSample.sln" },
};


if (IsRunningOnUnix ()) {
CakeSpec.GitRepositoryDependencies = new List<GitRepository> {
    new GitRepository { Path = "binding/Square.SocketRocket/Archives/SocketRocket/",   Url = "https://github.com/square/SocketRocket.git" },
    new GitRepository { Path = "binding/Square.Valet/Archives/Valet/",                 Url = "https://github.com/square/Valet.git" },
    new GitRepository { Path = "binding/Square.Aardvark/Archives/Aardvark/",           Url = "https://github.com/square/Aardvark.git" },
};
}


Task ("externals").IsDependentOn ("externals-base").Does (() => 
{
    // download pre-built files
    
    var source = string.Format("http://search.maven.org/remotecontent?filepath=com/squareup/okio/okio/{0}/okio-{0}.jar", okio_version);
    var destination = string.Format("binding/Square.OkIO/Jars/okio-{0}.jar", okio_version);
    if (!FileExists (destination)) DownloadFile (source, destination);
    
    source = string.Format("http://search.maven.org/remotecontent?filepath=com/squareup/okhttp/okhttp/{0}/okhttp-{0}.jar", okhttp_version);
    destination = string.Format("binding/Square.OkHttp/Jars/okhttp-{0}.jar", okhttp_version);
    if (!FileExists (destination)) DownloadFile (source, destination);
    
    source = string.Format("http://search.maven.org/remotecontent?filepath=com/squareup/okhttp3/okhttp/{0}/okhttp-{0}.jar", okhttp3_version);
    destination = string.Format("binding/Square.OkHttp3/Jars/okhttp-{0}.jar", okhttp3_version);
    if (!FileExists (destination)) DownloadFile (source, destination);
    
    source = string.Format("http://search.maven.org/remotecontent?filepath=com/squareup/okhttp/okhttp-ws/{0}/okhttp-ws-{0}.jar", okhttpws_version);
    destination = string.Format("binding/Square.OkHttp.WS/Jars/okhttp-ws-{0}.jar", okhttpws_version);
    if (!FileExists (destination)) DownloadFile (source, destination);
    
    source = string.Format("http://search.maven.org/remotecontent?filepath=com/squareup/okhttp3/okhttp-ws/{0}/okhttp-ws-{0}.jar", okhttp3ws_version);
    destination = string.Format("binding/Square.OkHttp3.WS/Jars/okhttp-ws-{0}.jar", okhttp3ws_version);
    if (!FileExists (destination)) DownloadFile (source, destination);

    source = string.Format("http://search.maven.org/remotecontent?filepath=com/squareup/okhttp/okhttp-urlconnection/{0}/okhttp-urlconnection-{0}.jar", okhttpurlconnection_version);
    destination = string.Format("binding/Square.OkHttp.UrlConnection/Jars/okhttp-urlconnection-{0}.jar", okhttpurlconnection_version);
    if (!FileExists (destination)) DownloadFile (source, destination);
    
    source = string.Format("http://search.maven.org/remotecontent?filepath=com/squareup/picasso/picasso/{0}/picasso-{0}.jar", picasso_version);
    destination = string.Format("binding/Square.Picasso/Jars/picasso-{0}.jar", picasso_version);
    if (!FileExists (destination)) DownloadFile (source, destination);
    
    source = string.Format("http://search.maven.org/remotecontent?filepath=com/squareup/android-times-square/{0}/android-times-square-{0}.aar", androidtimessquare_version);
    destination = string.Format("binding/Square.AndroidTimesSquare/Jars/android-times-square-{0}.aar", androidtimessquare_version);
    if (!FileExists (destination)) DownloadFile (source, destination);
    
    source = string.Format("http://search.maven.org/remotecontent?filepath=com/squareup/seismic/{0}/seismic-{0}.jar", seismic_version);
    destination = string.Format("binding/Square.Seismic/Jars/seismic-{0}.jar", seismic_version);
    if (!FileExists (destination)) DownloadFile (source, destination);
    
    source = string.Format("http://search.maven.org/remotecontent?filepath=com/squareup/pollexor/{0}/pollexor-{0}.jar", pollexor_version);
    destination = string.Format("binding/Square.Pollexor/Jars/pollexor-{0}.jar", pollexor_version);
    if (!FileExists (destination)) DownloadFile (source, destination);
    
    source = string.Format("http://search.maven.org/remotecontent?filepath=com/squareup/retrofit/retrofit/{0}/retrofit-{0}.jar", retrofit_version);
    destination = string.Format("binding/Square.Retrofit/Jars/retrofit-{0}.jar", retrofit_version);
    if (!FileExists (destination)) DownloadFile (source, destination);
    
    if (IsRunningOnUnix ()) {
    // checkout specific source versions
    
    RunGit ("binding/Square.SocketRocket/Archives/SocketRocket", "--git-dir=.git checkout " + socketrocket_version);
    
    RunGit ("binding/Square.Valet/Archives/Valet", "--git-dir=.git checkout " + valet_version);
    
    RunGit ("binding/Square.Aardvark/Archives/Aardvark", "--git-dir=.git checkout " + aardvark_version);
    
    // build native library source
    
    destination = string.Format("binding/Square.SocketRocket/Archives/libSocketRocket-{0}.a", socketrocket_version);
    source = "binding/Square.SocketRocket/Archives/SocketRocket/libSocketRocket.a";
    BuildXCode (
        project: "SocketRocket.xcodeproj", 
        target: "SocketRocket", 
        workingDirectory: "binding/Square.SocketRocket/Archives/SocketRocket");
    if (!FileExists (destination)) MoveFile (source, destination);
        
    destination = string.Format("binding/Square.Valet/Archives/libValet-{0}.a", valet_version);
    source = "binding/Square.Valet/Archives/Valet/libValet.a";
    BuildXCode (
        project: "Valet.xcodeproj", 
        target: "Valet iOS", 
        libraryTitle: "Valet", 
        workingDirectory: "binding/Square.Valet/Archives/Valet");
    if (!FileExists (destination)) MoveFile (source, destination);
        
    destination = string.Format("binding/Square.Aardvark/Archives/libAardvark-{0}.a", aardvark_version);
    source = "binding/Square.Aardvark/Archives/Aardvark/libAardvark.a";
    BuildXCode (
        project: "Aardvark.xcodeproj", 
        target: "Aardvark", 
        libraryTitle: "Aardvark", 
        workingDirectory: "binding/Square.Aardvark/Archives/Aardvark");
    if (!FileExists (destination)) MoveFile (source, destination);
    }
});


Task ("clean-native").IsDependentOn ("clean").Does (() => 
{
    CleanDirectories("binding/Square.OkIO/Jars");
    CleanDirectories("binding/Square.OkHttp/Jars");
    CleanDirectories("binding/Square.OkHttp3/Jars");
    CleanDirectories("binding/Square.OkHttp.WS/Jars");
    CleanDirectories("binding/Square.OkHttp3.WS/Jars");
    CleanDirectories("binding/Square.OkHttp.UrlConnection/Jars");
    CleanDirectories("binding/Square.Picasso/Jars");
    CleanDirectories("binding/Square.AndroidTimesSquare/Jars");
    CleanDirectories("binding/Square.Seismic/Jars");
    CleanDirectories("binding/Square.Pollexor/Jars");
    CleanDirectories("binding/Square.Retrofit/Jars");
    
    if (IsRunningOnUnix ()) {
    CleanDirectories("binding/Square.SocketRocket/Archives");
    CleanDirectories("binding/Square.Valet/Archives");
    CleanDirectories("binding/Square.Aardvark/Archives");
    }
});


Task ("clean").IsDependentOn ("clean-base").Does (() => 
{
    CleanDirectories("sample/Components");
    CleanDirectories("sample/packages");
});


DefineDefaultTasks ();

RunTarget (TARGET);
