echo off

rem download the files

rem check out any files

rem build any native libraries

rem build the solution
echo Building the solution
msbuild binding\Square.sln /p:Configuration=Release /t:Rebuild

rem build the nuget
echo Packaging the NuGets
nuget pack nuget\Square.OkIO.nuspec
nuget pack nuget\Square.OkHttp.nuspec
nuget pack nuget\Square.Picasso.nuspec
nuget pack nuget\Square.OkHttp.WS.nuspec
nuget pack nuget\Square.SocketRocket.nuspec

rem build the components
echo Packaging the Components
xamarin-component package component\square.picasso
move component\square.picasso\*.xam .\
xamarin-component package component\square.okhttp
move component\square.okhttp\*.xam .\
xamarin-component package component\square.okhttp.ws
move component\square.okhttp.ws\*.xam .\
xamarin-component package component\square.socketrocket
move component\square.socketrocket\*.xam .\
