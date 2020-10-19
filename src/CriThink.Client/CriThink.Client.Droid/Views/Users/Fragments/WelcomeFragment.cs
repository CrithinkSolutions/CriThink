using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
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
            var image = new AppCompatImageView(Activity);

            var drawable = Context.GetDrawable(ImageId);
            image.SetImageDrawable(drawable);

            var layout = new LinearLayout(Activity)
            {
                LayoutParameters =
                    new ViewGroup.LayoutParams(
                        ViewGroup.LayoutParams.MatchParent,
                        ViewGroup.LayoutParams.MatchParent)
            };

            layout.AddView(image);

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