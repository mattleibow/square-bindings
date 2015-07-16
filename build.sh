#!/bin/bash

# download the files
echo Downloading binaries
curl -L http://search.maven.org/remotecontent?filepath=com/squareup/okio/okio/1.5.0/okio-1.5.0.jar > binding/Square.OkIO/Jars/okio-1.5.0.jar
curl -L http://search.maven.org/remotecontent?filepath=com/squareup/okhttp/okhttp/2.4.0/okhttp-2.4.0.jar > binding/Square.OkHttp/Jars/okhttp-2.4.0.jar
curl -L http://search.maven.org/remotecontent?filepath=com/squareup/okhttp/okhttp-ws/2.4.0/okhttp-ws-2.4.0.jar > binding/Square.OkHttp.WS/Jars/okhttp-ws-2.4.0.jar
curl -L http://search.maven.org/remotecontent?filepath=com/squareup/picasso/picasso/2.5.2/picasso-2.5.2.jar > binding/Square.Picasso/Jars/picasso-2.5.2.jar

# check out any files
echo Downloading source code
git clone https://github.com/square/SocketRocket.git binding/Square.SocketRocket/Archives/SocketRocket
(cd ./binding/Square.SocketRocket/Archives/SocketRocket &&
    git --git-dir=.git checkout 28719c9719cb2ca857f3130d343e3c5326eb2cc3)

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
(cd ./binding &&
    xbuild Square.sln /p:Configuration=Release /t:Rebuild

# build the nuget
echo Packaging the NuGets
(cd ./nuget &&
    nuget pack Square.OkIO.nuspec -BasePath ./ &&
    nuget pack Square.OkHttp.nuspec -BasePath ./ &&
    nuget pack Square.Picasso.nuspec -BasePath ./ &&
    nuget pack Square.OkHttp.WS.nuspec -BasePath ./ &&
    nuget pack Square.SocketRocket.nuspec -BasePath ./)

# build the components
echo Packaging the Components
(cd ./component &&
    xamarin-component package square.picasso &&
    mv square.picasso/*.xam ../ &&
    xamarin-component package square.okhttp &&
    mv square.okhttp/*.xam ../ &&
    xamarin-component package square.okhttp.ws &&
    mv square.okhttp.ws/*.xam ../ &&
    xamarin-component package square.socketrocket &&
    mv square.socketrocket/*.xam ../)
