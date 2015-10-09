using System;

namespace Square.Valet
{
	partial class Valet
	{
		protected Valet()
		{
		}

		public Valet (string identifier, Accessibility accessibility)
			: this (identifier, false, accessibility)
		{
		}

		public Valet (string identifier, bool isSharedAccessGroupIdentifier, Accessibility accessibility)
		{
			Handle = isSharedAccessGroupIdentifier
				? InitWithSharedAccessGroupIdentifier (identifier, accessibility)
				: InitWithIdentifier (identifier, accessibility);
		}
	}

	partial class SecureEnclaveValet
	{
		public SecureEnclaveValet (string identifier, Accessibility accessibility)
			: this (identifier, false, accessibility)
		{
		}

		public SecureEnclaveValet (string identifier, bool isSharedAccessGroupIdentifier, Accessibility accessibility)
			: base (identifier, isSharedAccessGroupIdentifier, accessibility)
		{
		}

		public SecureEnclaveValet (string identifier)
			: this (identifier, false)
		{
		}

		public SecureEnclaveValet (string identifier, bool isSharedAccessGroupIdentifier)
		{
			Handle = isSharedAccessGroupIdentifier
				? InitWithSharedAccessGroupIdentifier (identifier)
				: InitWithIdentifier (identifier);
		}
	}

	partial class SynchronizableValet
	{
		public SynchronizableValet (string identifier, Accessibility accessibility)
			: this (identifier, false, accessibility)
		{
		}

		public SynchronizableValet (string identifier, bool isSharedAccessGroupIdentifier, Accessibility accessibility)
		{
			Handle = isSharedAccessGroupIdentifier
				? InitWithSharedAccessGroupIdentifier (identifier, accessibility)
				: InitWithIdentifier (identifier, accessibility);
		}
	}
}
