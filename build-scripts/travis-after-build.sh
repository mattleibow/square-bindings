#!/bin/bash

#
# Remove the certificates
#
echo Remove the certificates
security delete-certificate -c "iPhone Developer" ~/Library/Keychains/login.keychain

#
# Remove the provisioning profile
#
echo Remove the provisioning profile
rm -f ~/Library/MobileDevice/Provisioning\ Profiles/iOSDeveloper.mobileprovision

#
# Deactivate the Xamarin license
# 
echo Deactivate the Xamarin license
mono downloads/XamarinActivator/tools/XamarinActivator.exe deactivate -x ios -e "${XamarinEmail}" -p "${XamarinPassword}"
