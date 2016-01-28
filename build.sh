#!/bin/bash

okio_version=1.6.0
okhttp_version=2.5.0
okhttp3_version=3.0.1
okhttpws_version=2.5.0
picasso_version=2.5.2
androidtimessquare_version=1.6.4
socketrocket_version=0.4.1
valet_version=2.0.3
aardvark_version=1.2.2


echo Setting up
# clean up before packaging
rm -rf build
rm -rf nuget/build
# make sure all the folders are in place
mkdir build


# download the files
echo Downloading binaries
if [ ! -f binding/Square.OkIO/Jars/okio-$okio_version.jar ]; then
    curl -L http://search.maven.org/remotecontent?filepath=com/squareup/okio/okio/$okio_version/okio-$okio_version.jar > binding/Square.OkIO/Jars/okio-$okio_version.jar
fi
if [ ! -f binding/Square.OkHttp/Jars/okhttp-$okhttp_version.jar ]; then
    curl -L http://search.maven.org/remotecontent?filepath=com/squareup/okhttp/okhttp/$okhttp_version/okhttp-$okhttp_version.jar > binding/Square.OkHttp/Jars/okhttp-$okhttp_version.jar
fi
if [ ! -f binding/Square.OkHttp3/Jars/okhttp-$okhttp3_version.jar ]; then
    curl -L http://search.maven.org/remotecontent?filepath=com/squareup/okhttp3/okhttp/$okhttp3_version/okhttp-$okhttp3_version.jar > binding/Square.OkHttp3/Jars/okhttp-$okhttp3_version.jar
fi
if [ ! -f binding/Square.OkHttp.WS/Jars/okhttp-ws-$okhttpws_version.jar ]; then
    curl -L http://search.maven.org/remotecontent?filepath=com/squareup/okhttp/okhttp-ws/$okhttpws_version/okhttp-ws-$okhttpws_version.jar > binding/Square.OkHttp.WS/Jars/okhttp-ws-$okhttpws_version.jar
fi
if [ ! -f binding/Square.Picasso/Jars/picasso-$picasso_version.jar ]; then
    curl -L http://search.maven.org/remotecontent?filepath=com/squareup/picasso/picasso/$picasso_version/picasso-$picasso_version.jar > binding/Square.Picasso/Jars/picasso-$picasso_version.jar
fi
if [ ! -f binding/Square.AndroidTimesSquare/Jars/android-times-square-$androidtimessquare_version.aar ]; then
    curl -L http://search.maven.org/remotecontent?filepath=com/squareup/android-times-square/$androidtimessquare_version/android-times-square-$androidtimessquare_version.aar > binding/Square.AndroidTimesSquare/Jars/android-times-square-$androidtimessquare_version.aar
fi


# check out any files
echo Downloading source code
if [ ! -d binding/Square.SocketRocket/Archives/SocketRocket/.git ]; then
    git clone https://github.com/square/SocketRocket.git binding/Square.SocketRocket/Archives/SocketRocket
    (cd ./binding/Square.SocketRocket/Archives/SocketRocket &&
        git --git-dir=.git checkout $socketrocket_version)
fi
if [ ! -d binding/Square.Valet/Archives/Valet/.git ]; then
    git clone https://github.com/square/Valet.git binding/Square.Valet/Archives/Valet
    (cd ./binding/Square.Valet/Archives/Valet &&
        git --git-dir=.git checkout $valet_version)
fi
if [ ! -d binding/Square.Aardvark/Archives/Aardvark/.git ]; then
    git clone https://github.com/square/Aardvark.git binding/Square.Aardvark/Archives/Aardvark
    (cd ./binding/Square.Aardvark/Archives/Aardvark &&
        git --git-dir=.git checkout $aardvark_version)
fi


# build any native libraries
echo Building native libraries
if [ ! -f binding/Square.SocketRocket/Archives/libSocketRocket-$socketrocket_version.a ]; then
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
    mv libSocketRocket.a ../libSocketRocket-$socketrocket_version.a)
fi
if [ ! -f binding/Square.Valet/Archives/libValet-$valet_version.a ]; then
  (cd ./binding/Square.Valet/Archives/Valet &&
    xcodebuild -project Valet.xcodeproj -target "Valet iOS" -sdk iphonesimulator -arch i386 -configuration Release clean build &&
    mv build/Release-iphonesimulator/libValet.a libValet-i386.a &&
    xcodebuild -project Valet.xcodeproj -target "Valet iOS" -sdk iphonesimulator -arch x86_64 -configuration Release clean build &&
    mv build/Release-iphonesimulator/libValet.a libValet-x86_64.a &&
    xcodebuild -project Valet.xcodeproj -target "Valet iOS" -sdk iphoneos -arch armv7 -configuration Release clean build &&
    mv build/Release-iphoneos/libValet.a libValet-armv7.a &&
    xcodebuild -project Valet.xcodeproj -target "Valet iOS" -sdk iphoneos -arch armv7s -configuration Release clean build &&
    mv build/Release-iphoneos/libValet.a libValet-armv7s.a &&
    xcodebuild -project Valet.xcodeproj -target "Valet iOS" -sdk iphoneos -arch arm64 -configuration Release clean build &&
    mv build/Release-iphoneos/libValet.a libValet-arm64.a &&
    lipo -create libValet-i386.a libValet-x86_64.a libValet-armv7.a libValet-armv7s.a libValet-arm64.a -output libValet.a &&
    rm libValet-*.a &&
    mv libValet.a ../libValet-$valet_version.a)
