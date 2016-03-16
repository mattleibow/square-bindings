#!/bin/bash

#
# Remove the certificates
#
echo Remove the certificates
security delete-certificate -c "iPhone Developer" ~/Library/Keychains/login.keychain
security delete-certificate -c "Mac Developer" ~/Library/Keychains/login.keychain

#
# Remove the provisioning profile
#
echo Remove the provisioning profile
rm -f ~/Library/MobileDevice/Provisioning\ Profiles/iOSDeveloper.mobileprovision
profiles -r -u 166f1817-0df7-4b6e-8a91-ff67aba0f3f6
profiles -c

#
# Deactivate the Xamarin license
# 
echo Deactivate the Xamarin license
mono downloads/XamarinActivator/tools/XamarinActivator.exe deactivate -x ios -e "${XamarinEmail}" -p "${XamarinPassword}"
