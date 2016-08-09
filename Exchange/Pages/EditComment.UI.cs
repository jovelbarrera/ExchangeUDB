using System;
using Exchange.Configs;
using Exchange.Controls;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class EditComment
	{
		private ScrollView _mainLayout;
		private CustomImage _pictureImage;
		private CustomEditor _messageEditor;
		private Label _chractersLeftLabel;

		public void InitializeComponents()
		{
			Title = "Nuevo Comentatio";
			BackgroundColor = Color.White;

			var contentLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Padding = new Thickness(20),
				Spacing = 15,
			};

			#region image
			var imageLayout = new StackLayout
			{
				HorizontalOptions = LayoutOptions.End,
			};
			contentLayout.Children.Add(imageLayout);
			_pictureImage = new CustomImage
			{
				Aspect = Aspect.Fill,
				Source = "ic_picture_placeholder.png",
				WidthRequest = 60,
				HeightRequest = 60,
			};
			imageLayout.Children.Add(_pictureImage);
			#endregion

			#region inputs
			var inputContentLayout = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			contentLayout.Children.Add(inputContentLayout);
			var inputsLayout = new RelativeLayout
			{
				VerticalOptions = LayoutOptions.Start,
			};
			inputContentLayout.Children.Add(inputsLayout);

			_messageEditor = new CustomEditor
			{
				Style = Styles.NormalEditor,
				TextColor = Styles.Colors.Subtitle,
				Text = "Escribe tu pregunta",
				HeightRequest = 100,
			};
			_messageEditor.TextChanged += MessageEditor_TextChanged;
			_messageEditor.Focused += MessageEditor_Focused;
			_messageEditor.Unfocused += MessageEditor_Unfocused;
			inputsLayout.Children.Add(_messageEditor,
									  Constraint.Constant(0),
									  Constraint.Constant(0),
									  Constraint.RelativeToParent(p => p.Width),
									  Constraint.Constant(160));
			BoxView descriptionSeparator = UIHelper.UIHelper.Separator();
			inputsLayout.Children.Add(descriptionSeparator,
									  Constraint.RelativeToView(_messageEditor, (p, v) => v.X),
									  Constraint.RelativeToView(_messageEditor, (p, v) => v.Y + v.Height + 5),
									  Constraint.RelativeToView(_messageEditor, (p, v) => v.Width),
									  Constraint.Constant(1));
			_chractersLeftLabel = new Label
			{
				Style = Styles.Verbosa,
				HorizontalTextAlignment = TextAlignment.End,
				Text = "@" + MaxCharsOnDecription,
			};
			inputsLayout.Children.Add(_chractersLeftLabel,
									  Constraint.RelativeToView(descriptionSeparator, (p, v) => v.X),
									  Constraint.RelativeToView(descriptionSeparator, (p, v) => v.Y + v.Height + 5),
									  Constraint.RelativeToView(descriptionSeparator, (p, v) => v.Width),
									  Constraint.Constant(20));
			
			var noteLabel = new Label
			{
				Style = Styles.Verbosa,
				FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
				Text = "Al menos una tag del tema relacionado a tu pregunta.\nPara múltiles tags separa con comas."
			};
			inputsLayout.Children.Add(noteLabel,
									  Constraint.RelativeToView(_chractersLeftLabel, (p, v) => v.X),
									  Constraint.RelativeToView(_chractersLeftLabel, (p, v) => v.Y + v.Height + 5),
									  Constraint.RelativeToView(_chractersLeftLabel, (p, v) => v.Width),
									  Constraint.Constant(40));
			#endregion

			var doneToolbarItem = new ToolbarItem
			{
				Text = "Hecho",
			};

			if (Device.OS == TargetPlatform.Android)
				doneToolbarItem.Icon = "ic_action_done.png";

			doneToolbarItem.Clicked += DoneToolbarItem_Clicked;
			ToolbarItems.Add(doneToolbarItem);

			_mainLayout = new ScrollView { Content = contentLayout };
			Content = _mainLayout;
		}
	}
}


