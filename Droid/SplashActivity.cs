using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Exchange.Droid
{
	[Activity(Theme = "@style/Theme.Splash", MainLauncher = true,
			   ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
			   ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true,
	          LaunchMode = LaunchMode.SingleTask)]

	[IntentFilter(new[] { "android.intent.action.VIEW" },
		Categories = new[]{"android.intent.category.DEFAULT",
		"android.intent.category.BROWSABLE"},
		DataScheme = "exchangeudb",
		DataHost = "open")]

	public class SplashActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			StartActivity(typeof(MainActivity));
		}
	}
}
