using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Android.Content;
using CriThink.Client.Core.Constants;
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
                if (request.PresentationValues.ContainsKey(MvxBundleConstaints.ClearBackStack))
                {
                    intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask | ActivityFlags.ClearTop | ActivityFlags.SingleTop);
                }
            }

            return intent;
        }
    }

    public class GoHomeStackPresenter : MvxAndroidViewPresenter
    {
        public GoHomeStackPresenter(IEnumerable<Assembly> androidViewAssemblies)
            : base(androidViewAssemblies)
        { }

        public override Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is MyFunkyPresentationHint)
            {

            }

            return base.ChangePresentation(hint);
        }
    }

    public class MyFunkyPresentationHint : MvxPresentationHint
    {
        public int DegreeOfFunkiness { get; set; }
    }
}