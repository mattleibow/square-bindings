echo off

echo Setting up
rem clean up before packaging
rmdir /Q /S build
rmdir /Q /S nuget\build
rem make sure all the folders are in place
mkdir build

rem download the files

rem check out any files

rem build any native libraries

rem build the solution
echo Building the solution
msbuild binding\Square.sln /p:Configuration=Release /t:Rebuild

rem copy the output for NuGet (so that we can avoid Windows/Mac path issues)
echo Copying the output files packaging
mkdir nuget\build
copy README.md nuget\build
copy LICENSE.txt nuget\build
copy binding\Square.OkIO\bin\Release\Square.OkIO.dll nuget\build
copy binding\Square.OkHttp\bin\Release\Square.OkHttp.dll nuget\build
copy binding\Square.Picasso\bin\Release\Square.Picasso.dll nuget\build
copy binding\Square.OkHttp.WS\bin\Release\Square.OkHttp.WS.dll nuget\build
copy binding\Square.SocketRocket\bin\Release\Square.SocketRocket.dll nuget\build

rem build the nuget
echo Packaging the NuGets
nuget pack nuget\Square.OkIO.nuspec -OutputDirectory build
nuget pack nuget\Square.OkHttp.nuspec -OutputDirectory build
nuget pack nuget\Square.Picasso.nuspec -OutputDirectory build
nuget pack nuget\Square.OkHttp.WS.nuspec -OutputDirectory build
nuget pack nuget\Square.SocketRocket.nuspec -OutputDirectory build

rem build the components
echo Packaging the Components
xamarin-component package component\square.picasso
xamarin-component package component\square.okhttp
xamarin-component package component\square.okhttp.ws
xamarin-component package component\square.socketrocket

rem move the files to the output location
echo Moving files to the build directory
move component\square.picasso\*.xam build
move component\square.okhttp.ws\*.xam build
move component\square.okhttp\*.xam build
move component\square.socketrocket\*.xam build

rem clean any temporary files/folders
echo Cleaning up
rmdir /Q /S nuget\build