fi
if [ ! -f binding/Square.Aardvark/Archives/libAardvark-$aardvark_version.a ]; then
  (cd ./binding/Square.Aardvark/Archives/Aardvark &&
    xcodebuild -project Aardvark.xcodeproj -target "Aardvark" -sdk iphonesimulator -arch i386 -configuration Release clean build &&
    mv build/Release-iphonesimulator/libAardvark.a libAardvark-i386.a &&
    xcodebuild -project Aardvark.xcodeproj -target "Aardvark" -sdk iphonesimulator -arch x86_64 -configuration Release clean build &&
    mv build/Release-iphonesimulator/libAardvark.a libAardvark-x86_64.a &&
    xcodebuild -project Aardvark.xcodeproj -target "Aardvark" -sdk iphoneos -arch armv7 -configuration Release clean build &&
    mv build/Release-iphoneos/libAardvark.a libAardvark-armv7.a &&
    xcodebuild -project Aardvark.xcodeproj -target "Aardvark" -sdk iphoneos -arch armv7s -configuration Release clean build &&
    mv build/Release-iphoneos/libAardvark.a libAardvark-armv7s.a &&
    xcodebuild -project Aardvark.xcodeproj -target "Aardvark" -sdk iphoneos -arch arm64 -configuration Release clean build &&
    mv build/Release-iphoneos/libAardvark.a libAardvark-arm64.a &&
    lipo -create libAardvark-i386.a libAardvark-x86_64.a libAardvark-armv7.a libAardvark-armv7s.a libAardvark-arm64.a -output libAardvark.a &&
    rm libAardvark-*.a &&
    mv libAardvark.a ../libAardvark-$aardvark_version.a)
fi

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
cp binding/Square.OkHttp3/bin/Release/Square.OkHttp3.dll nuget/build
cp binding/Square.Picasso/bin/Release/Square.Picasso.dll nuget/build
cp binding/Square.OkHttp.WS/bin/Release/Square.OkHttp.WS.dll nuget/build
cp binding/Square.SocketRocket/bin/Release/Square.SocketRocket.dll nuget/build
cp binding/Square.AndroidTimesSquare/bin/Release/Square.AndroidTimesSquare.dll nuget/build
cp binding/Square.Valet/bin/Release/Square.Valet.dll nuget/build
cp binding/Square.Aardvark/bin/Release/Square.Aardvark.dll nuget/build


# build the nuget
echo Packaging the NuGets
nuget pack nuget/Square.OkIO.nuspec -BasePath nuget -OutputDirectory build
nuget pack nuget/Square.OkHttp.nuspec -BasePath nuget -OutputDirectory build
nuget pack nuget/Square.OkHttp3.nuspec -BasePath nuget -OutputDirectory build
nuget pack nuget/Square.Picasso.nuspec -BasePath nuget -OutputDirectory build
nuget pack nuget/Square.OkHttp.WS.nuspec -BasePath nuget -OutputDirectory build
nuget pack nuget/Square.SocketRocket.nuspec -BasePath nuget -OutputDirectory build
nuget pack nuget/Square.AndroidTimesSquare.nuspec -BasePath nuget -OutputDirectory build
nuget pack nuget/Square.Valet.nuspec -BasePath nuget -OutputDirectory build
nuget pack nuget/Square.Aardvark.nuspec -BasePath nuget -OutputDirectory build


# build the components
echo Packaging the Components
xamarin-component package component/square.picasso
xamarin-component package component/square.okhttp
xamarin-component package component/square.okhttp3
xamarin-component package component/square.okhttp.ws
xamarin-component package component/square.socketrocket
xamarin-component package component/square.androidtimessquare
xamarin-component package component/square.valet
xamarin-component package component/square.aardvark


# move the files to the output location
echo Moving files to the build directory
mv component/square.picasso/*.xam build
mv component/square.okhttp.ws/*.xam build
mv component/square.okhttp/*.xam build
mv component/square.okhttp3/*.xam build
mv component/square.socketrocket/*.xam build
mv component/square.androidtimessquare/*.xam build
mv component/square.valet/*.xam build
mv component/square.aardvark/*.xam build


# clean any temporary files/folders
echo Cleaning up
rm -rf nuget/build
