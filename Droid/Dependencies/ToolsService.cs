using Android.App;
using Android.Support.Design.Widget;
using Exchange.Dependencies;
using Exchange.Droid.Dependencies;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToolsService))]
namespace Exchange.Droid.Dependencies
{
	public class ToolsService : IToolsService
	{
		private const int TabsNumber = 2;

		public void TabLayout()
		{
			var activity = Forms.Context as Activity;
			TabLayout tabs = null;
			tabs = activity.FindViewById<TabLayout>(Resource.Id.sliding_tabs);
			if (tabs == null)
				return;
			for (int i = 0; i < TabsNumber; i++)
			{
				var tab = tabs.GetTabAt(i);
				tab.SetText(string.Empty);
				switch (i)
				{
					case 0:
						tab.SetIcon(activity.Resources.GetDrawable(Resource.Drawable.ask_tab));
						break;
					case 1:
						tab.SetIcon(activity.Resources.GetDrawable(Resource.Drawable.exchange_tab));
						break;
					default:
						System.Diagnostics.Debug.WriteLine("Default case");
						break;
				}
			}
		}
	}
}