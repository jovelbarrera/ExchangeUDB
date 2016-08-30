using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using BranchXamarinSDK;
using Xamarin.Facebook;

namespace Exchange.Droid
{
	[Activity(Label = "Exchange", Icon = "@drawable/ic_launcher", Theme = "@style/AppTheme",
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		public static ICallbackManager CallbackManager;
		public static MainActivity Instance;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			ToolbarResource = Resource.Layout.Toolbar;
			TabLayoutResource = Resource.Layout.Tabbar;

			Xamarin.Facebook.FacebookSdk.SdkInitialize(this);
			global::Xamarin.Forms.Forms.Init(this, bundle);

			CallbackManager = CallbackManagerFactory.Create();
			Instance = this;

			var app = new App();

			BranchAndroid.Init(this, "key_test_fhqjhUDb0PnWR0j87e1Q9kifBBjk0cMf", app);

            LoadApplication(app);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			CallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
		}

		private void ConsolePrintAndroidKeyHash()
		{
			PackageInfo info = this.PackageManager.GetPackageInfo(this.PackageName, PackageInfoFlags.Signatures);
			foreach (Android.Content.PM.Signature signature in info.Signatures)
			{
				Java.Security.MessageDigest md = Java.Security.MessageDigest.GetInstance("SHA");
				md.Update(signature.ToByteArray());

				string keyhash = System.Convert.ToBase64String(md.Digest());
				System.Console.WriteLine("KeyHash:", keyhash);
			}
		}
	}
}

