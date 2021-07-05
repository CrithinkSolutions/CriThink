using System;
using System.Threading.Tasks;
using MvvmCross.Localization;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel, IMvxLocalizedTextSourceOwner
    {
        public IMvxLanguageBinder LocalizedTextSource => new MvxLanguageBinder("", GetType().Name);

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private bool _hasFailed;
        public bool HasFailed
        {
            get => _hasFailed;
            set => SetProperty(ref _hasFailed, value);
        }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; }

        protected T LoadChildViewModel<T>(IMvxViewModelLoader mvxViewModelLoader) where T : IMvxViewModel
        {
            if (mvxViewModelLoader is null)
                throw new ArgumentNullException(nameof(mvxViewModelLoader));

            return (T) mvxViewModelLoader.LoadViewModel(
                new MvxViewModelRequest<T>(null, null), null);
        }
    }

    public abstract class BaseViewModel<TParameter> : BaseViewModel, IMvxViewModel<TParameter>
    {
        public abstract void Prepare(TParameter parameter);
    }

    public abstract class MvxBaseViewModel<TParameter, TResult> : BaseViewModel, IMvxViewModel<TParameter, TResult>
    {
        public abstract void Prepare(TParameter parameter);
    }

    public abstract class ViewModelResult<TResult> : BaseViewModel, IMvxViewModelResult<TResult>
    {
        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.TrySetCanceled();

            base.ViewDestroy(viewFinishing);
        }
    }
}
