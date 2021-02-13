﻿using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.Users;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [MvxFragmentPresentation(typeof(HomeViewModel), "tabs_container_frame")]
    [Register(nameof(AboutView))]
    public class AboutView : MvxFragment<AboutViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.about_view, null);

            var txtUsername = view.FindViewById<AppCompatTextView>(Resource.Id.txtUsername);
            var imgProfile = view.FindViewById<MvxCachedImageView>(Resource.Id.imgProfile);
            var btnViewProfile = view.FindViewById<AppCompatButton>(Resource.Id.btnViewProfile);
            var recyclerOptions = view.FindViewById<MvxRecyclerView>(Resource.Id.recyclerOptions);

            var layoutManager = new LinearLayoutManager(Activity) { AutoMeasureEnabled = true };
            recyclerOptions.SetLayoutManager(layoutManager);
            recyclerOptions.HasFixedSize = true;

            recyclerOptions.ItemTemplateSelector = new MenuItemSelector();

            var set = CreateBindingSet();

            set.Bind(txtUsername).To(vm => vm.Username);
            set.Bind(imgProfile).For(v => v.Transformations).To(vm => vm.ProfileImageTransformations);
            set.Bind(imgProfile).For(v => v.ImagePath).To(vm => vm.ProfileImagePath);
            set.Bind(btnViewProfile).For(v => v.Text).ToLocalizationId("ViewProfile");
            set.Bind(btnViewProfile).To(vm => vm.NavigateToProfileCommand);
            set.Bind(recyclerOptions).For(v => v.ItemsSource).To(vm => vm.MenuCollection);

            set.Apply();

            return view;
        }
    }
}