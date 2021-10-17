using System;
using System.Collections;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace CriThink.Client.Droid.Views.NewsChecker.Adapters
{
    public class QuestionNewsAdapter : MvxRecyclerAdapter
    {
        public QuestionNewsAdapter(IMvxAndroidBindingContext bindingContext)
            : base(bindingContext)
        { }

        [Preserve(Conditional = true)]
        protected QuestionNewsAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
            var view = InflateViewForHolder(parent, Resource.Layout.cell_newsquestion, itemBindingContext);

            return new QuestionNewsViewHolder(view, itemBindingContext);
        }
    }

    public class QuestionNewsViewHolder : MvxRecyclerViewHolder
    {
        private readonly AppCompatTextView _tvQuestion;
        private readonly RadioGroup _radioGroup;
        private readonly RadioButton _radioVote1;
        private readonly RadioButton _radioVote2;
        private readonly RadioButton _radioVote3;
        private readonly RadioButton _radioVote4;
        private readonly RadioButton _radioVote5;
        public QuestionNewsViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            _tvQuestion = itemView.FindViewById<AppCompatTextView>(Resource.Id.tv_question);
            _radioGroup = itemView.FindViewById<RadioGroup>(Resource.Id.radioGroupVote);
            _radioVote1 = itemView.FindViewById<RadioButton>(Resource.Id.radioVote1);
            _radioVote2 = itemView.FindViewById<RadioButton>(Resource.Id.radioVote2);
            _radioVote3 = itemView.FindViewById<RadioButton>(Resource.Id.radioVote3);
            _radioVote4 = itemView.FindViewById<RadioButton>(Resource.Id.radioVote4);
            _radioVote5 = itemView.FindViewById<RadioButton>(Resource.Id.radioVote5);
            this.DelayBind(() =>
            {
                using (var set = this.CreateBindingSet<QuestionNewsViewHolder, NewsSourceGetQuestionViewModel>())
                {
                    set.Bind(_tvQuestion).To(v => v.Question);
                    set.Bind(_radioVote1).For(v => v.Text).ToLocalizationId("One");
                    set.Bind(_radioVote1).For(v => v.Checked).To(vm => vm.Response[0]).TwoWay();
                    set.Bind(_radioVote2).For(v => v.Text).ToLocalizationId("Two");
                    set.Bind(_radioVote2).For(v => v.Checked).To(vm => vm.Response[1]).TwoWay();
                    set.Bind(_radioVote3).For(v => v.Text).ToLocalizationId("Three");
                    set.Bind(_radioVote3).For(v => v.Checked).To(vm => vm.Response[2]).TwoWay();
                    set.Bind(_radioVote4).For(v => v.Text).ToLocalizationId("Four");
                    set.Bind(_radioVote4).For(v => v.Checked).To(vm => vm.Response[3]).TwoWay();
                    set.Bind(_radioVote5).For(v => v.Text).ToLocalizationId("Five");
                    set.Bind(_radioVote5).For(v => v.Checked).To(vm => vm.Response[4]).TwoWay();
                }
            });


        }
    }
}
