using System;
using System.Collections.Generic;
using Exchange.Configs;
using Exchange.Controls;
using Exchange.Models;
using Xamarin.Forms;

namespace Exchange.ViewCells
{
	public class ActivityViewCell : ViewCell
	{
		private Label _infoLabel;

		public ActivityViewCell()
		{
			var maiLayout = new StackLayout
			{
				Padding = new Thickness(20, 5),
				Spacing = 10,
			};
			_infoLabel = new Label
			{
				Style = Styles.Verbosa,
			};
			maiLayout.Children.Add(_infoLabel);
			maiLayout.Children.Add(UIHelper.UIHelper.Separator());

			View = maiLayout;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			Activity activity = this.BindingContext as Activity;

			if (activity == null)
				return;

			var formattedString = new FormattedString();
			var title = new Span
			{
				ForegroundColor = Styles.Colors.NormalText,
				FontAttributes = FontAttributes.Bold,
				Text = activity.TypeString() + " ",
			};
			var detail = new Span
			{
				ForegroundColor = Styles.Colors.NormalText,
				Text = activity.Detail,
			};
			formattedString.Spans.Add(title);
			formattedString.Spans.Add(detail);
			_infoLabel.FormattedText = formattedString;
		}
	}
}


