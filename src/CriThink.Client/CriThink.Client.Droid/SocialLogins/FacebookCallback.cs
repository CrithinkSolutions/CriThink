using System;
using Android.Runtime;
using Xamarin.Facebook;
using Exception = Java.Lang.Exception;

namespace CriThink.Client.Droid.SocialLogins
{
    public class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
    {
        public Action HandleCancel { get; set; }

        public Action<FacebookException> HandleError { get; set; }

        public Action<TResult> HandleSuccess { get; set; }

        public void OnCancel()
        {
            HandleCancel?.Invoke();
        }

        public void OnError(FacebookException error)
        {
            HandleError?.Invoke(error);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            HandleSuccess?.Invoke(result.JavaCast<TResult>());
        }
    }

    public class LoginStatusCallback : Java.Lang.Object, ILoginStatusCallback
    {
        public Action HandleCancel { get; set; }

        public Action<Exception> HandleError { get; set; }

        public Action<AccessToken> HandleSuccess { get; set; }

        public void OnCompleted(AccessToken accessToken)
        {
            HandleSuccess?.Invoke(accessToken);
        }

        public void OnError(Exception exception)
        {
            HandleError?.Invoke(exception);
        }

        public void OnFailure()
        {
            HandleCancel?.Invoke();
        }
    }
}