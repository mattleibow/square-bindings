#!/bin/bash

echo Setting up
# clean up before packaging
rm -rf build
rm -rf nuget/build
# make sure all the folders are in place
mkdir build

# download the files
echo Downloading binaries
if [ ! -f binding/Square.OkIO/Jars/okio-1.5.0.jar ]; then
    curl -L http://search.maven.org/remotecontent?filepath=com/squareup/okio/okio/1.5.0/okio-1.5.0.jar > binding/Square.OkIO/Jars/okio-1.5.0.jar
fi
if [ ! -f binding/Square.OkHttp/Jars/okhttp-2.4.0.jar ]; then
    curl -L http://search.maven.org/remotecontent?filepath=com/squareup/okhttp/okhttp/2.4.0/okhttp-2.4.0.jar > binding/Square.OkHttp/Jars/okhttp-2.4.0.jar
fi
if [ ! -f binding/Square.OkHttp.WS/Jars/okhttp-ws-2.4.0.jar ]; then
    curl -L http://search.maven.org/remotecontent?filepath=com/squareup/okhttp/okhttp-ws/2.4.0/okhttp-ws-2.4.0.jar > binding/Square.OkHttp.WS/Jars/okhttp-ws-2.4.0.jar
fi
if [ ! -f binding/Square.Picasso/Jars/picasso-2.5.2.jar ]; then
    curl -L http://search.maven.org/remotecontent?filepath=com/squareup/picasso/picasso/2.5.2/picasso-2.5.2.jar > binding/Square.Picasso/Jars/picasso-2.5.2.jar
fi

# check out any files
echo Downloading source code
if [ ! -d binding/Square.SocketRocket/Archives/SocketRocket/.git ]; then
    git clone https://github.com/square/SocketRocket.git binding/Square.SocketRocket/Archives/SocketRocket
    (cd ./binding/Square.SocketRocket/Archives/SocketRocket &&
        git --git-dir=.git checkout 28719c9719cb2ca857f3130d343e3c5326eb2cc3)
fi

# build any native libraries
echo Building native libraries
(cd ./binding/Square.SocketRocket/Archives/SocketRocket &&
    xcodebuild -project SocketRocket.xcodeproj -target SocketRocket -sdk iphonesimulator -arch i386 -configuration Release clean build &&
    mv build/Release-iphonesimulator/libSocketRocket.a libSocketRocket-i386.a &&
    xcodebuild -project SocketRocket.xcodeproj -target SocketRocket -sdk iphonesimulator -arch x86_64 -configuration Release clean build &&
    mv build/Release-iphonesimulator/libSocketRocket.a libSocketRocket-x86_64.a &&
    xcodebuild -project SocketRocket.xcodeproj -target SocketRocket -sdk iphoneos -arch armv7 -configuration Release clean build &&
    mv build/Release-iphoneos/libSocketRocket.a libSocketRocket-armv7.a &&
    xcodebuild -project SocketRocket.xcodeproj -target SocketRocket -sdk iphoneos -arch armv7s -configuration Release clean build &&
    mv build/Release-iphoneos/libSocketRocket.a libSocketRocket-armv7s.a &&
    xcodebuild -project SocketRocket.xcodeproj -target SocketRocket -sdk iphoneos -arch arm64 -configuration Release clean build &&
    mv build/Release-iphoneos/libSocketRocket.a libSocketRocket-arm64.a &&
    lipo -create libSocketRocket-i386.a libSocketRocket-x86_64.a libSocketRocket-armv7.a libSocketRocket-armv7s.a libSocketRocket-arm64.a -output libSocketRocket.a &&
    rm libSocketRocket-*.a &&
    mv libSocketRocket.a ../)

# build the solution
echo Building the solution
xbuild binding/Square.sln /p:Configuration=Release /t:Rebuild

# copy the output for NuGet (so that we can avoid Windows/Mac path issues)
echo Copying the output files packaging
mkdir nuget/build
cp README.md nuget/build
cp LICENSE.txt nuget/build
cp binding/Square.OkIO/bin/Release/Square.OkIO.dll nuget/build
cp binding/Square.OkHttp/bin/Release/Square.OkHttp.dll nuget/build
cp binding/Square.Picasso/bin/Release/Square.Picasso.dll nuget/build
cp binding/Square.OkHttp.WS/bin/Release/Square.OkHttp.WS.dll nuget/build
cp binding/Square.SocketRocket/bin/Release/Square.SocketRocket.dll nuget/build

# build the nuget
echo Packaging the NuGets
nuget pack nuget/Square.OkIO.nuspec -BasePath nuget -OutputDirectory build
nuget pack nuget/Square.OkHttp.nuspec -BasePath nuget -OutputDirectory build
nuget pack nuget/Square.Picasso.nuspec -BasePath nuget -OutputDirectory build
nuget pack nuget/Square.OkHttp.WS.nuspec -BasePath nuget -OutputDirectory build
nuget pack nuget/Square.SocketRocket.nuspec -BasePath nuget -OutputDirectory build

# build the components
echo Packaging the Components
xamarin-component package component/square.picasso
xamarin-component package component/square.okhttp
xamarin-component package component/square.okhttp.ws
xamarin-component package component/square.socketrocket

# move the files to the output location
echo Moving files to the build directory
mv component/square.picasso/*.xam build
mv component/square.okhttp.ws/*.xam build
mv component/square.okhttp/*.xam build
mv component/square.socketrocket/*.xam build

# clean any temporary files/folders
echo Cleaning up
rm -rf nuget/build
