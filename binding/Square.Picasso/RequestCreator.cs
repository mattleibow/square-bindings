using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Square.Picasso
{
    partial class RequestCreator
    {
        public virtual void Into (ImageView imageView, Action onSuccess, Action onError)
        {
            Into(imageView, new ActionCallback(onSuccess, onError));
        }

        private class ActionCallback : Java.Lang.Object, ICallback
        {
            private readonly Action onSuccess;
            private readonly Action onError;

            public ActionCallback(Action onSuccess, Action onError)
            {
                this.onSuccess = onSuccess;
                this.onError = onError;
            }
            public void OnSuccess()
            {
                if (onSuccess != null)
                {
                    onSuccess();
                }
            }

            public void OnError()
            {
                if (onError != null)
                {
                    onError();
                }
            }
        }
    }
}