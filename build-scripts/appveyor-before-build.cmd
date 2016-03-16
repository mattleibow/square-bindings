
REM
REM Variables
REM
set XamarinComponentVersion=1.1.0.29

REM
REM Link the Android SDK directory to one without spaces
REM
mklink /j "%ANDROID_HOME%" "C:\Program Files (x86)\Android\android-sdk"

REM
REM Install the tools, such as proguard
REM
echo y | "%ANDROID_HOME%\tools\android.bat" update sdk --no-ui --all --filter platform-tools,tools

REM
REM Install the Android 10 (2.3) platfom
REM
echo y | "%ANDROID_HOME%\tools\android.bat" update sdk --no-ui --all --filter android-10

REM
REM Install chocolates
REM
cinst wget
cinst 7zip.commandline

REM
REM Login to the Xamarin Component Store
REM
if not exist downloads (mkdir downloads)
wget -nc -O "downloads\XamarinComponent-%XamarinComponentVersion%.nupkg" "https://www.nuget.org/api/v2/package/XamarinComponent/%XamarinComponentVersion%"
7za -odownloads\XamarinComponent x "downloads\XamarinComponent-%XamarinComponentVersion%.nupkg"
echo Y | downloads\XamarinComponent\tools\xamarin-component.exe login "%XamarinEmail%" -p "%XamarinPassword%" 
