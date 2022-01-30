using UIKit;

namespace CriThink.Client.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
#pragma warning disable CS0618 // Type or member is obsolete
            UIApplication.Main(args, null, "AppDelegate");
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}