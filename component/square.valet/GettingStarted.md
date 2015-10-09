# Getting Started with Keychain Valet

> Valet lets you securely store data in the iOS or OS X Keychain without knowing a thing about how the 
> Keychain works. It’s easy. We promise.

## Usage

To begin storing data securely using Valet, we need to create a `Valet` instance:

    var myValet = new Valet("Druidia", Accessibility.WhenUnlocked);

This instance can be used to store and retrieve data securely, but only when the device is unlocked.

### Choosing the Best Accessibility Value

The `Accessibility` enum is used to determine when your secrets can be accessed. 
It’s a good idea to use the strictest accessibility possible that will allow our app to function.  

For example, if our app does not run in the background we will want to ensure the secrets can only 
be read when the phone is unlocked by using `Accessibility.WhenUnlocked` or 
`Accessibility.WhenUnlockedThisDeviceOnly`.

### Reading and Writing

Data can be read from and written to the Valet using various methods. 
There is a `GetString` and `SetString` pair for working with `string` values. And, there is 
a `GetObject` and `SetObject` pair for working with `NSData` values:

    var username = "Skroob";
	
    myValet.SetString("12345", username);
    var myLuggageCombination = myValet.GetString(username);

Valets created with a different class type, via a different initializer, or with a different 
identifier or accessibility attribute will not be able to read or modify values in other Valets.

### Sharing Secrets Among Multiple Applications

Data can be stored and retrieved securely across any app written by the same developer:

 First, add the value, `Druidia`, to the Keychain Access Groups section in the app’s 
`Entitlements.plist`. Then we an access that group in our code: 

    var mySharedValet = new Valet("Druidia", true, Accessibility.WhenUnlocked);

### Sharing Secrets Across Devices with iCloud

Data can be stored and retrieved securely across any app on other devices logged into the same 
iCloud account with iCloud Keychain enabled when using the `SynchronizableValet`:

    var mySynchronizableValet = new SynchronizableValet("Druidia", Accessibility.WhenUnlocked);

If iCloud Keychain is not enabled on this device, secrets can still be read and written, but will 
not sync to other devices.

### Protecting Secrets with Touch ID or iOS Passcode

Data can be stored and retrieved securely, requiring the user to confirm their presence via Touch 
ID or by entering their iOS passcode: 

    var mySecureEnclaveValet = new SecureEnclaveValet("Druidia");

This instance stores and retrieves data in the Secure Enclave (supported on iOS 8.0 or later). If 
no passcode is set on the device, this instance will be unable to access or store data. Data is 
removed from the Secure Enclave when the user removes a passcode from the device. Storing data 
using `SecureEnclaveValet` is the most secure way to store data on iOS.
