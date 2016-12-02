using Exchange.Dependencies;
using Exchange.Droid.Dependencies;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidPlatform))]
namespace Exchange.Droid.Dependencies
{
	public class DroidPlatform : IPlatform
	{
		public void ChangeScreenOrientation(ScreenOrientation screenOrientation)
		{
			if (screenOrientation == ScreenOrientation.Portrait)
				MainActivity.Instance.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
			else if (screenOrientation == ScreenOrientation.Landscape)
				MainActivity.Instance.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
			else if (screenOrientation == ScreenOrientation.Sensor)
				MainActivity.Instance.RequestedOrientation = Android.Content.PM.ScreenOrientation.Sensor;
		}
	}
}