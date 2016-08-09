using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class HomePage : TabbedPage
	{
		public HomePage()
		{
			Children.Add(new AskPage());
			Children.Add(new ExchangePage());
		}

		protected override void OnCurrentPageChanged()
		{
			base.OnCurrentPageChanged();
			Title = CurrentPage.Title;
		}
	}
}


