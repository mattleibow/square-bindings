using System;
using ObjCRuntime;

namespace Square.Valet
{
	public enum Accessibility
	{
		/// <summary>
		/// Valet data can only be accessed while the device is unlocked. 
		/// </summary>
		/// <remarks>
		/// This attribute is recommended for data that only needs to be accessible while the application is in the foreground. 
		/// Valet data with this accessibility will migrate to a new device when using encrypted backups.
		/// </remarks>
		WhenUnlocked = 1,
		/// <summary>
		/// Valet data can only be accessed once the device has been unlocked after a restart. 
		/// </summary>
		/// <remarks>
		/// This attribute is recommended for data that needs to be accessible by background applications. 
		/// Valet data with this attribute will migrate to a new device when using encrypted backups.
		/// </remarks>
		AfterFirstUnlock,
		/// <summary>
		/// Valet data can always be accessed regardless of the lock state of the device. 
		/// </summary>
		/// <remarks>
		/// This attribute is not recommended. 
		/// Valet data with this attribute will migrate to a new device when using encrypted backups.
		/// </remarks>
		Always,
		/// <summary>
		/// Valet data can only be accessed while the device is unlocked. 
		/// </summary>
		/// <remarks>
		/// This class is only available if a passcode is set on the device. 
		/// This is recommended for items that only need to be accessible while the application is in the foreground. 
		/// Valet data with this attribute will never migrate to a new device, so these items will be missing after a backup is restored to a new device. 
		/// No items can be stored in this class on devices without a passcode. 
		/// Disabling the device passcode will cause all items in this class to be deleted.
		/// </remarks>
		WhenPasscodeSetThisDeviceOnly,
		/// <summary>
		/// Valet data can only be accessed while the device is unlocked. 
		/// </summary>
		/// <remarks>
		/// This is recommended for data that only needs to be accessible while the application is in the foreground. 
		/// Valet data with this attribute will never migrate to a new device, so these items will be missing after a backup is restored to a new device.
		/// </remarks>
		WhenUnlockedThisDeviceOnly,
		/// <summary>
		/// Valet data can only be accessed once the device has been unlocked after a restart. 
		/// </summary>
		/// <remarks>
		/// This is recommended for items that need to be accessible by background applications. 
		/// Valet data with this attribute will never migrate to a new device, so these items will be missing after a backup is restored to a new device.
		/// </remarks>
		AfterFirstUnlockThisDeviceOnly,
		/// <summary>
		/// Valet data can always be accessed regardless of the lock state of the device. 
		/// </summary>
		/// <remarks>
		/// This option is not recommended. 
		/// Valet data with this attribute will never migrate to a new device, so these items will be missing after a backup is restored to a new device.
		/// </remarks>
		AlwaysThisDeviceOnly
	}

	public enum MigrationError
	{
		/// <summary>
		/// Migration failed because the keychain query was not valid.
		/// </summary>
		InvalidQuery = 1,
		/// <summary>
		/// Migration failed because no items to migrate were found.
		/// </summary>
		NoItemsToMigrateFound,
		/// <summary>
		/// Migration failed because the keychain could not be read.
		/// </summary>
		CouldNotReadKeychain,
		/// <summary>
		/// Migration failed because a key in the query result could not be read.
		/// </summary>
		KeyInQueryResultInvalid,
		/// <summary>
		/// Migration failed because some data in the query result could not be read.
		/// </summary>
		DataInQueryResultInvalid,
		/// <summary>
		/// Migration failed because two keys with the same value were found in the keychain.
		/// </summary>
		DuplicateKeyInQueryResult,
		/// <summary>
		/// Migration failed because a key in the keychain duplicates a key already managed by Valet.
		/// </summary>
		KeyInQueryResultAlreadyExistsInValet,
		/// <summary>
		/// Migration failed because writing to the keychain failed.
		/// </summary>
		CouldNotWriteToKeychain,
		/// <summary>
		/// Migration failed because removing the migrated data from the keychain failed.
		/// </summary>
		RemovalFailed
	}
}
