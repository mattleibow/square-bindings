using System;

namespace Square.Valet
{
	partial class Valet
	{
		protected Valet ()
		{
		}

		public Valet (string identifier, Accessibility accessibility, bool isSharedAccessGroupIdentifier = false)
		{
			Handle = isSharedAccessGroupIdentifier
				? InitWithSharedAccessGroupIdentifier (identifier, accessibility)
				: InitWithIdentifier (identifier, accessibility);
		}
	}

	partial class SecureEnclaveValet
	{
		protected SecureEnclaveValet ()
		{
		}

		[Obsolete ("Use backwards-compatible SecureEnclaveValet with AccessControl.UserPresence instead")]
		public SecureEnclaveValet (string identifier)
			: this (identifier, AccessControl.UserPresence)
		{
		}

		[Obsolete ("Use backwards-compatible SecureEnclaveValet with VAccessControl.UserPresence instead")]
		public SecureEnclaveValet (string identifier, Accessibility accessibility, bool isSharedAccessGroupIdentifier = false)
			: this (identifier, AccessControl.UserPresence, isSharedAccessGroupIdentifier)
		{
		}

		public SecureEnclaveValet (string identifier, AccessControl accessControl, bool isSharedAccessGroupIdentifier = false)
		{
			Handle = isSharedAccessGroupIdentifier
				? InitWithSharedAccessGroupIdentifier (identifier, accessControl)
				: InitWithIdentifier (identifier, accessControl);
		}
	}

	partial class SynchronizableValet
	{
		protected SynchronizableValet ()
		{
		}

		public SynchronizableValet (string identifier, Accessibility accessibility, bool isSharedAccessGroupIdentifier = false)
		{
			Handle = isSharedAccessGroupIdentifier
				? InitWithSharedAccessGroupIdentifier (identifier, accessibility)
				: InitWithIdentifier (identifier, accessibility);
		}
	}

	partial class SinglePromptSecureEnclaveValet
	{
		protected SinglePromptSecureEnclaveValet ()
		{
		}

		public SinglePromptSecureEnclaveValet (string identifier, AccessControl accessControl, bool isSharedAccessGroupIdentifier = false)
		{
			Handle = isSharedAccessGroupIdentifier
				? InitWithSharedAccessGroupIdentifier (identifier, accessControl)
				: InitWithIdentifier (identifier, accessControl);
		}
	}
}
