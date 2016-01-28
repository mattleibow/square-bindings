using System;

using Foundation;
using UIKit;

using Square.Valet;

namespace ValetSample
{
	public partial class ViewController : UIViewController
	{
		private SecureEnclaveValet valet;
		private string username;

		public ViewController (IntPtr handle)
			: base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			valet = new SecureEnclaveValet ("Enclave-Id", AccessControl.UserPresence);
			username = "CustomerPresentProof";
		}

		partial void OnSetItem (UIButton sender)
		{
			var str = string.Format ("I am here! {0}", new NSUuid ().AsString ());
			var success = valet.SetString (str, username);

			textView.Text += (success ? "Set successful." : "Set failed.") + Environment.NewLine;
		}

		partial void OnGetItem (UIButton sender)
		{
			var password = valet.GetString (username, "Use TouchID to retreive password");

			textView.Text += string.Format ("Retrieved '{0}'.", password) + Environment.NewLine;
		}

		partial void OnRemoveItem (UIButton sender)
		{
			var success = valet.RemoveObject (username);

			textView.Text += (success ? "Removed item." : "Failure to remove item.") + Environment.NewLine;
		}
	}
}
