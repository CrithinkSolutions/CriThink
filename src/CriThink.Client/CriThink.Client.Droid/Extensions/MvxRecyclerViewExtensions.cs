using System;
using System.Windows.Input;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Droid.Controls;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.ViewModels;

namespace CriThink.Client.Droid.Extensions
{
    public static class MvxRecyclerViewExtensions
    {
        public static void AddOnScrollFetchItemsListener(this MvxRecyclerView recyclerView, LinearLayoutManager linearLayoutManager, Func<MvxNotifyTask> fetchItemsTaskCompletionFunc, Func<ICommand> fetchItemsCommandFunc)
        {
            var onScrollListener = new RecyclerViewOnScrollListener(linearLayoutManager);
            onScrollListener.LoadMoreEvent += (object sender, EventArgs e) =>
            {
                var fetchItemsTaskCompletion = fetchItemsTaskCompletionFunc.Invoke();
                if (fetchItemsTaskCompletion == null || !fetchItemsTaskCompletion.IsNotCompleted)
                    fetchItemsCommandFunc.Invoke().Execute(null);
            };
            recyclerView.AddOnScrollListener(onScrollListener);
        }
    }
}