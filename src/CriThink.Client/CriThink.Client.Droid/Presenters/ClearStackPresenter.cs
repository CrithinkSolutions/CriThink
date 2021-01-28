using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.ViewModels;

namespace CriThink.Client.Droid.Presenters
{
    public class ClearStackPresenter : MvxAndroidViewPresenter
    {
        public ClearStackPresenter(IEnumerable<Assembly> androidViewAssemblies)
            : base(androidViewAssemblies)
        { }

        protected override Intent CreateIntentForRequest(MvxViewModelRequest request)
        {
            var intent = base.CreateIntentForRequest(request);

            if (request.PresentationValues != null)
            {
                if (request.PresentationValues.ContainsKey("MyCustomFlag"))
                {
                    intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask | ActivityFlags.ClearTop | ActivityFlags.SingleTop);
                }
            }

            return intent;
        }
    }
}