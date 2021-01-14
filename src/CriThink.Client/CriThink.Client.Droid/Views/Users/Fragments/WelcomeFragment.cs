using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Fragment.App;
using CriThink.Client.Droid.Constants;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [Register(ViewConstants.Namespace + ".users." + nameof(WelcomeFragment))]
    public class WelcomeFragment : Fragment
    {
        private const string KeyContent = "WelcomeFragment:Content";

        public int ImageId { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var image = new AppCompatImageView(Activity) { Id = ImageId + 123 };

            var drawable = Context.GetDrawable(ImageId);
            image.SetImageDrawable(drawable);

            var layout = new ConstraintLayout(Activity)
            {
                LayoutParameters =
                    new ViewGroup.LayoutParams(
                        ViewGroup.LayoutParams.MatchParent,
                        ViewGroup.LayoutParams.MatchParent)
            };

            layout.SetBackgroundResource(Resource.Color.welcomeBackground);
            layout.AddView(image);

            var constraintSet = new ConstraintSet();
            constraintSet.Connect(image.Id, ConstraintSet.Left, ConstraintSet.ParentId, ConstraintSet.Left, 0);
            constraintSet.Connect(image.Id, ConstraintSet.Right, ConstraintSet.ParentId, ConstraintSet.Right, 0);
            constraintSet.Connect(image.Id, ConstraintSet.Top, ConstraintSet.ParentId, ConstraintSet.Top, 0);
            constraintSet.Connect(image.Id, ConstraintSet.Bottom, ConstraintSet.ParentId, ConstraintSet.Bottom, 0);
            constraintSet.ApplyTo(layout);

            return layout;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (savedInstanceState != null && savedInstanceState.ContainsKey(KeyContent))
                ImageId = savedInstanceState.GetInt(KeyContent);
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt(KeyContent, ImageId);
        }
    }
}