using System;
using Android.Runtime;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace CriThink.Client.Droid.Views.NewsChecker.Dialogs
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(QuestionAboutNewsView))]
    public class QuestionAboutNewsView
    {
        public QuestionAboutNewsView()
        {
        }
    }
}
