echo off

rem build the solution
echo Building the solution
msbuild binding\Square.sln /p:Configuration=Release /t:Rebuild

rem build the nuget
echo Packaging the NuGets
nuget pack nuget\Square.OkIO.nuspec
nuget pack nuget\Square.OkHttp.nuspec
nuget pack nuget\Square.Picasso.nuspec

rem build the components
echo Packaging the Components
xamarin-component package component\square.picasso
move component\square.picasso\*.xam .\