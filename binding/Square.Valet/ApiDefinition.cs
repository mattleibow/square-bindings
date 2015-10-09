using System;

using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace Square.Valet
{
	// @interface VALValet : NSObject <NSCopying>
	[BaseType (typeof(NSObject), Name = "VALValet")]
	[DisableDefaultCtor]
	interface Valet : INSCopying
	{
		// extern NSString *const _Nonnull VALMigrationErrorDomain;
		[Static]
		[Field ("VALMigrationErrorDomain", "__Internal")]
		NSString VALMigrationErrorDomain { get; }

		// -(instancetype _Nullable)initWithIdentifier:(NSString * _Nonnull)identifier accessibility:(VALAccessibility)accessibility __attribute__((objc_designated_initializer));
		[Export ("initWithIdentifier:accessibility:")]
		[Internal]
		IntPtr InitWithIdentifier (string identifier, Accessibility accessibility);

		// -(instancetype _Nullable)initWithSharedAccessGroupIdentifier:(NSString * _Nonnull)sharedAccessGroupIdentifier accessibility:(VALAccessibility)accessibility __attribute__((objc_designated_initializer));
		[Export ("initWithSharedAccessGroupIdentifier:accessibility:")]
		[Internal]
		IntPtr InitWithSharedAccessGroupIdentifier (string sharedAccessGroupIdentifier, Accessibility accessibility);

		// @property (readonly, copy) NSString * _Nonnull identifier;
		[Export ("identifier")]
		string Identifier { get; }

		// @property (readonly, getter = isSharedAcrossApplications) BOOL sharedAcrossApplications;
		[Export ("sharedAcrossApplications")]
		bool SharedAcrossApplications { [Bind ("isSharedAcrossApplications")] get; }

		// @property (readonly) VALAccessibility accessibility;
		[Export ("accessibility")]
		Accessibility Accessibility { get; }

		// -(BOOL)isEqualToValet:(VALValet * _Nonnull)otherValet;
		[Export ("isEqualToValet:")]
		bool IsEqualTo (Valet otherValet);

		// -(BOOL)canAccessKeychain;
		[Export ("canAccessKeychain")]
		bool CanAccessKeychain { get; }

		// -(BOOL)setObject:(NSData * _Nonnull)value forKey:(NSString * _Nonnull)key;
		[Export ("setObject:forKey:")]
		bool SetObject (NSData value, string key);

		// -(NSData * _Nullable)objectForKey:(NSString * _Nonnull)key;
		[Export ("objectForKey:")]
		[return: NullAllowed]
		NSData GetObject (string key);

		// -(BOOL)setString:(NSString * _Nonnull)string forKey:(NSString * _Nonnull)key;
		[Export ("setString:forKey:")]
		bool SetString (string @string, string key);

		// -(NSString * _Nullable)stringForKey:(NSString * _Nonnull)key;
		[Export ("stringForKey:")]
		[return: NullAllowed]
		string GetString (string key);

		// -(BOOL)containsObjectForKey:(NSString * _Nonnull)key;
		[Export ("containsObjectForKey:")]
		bool ContainsObject (string key);

		// -(NSSet * _Nonnull)allKeys;
		[Export ("allKeys")]
		NSSet AllKeys { get; }

		// -(BOOL)removeObjectForKey:(NSString * _Nonnull)key;
		[Export ("removeObjectForKey:")]
		bool RemoveObject (string key);

		// -(BOOL)removeAllObjects;
		[Export ("removeAllObjects")]
		bool RemoveAllObjects { get; }

		// -(NSError * _Nullable)migrateObjectsMatchingQuery:(NSDictionary * _Nonnull)secItemQuery removeOnCompletion:(BOOL)remove;
		[Export ("migrateObjectsMatchingQuery:removeOnCompletion:")]
		[return: NullAllowed]
		NSError MigrateObjectsMatchingQuery (NSDictionary secItemQuery, bool remove);

		// -(NSError * _Nullable)migrateObjectsFromValet:(VALValet * _Nonnull)valet removeOnCompletion:(BOOL)remove;
		[Export ("migrateObjectsFromValet:removeOnCompletion:")]
		[return: NullAllowed]
		NSError MigrateObjectsFromValet (Valet valet, bool remove);
	}

	// @interface VALSecureEnclaveValet : VALValet
	[iOS (8, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof(Valet), Name = "VALSecureEnclaveValet")]
	interface SecureEnclaveValet
	{
		// +(BOOL)supportsSecureEnclaveKeychainItems;
		[Static]
		[Export ("supportsSecureEnclaveKeychainItems")]
		bool SupportsSecureEnclaveKeychainItems { get; }

		// -(instancetype _Nullable)initWithIdentifier:(NSString * _Nonnull)identifier accessibility:(VALAccessibility)accessibility __attribute__((objc_designated_initializer));
		[Export ("initWithIdentifier:accessibility:")]
		[Internal, New, Sealed]
		IntPtr InitWithIdentifier (string identifier, Accessibility accessibility);

		// -(instancetype _Nullable)initWithSharedAccessGroupIdentifier:(NSString * _Nonnull)sharedAccessGroupIdentifier accessibility:(VALAccessibility)accessibility __attribute__((objc_designated_initializer));
		[Export ("initWithSharedAccessGroupIdentifier:accessibility:")]
		[Internal, New, Sealed]
		IntPtr InitWithSharedAccessGroupIdentifier (string sharedAccessGroupIdentifier, Accessibility accessibility);

		// -(instancetype _Nullable)initWithIdentifier:(NSString * _Nonnull)identifier;
		[Export ("initWithIdentifier:")]
		[Internal, Sealed]
		IntPtr InitWithIdentifier (string identifier);

		// -(instancetype _Nullable)initWithSharedAccessGroupIdentifier:(NSString * _Nonnull)sharedAccessGroupIdentifier;
		[Export ("initWithSharedAccessGroupIdentifier:")]
		[Internal, Sealed]
		IntPtr InitWithSharedAccessGroupIdentifier (string sharedAccessGroupIdentifier);

		// -(NSData * _Nullable)objectForKey:(NSString * _Nonnull)key userPrompt:(NSString * _Nonnull)userPrompt;
		[Export ("objectForKey:userPrompt:")]
		[return: NullAllowed]
		NSData GetObject (string key, string userPrompt);

		// -(NSString * _Nullable)stringForKey:(NSString * _Nonnull)key userPrompt:(NSString * _Nonnull)userPrompt;
		[Export ("stringForKey:userPrompt:")]
		[return: NullAllowed]
		string GetString (string key, string userPrompt);
	}

	// @interface VALSynchronizableValet : VALValet
	[DisableDefaultCtor]
	[BaseType (typeof(Valet), Name = "VALSynchronizableValet")]
	interface SynchronizableValet
	{
		// -(instancetype _Nullable)initWithIdentifier:(NSString * _Nonnull)identifier accessibility:(VALAccessibility)accessibility __attribute__((objc_designated_initializer));
		[Export ("initWithIdentifier:accessibility:")]
		[Internal, New, Sealed]
		IntPtr InitWithIdentifier (string identifier, Accessibility accessibility);

		// -(instancetype _Nullable)initWithSharedAccessGroupIdentifier:(NSString * _Nonnull)sharedAccessGroupIdentifier accessibility:(VALAccessibility)accessibility __attribute__((objc_designated_initializer));
		[Export ("initWithSharedAccessGroupIdentifier:accessibility:")]
		[Internal, New, Sealed]
		IntPtr InitWithSharedAccessGroupIdentifier (string sharedAccessGroupIdentifier, Accessibility accessibility);

		// +(BOOL)supportsSynchronizableKeychainItems;
		[Static]
		[Export ("supportsSynchronizableKeychainItems")]
		bool SupportsSynchronizableKeychainItems { get; }
	}
}
