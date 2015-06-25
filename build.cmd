echo off

rem build the solution
echo Building the solution
msbuild binding\Square.sln /p:Configuration=Release /t:Rebuild

rem build the nuget
echo Packaging the NuGet
nuget pack nuget\Square.OkIO.nuspec
nuget pack nuget\Square.OkHttp.nuspec
nuget pack nuget\Square.Picasso.nuspec
