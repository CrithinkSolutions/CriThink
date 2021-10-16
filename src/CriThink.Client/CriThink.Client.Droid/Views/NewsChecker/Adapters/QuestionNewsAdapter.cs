using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
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
        public QuestionNewsViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            _tvQuestion = itemView.FindViewById<AppCompatTextView>(Resource.Id.tv_question);
            _radioGroup = itemView.FindViewById<RadioGroup>(Resource.Id.radioGroupVote);
            this.DelayBind(() =>
            {
                using (var set = this.CreateBindingSet<QuestionNewsViewHolder, NewsSourceGetQuestionResponse>())
                {
                    set.Bind(_tvQuestion).To(x => x.Text);
                    set.Apply();
                }
            });


        }
    }
}
