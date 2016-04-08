#!/bin/bash

#
# Variables
#
export MonoVersion=4.4.0
export MonoTouchVersion=9.8.0.156
export XamarinMacVersion=2.8.0.58
export ActivatorVersion=1.0.1
export XamarinComponentVersion=1.1.0.29

#
# Encrypting the certificates and profiles
# 
#openssl aes-256-cbc -k "${CertificatePassword}" -in build-scripts/iOSDeveloper.mobileprovision -out build-scripts/iOSDeveloper.mobileprovision.enc -a
#openssl aes-256-cbc -k "${CertificatePassword}" -in build-scripts/iOSDeveloper.cer -out build-scripts/iOSDeveloper.cer.enc -a
#openssl aes-256-cbc -k "${CertificatePassword}" -in build-scripts/iOSDeveloper.p12 -out build-scripts/iOSDeveloper.p12.enc -a
#openssl aes-256-cbc -k "${CertificatePassword}" -in build-scripts/MacDeveloper.provisionprofile -out build-scripts/MacDeveloper.provisionprofile.enc -a
#openssl aes-256-cbc -k "${CertificatePassword}" -in build-scripts/MacDeveloper.cer -out build-scripts/MacDeveloper.cer.enc -a
#openssl aes-256-cbc -k "${CertificatePassword}" -in build-scripts/MacDeveloper.p12 -out build-scripts/MacDeveloper.p12.enc -a

#
# Decrypting the certificates and profiles
#
echo Decrypting the certificates and profiles
openssl aes-256-cbc -k "${CertificatePassword}" -in build-scripts/iOSDeveloper.mobileprovision.enc -d -a -out build-scripts/iOSDeveloper.mobileprovision
openssl aes-256-cbc -k "${CertificatePassword}" -in build-scripts/iOSDeveloper.cer.enc -d -a -out build-scripts/iOSDeveloper.cer
openssl aes-256-cbc -k "${CertificatePassword}" -in build-scripts/iOSDeveloper.p12.enc -d -a -out build-scripts/iOSDeveloper.p12
openssl aes-256-cbc -k "${CertificatePassword}" -in build-scripts/MacDeveloper.provisionprofile.enc -d -a -out build-scripts/MacDeveloper.provisionprofile
openssl aes-256-cbc -k "${CertificatePassword}" -in build-scripts/MacDeveloper.cer.enc -d -a -out build-scripts/MacDeveloper.cer
openssl aes-256-cbc -k "${CertificatePassword}" -in build-scripts/MacDeveloper.p12.enc -d -a -out build-scripts/MacDeveloper.p12

#
# Add the certificates to the keychain
#
echo Add the certificates to the keychain
security set-keychain-settings -t 3600 -l ~/Library/Keychains/login.keychain
# iOS
security import ./build-scripts/iOSDeveloper.cer -k ~/Library/Keychains/login.keychain -T /usr/bin/codesign
security import ./build-scripts/iOSDeveloper.p12 -k ~/Library/Keychains/login.keychain -T /usr/bin/codesign -P "${CertificatePassword}"
# Mac
security import ./build-scripts/MacDeveloper.cer -k ~/Library/Keychains/login.keychain -T /usr/bin/codesign
security import ./build-scripts/MacDeveloper.p12 -k ~/Library/Keychains/login.keychain -T /usr/bin/codesign -P "${CertificatePassword}"

#
# Put the provisioning profile in place
#
echo Put the provisioning profile in place
# iOS
mkdir -p ~/Library/MobileDevice/Provisioning\ Profiles
cp ./build-scripts/iOSDeveloper.mobileprovision ~/Library/MobileDevice/Provisioning\ Profiles/
[ -f ~/Library/MobileDevice/Provisioning\ Profiles/iOSDeveloper.mobileprovision ] && echo "Added provisioning profile" || echo "Error adding provisioning profile"
# Mac
profiles -i -F ./build-scripts/MacDeveloper.provisionprofile
profiles -c

#
# Download and install Mono and Xamarin
#
echo Download and install Mono and Xamarin
wget -nc -P downloads "http://download.mono-project.com/archive/${MonoVersion}/macos-10-universal/MonoFramework-MDK-${MonoVersion}.macos10.xamarin.universal.pkg"
sudo installer -pkg "downloads/MonoFramework-MDK-${MonoVersion}.macos10.xamarin.universal.pkg" -target / 
wget -nc -P downloads "http://download.xamarin.com/xm-binding-preview/xamarin.mac-${XamarinMacVersion}.pkg"
sudo installer -pkg "downloads/xamarin.mac-${XamarinMacVersion}.pkg" -target / 
wget -nc -P downloads "http://download.xamarin.com/MonoTouch/Mac/monotouch-${MonoTouchVersion}.pkg"
sudo installer -pkg "downloads/monotouch-${MonoTouchVersion}.pkg" -target /

#
# Activate the Xamarin license
#
echo Activate the Xamarin license
mkdir -p downloads
wget -nc -O "downloads/XamarinActivator-${ActivatorVersion}.nupkg" "https://www.nuget.org/api/v2/package/XamarinActivator/${ActivatorVersion}"
unzip -o -d downloads/XamarinActivator "downloads/XamarinActivator-${ActivatorVersion}.nupkg"
mono downloads/XamarinActivator/tools/XamarinActivator.exe activate -x ios -x mac -e "${XamarinEmail}" -p "${XamarinPassword}" 

#
# Login to the Xamarin Component Store
#
echo Login to the Xamarin Component Store
mkdir -p downloads
wget -nc -O "downloads/XamarinComponent-${XamarinComponentVersion}.nupkg" "https://www.nuget.org/api/v2/package/XamarinComponent/${XamarinComponentVersion}"
unzip -o -d downloads/XamarinComponent "downloads/XamarinComponent-${XamarinComponentVersion}.nupkg"
echo Y | mono downloads/XamarinComponent/tools/xamarin-component.exe login "${XamarinEmail}" -p "${XamarinPassword}" 
