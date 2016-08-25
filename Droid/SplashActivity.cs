using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Exchange.Droid
{
	[Activity (Theme = "@style/Theme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
	public class SplashActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			StartActivity (typeof(MainActivity));
		}
	}
}
