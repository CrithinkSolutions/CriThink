using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Views.DebunkingNews;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Plugin.Visibility;
using MvvmCross.WeakSubscription;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.NewsChecker
{
    [MvxActivityPresentation]
    [Activity]
    public class NewsCheckerResultView : MvxActivity<NewsCheckerResultViewModel>
    {
        private const int VOTE = 5;

        private AppCompatImageView[] _imgUvVotes;
        private AppCompatImageView[] _imgCvVotes;
        private AppCompatTextView _tvCommunityVoteRating;
        private AppCompatTextView _tvUserVoteRating;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.newscheckerresult_view);

            var txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            var imgHeader = FindViewById<MvxSvgCachedImageView>(Resource.Id.imgHeader);
            var txtRelatedDNews = FindViewById<AppCompatTextView>(Resource.Id.txtRelatedDNews);
            var recyclerRelatedDNews = FindViewById<MvxRecyclerView>(Resource.Id.recyclerRelatedDNews);
            var boxUserVote = FindViewById<ConstraintLayout>(Resource.Id.box_user_vote);
            var boxCommunityVote = FindViewById<ConstraintLayout>(Resource.Id.box_community_vote);
            var tvResponseTitle = FindViewById<AppCompatTextView>(Resource.Id.tvResponseTitle);
            var tvResponse = FindViewById<AppCompatTextView>(Resource.Id.tvResponse);
            var tvDescription = FindViewById<AppCompatTextView>(Resource.Id.tvDescription);
            var tvNotification = FindViewById<AppCompatTextView>(Resource.Id.tvNotification);
            var switchNotification = FindViewById<SwitchCompat>(Resource.Id.switchNotification);
            var tvUserVote = FindViewById<AppCompatTextView>(Resource.Id.tvUserVote);
            var btnShare = FindViewById<AppCompatButton>(Resource.Id.btnShare);
            var viewFooter = FindViewById<View>(Resource.Id.viewFooter);
            _tvUserVoteRating = FindViewById<AppCompatTextView>(Resource.Id.tvUvRating);
            var imgUvVote1 = FindViewById<AppCompatImageView>(Resource.Id.img_uv_vote_1);
            var imgUvVote2 = FindViewById<AppCompatImageView>(Resource.Id.img_uv_vote_2);
            var imgUvVote3 = FindViewById<AppCompatImageView>(Resource.Id.img_uv_vote_3);
            var imgUvVote4 = FindViewById<AppCompatImageView>(Resource.Id.img_uv_vote_4);
            var imgUvVote5 = FindViewById<AppCompatImageView>(Resource.Id.img_uv_vote_5);
            var tvCommunityVote = FindViewById<AppCompatTextView>(Resource.Id.tvCommunityVote);
            _tvCommunityVoteRating = FindViewById<AppCompatTextView>(Resource.Id.tvCvRating);
            var imgCvVote1 = FindViewById<AppCompatImageView>(Resource.Id.img_cv_vote_1);
            var imgCvVote2 = FindViewById<AppCompatImageView>(Resource.Id.img_cv_vote_2);
            var imgCvVote3 = FindViewById<AppCompatImageView>(Resource.Id.img_cv_vote_3);
            var imgCvVote4 = FindViewById<AppCompatImageView>(Resource.Id.img_cv_vote_4);
            var imgCvVote5 = FindViewById<AppCompatImageView>(Resource.Id.img_cv_vote_5);
            _imgCvVotes = new AppCompatImageView[VOTE]
            {
                imgCvVote1,
                imgCvVote2,
                imgCvVote3,
                imgCvVote4,
                imgCvVote5
            };
            _imgUvVotes = new AppCompatImageView[VOTE]
            {
                imgUvVote1,
                imgUvVote2,
                imgUvVote3,
                imgUvVote4,
                imgUvVote5
            };
            SetVote();
            ViewModel.WeakSubscribe(() => ViewModel.NewsCheckerResultModel, SetVote_WeakSubscribe);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);
            Window.SetStatusBarColor(new Color(ContextCompat.GetColor(this, Resource.Color.gradientHeaderStart)));

            var layoutManager = new LinearLayoutManager(this);
            recyclerRelatedDNews.SetLayoutManager(layoutManager);
            recyclerRelatedDNews.SetItemAnimator(null);

            var adapter = new RelatedDebunkingNewsAdapter((IMvxAndroidBindingContext) BindingContext);
            recyclerRelatedDNews.Adapter = adapter;

            var set = CreateBindingSet();
            set.Bind(imgHeader).For(v => v.ImagePath).To(vm => vm.ResultImage);
            set.Bind(txtRelatedDNews).ToLocalizationId("RelatedDNews");
            set.Bind(adapter).For(v => v.ItemsSource).To(vm => vm.Feed);

            set.Bind(recyclerRelatedDNews).For(v => v.ItemClick).To(vm => vm.DebunkingNewsSelectedCommand);

            set.Bind(txtRelatedDNews).For(v => v.Visibility).To(vm => vm.HasRelatedDebunkingNews).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(recyclerRelatedDNews).For(v => v.Visibility).To(vm => vm.HasRelatedDebunkingNews).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(tvResponse).To(vm => vm.Classification);
            set.Bind(tvResponseTitle).To(vm => vm.ClassificationTitle);
            set.Bind(tvDescription).To(vm => vm.Description);
            set.Bind(tvUserVote).ToLocalizationId("UserVote");
            set.Bind(tvCommunityVote).ToLocalizationId("CommunityVote");
            set.Bind(boxCommunityVote).For(v => v.Visibility).To(vm => vm.NewsCheckerResultModel.IsUnknownResult).WithConversion<MvxInvertedVisibilityValueConverter>();
            set.Bind(viewFooter).For(v => v.Visibility).To(vm => vm.NewsCheckerResultModel.IsUnknownResult).WithConversion<MvxInvertedVisibilityValueConverter>();
            set.Bind(tvNotification).For(v => v.Visibility).To(vm => vm.NewsCheckerResultModel.IsUnknownResult).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(switchNotification).For(v => v.Visibility).To(vm => vm.NewsCheckerResultModel.IsUnknownResult).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(tvNotification).ToLocalizationId("NotificationTitle");
            set.Bind(switchNotification).For(v => v.Checked).To(vm => vm.IsSubscribed).TwoWay();
            set.Bind(txtTitle).ToLocalizationId("Title");

            set.Bind(btnShare).For(v => v.Text).ToLocalizationId("Share");
            set.Bind(btnShare).To(vm => vm.ShareCommand);

            set.Apply();
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Task.Run(() => ViewModel.NavigateToHomeAsync());
            return true;
        }

        public override void OnBackPressed()
        {
            Task.Run(() => ViewModel.NavigateToHomeAsync());
        }

        private void SetVote_WeakSubscribe(object sender, PropertyChangedEventArgs e)
        {
            SetVote();
        }

        private void SetVote()
        {
            if (ViewModel.NewsCheckerResultModel != null)
            {
                var newsSourcePostAnswersResponse = ViewModel.NewsCheckerResultModel.NewsSourcePostAnswersResponse;
                _tvUserVoteRating.Text = $"{newsSourcePostAnswersResponse.UserRate ?? 0}/5";
                _tvCommunityVoteRating.Text = $"{newsSourcePostAnswersResponse.CommunityRate ?? 0}/5";
                var roundUserVote = Math.Round(newsSourcePostAnswersResponse.UserRate ?? 0, 0);
                var roundCommunityVote = Math.Round(newsSourcePostAnswersResponse.CommunityRate ?? 0, 0);
                for (int i = 0; i < VOTE; i++)
                {
                    SetImageVote(_imgUvVotes[i], roundUserVote > i);
                    SetImageVote(_imgCvVotes[i], roundCommunityVote > i);
                }
            }
        }

        private void SetImageVote(AppCompatImageView imageView, bool isOn)
        {
            if (isOn)
            {
                imageView.SetBackgroundResource(Resource.Drawable.background_vote_on);
                imageView.SetImageResource(Resource.Drawable.ic_vote_on);
            }
            else
            {
                imageView.SetBackgroundResource(Resource.Drawable.background_vote_off);
                imageView.SetImageResource(Resource.Drawable.ic_vote_off);

            }
        }
    }
}