using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class VideoPlayerPage
	{
		protected override void InitializeComponents()
		{
			BackgroundColor = Color.Black;
		}

		private RelativeLayout PlayerUI(View playerView)
		{
			if (playerView == null)
				return new RelativeLayout();

			var mainLayout = new RelativeLayout();
			mainLayout.Children.Add(playerView,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent(p => p.Width),
				Constraint.RelativeToParent(p => p.Height)
			);
			
			return mainLayout;
		}
	}
}


