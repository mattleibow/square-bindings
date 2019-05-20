#!/usr/bin/env bash

#Make sure that CocoaPods is 1.5.3 version
echo 'Selecting CocoaPods...'
sudo gem uninstall cocoapods 
sudo gem install cocoapods -v 1.5.3