using System;
using Exchange.Configs;
using Exchange.Controls;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class EditAsk
	{
		private ScrollView _mainLayout;
		private CustomImage _pictureImage;
		private CustomEntry _titleEntry;
		private CustomEditor _descriptionEditor;
		private Label _chractersLeftLabel;
		private CustomEntry _tagsEntry;
		private StackLayout _tagsLayout;

		public void InitializeComponents()
		{
			Title = "Nueva Pregunta";
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
			_titleEntry = new CustomEntry
			{
				Style = Styles.NormalEntry,
				Placeholder = "Título",
			};
			_titleEntry.TextChanged += TitleEntry_TextChanged;
			inputsLayout.Children.Add(_titleEntry,
									  Constraint.Constant(0),
									  Constraint.Constant(0),
									  Constraint.RelativeToParent(p => p.Width),
									  Constraint.Constant(40)
									 );
			BoxView titleSeparator = UIHelper.UIHelper.Separator();
			inputsLayout.Children.Add(titleSeparator,
									  Constraint.RelativeToView(_titleEntry, (p, v) => v.X),
									  Constraint.RelativeToView(_titleEntry, (p, v) => v.Y + v.Height + 5),
									  Constraint.RelativeToView(_titleEntry, (p, v) => v.Width),
									  Constraint.Constant(1));
			_descriptionEditor = new CustomEditor
			{
				Style = Styles.NormalEditor,
				TextColor = Styles.Colors.Subtitle,
				Text = "Escribe tu pregunta",
				HeightRequest = 100,
			};
			_descriptionEditor.TextChanged += DescriptionEditor_TextChanged;
			_descriptionEditor.Focused += DescriptionEditor_Focused;
			_descriptionEditor.Unfocused += DescriptionEditor_Unfocused;
			inputsLayout.Children.Add(_descriptionEditor,
									  Constraint.RelativeToView(titleSeparator, (p, v) => v.X),
									  Constraint.RelativeToView(titleSeparator, (p, v) => v.Y + v.Height + 5),
									  Constraint.RelativeToView(titleSeparator, (p, v) => v.Width),
									  Constraint.Constant(160));
			BoxView descriptionSeparator = UIHelper.UIHelper.Separator();
			inputsLayout.Children.Add(descriptionSeparator,
									  Constraint.RelativeToView(_descriptionEditor, (p, v) => v.X),
									  Constraint.RelativeToView(_descriptionEditor, (p, v) => v.Y + v.Height + 5),
									  Constraint.RelativeToView(_descriptionEditor, (p, v) => v.Width),
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
			_tagsEntry = new CustomEntry
			{
				Style = Styles.NormalEntry,
				Placeholder = "Tags",
			};
			_tagsEntry.TextChanged += TagsEntry_TextChanged;
			_tagsEntry.Completed += TagsEntry_Completed;
			inputsLayout.Children.Add(_tagsEntry,
									  Constraint.RelativeToView(_chractersLeftLabel, (p, v) => v.X),
									  Constraint.RelativeToView(_chractersLeftLabel, (p, v) => v.Y + v.Height + 5),
									  Constraint.RelativeToView(_chractersLeftLabel, (p, v) => v.Width),
									  Constraint.Constant(40));
			BoxView tagsSeparator = UIHelper.UIHelper.Separator();
			inputsLayout.Children.Add(tagsSeparator,
									  Constraint.RelativeToView(_tagsEntry, (p, v) => v.X),
									  Constraint.RelativeToView(_tagsEntry, (p, v) => v.Y + v.Height + 5),
									  Constraint.RelativeToView(_tagsEntry, (p, v) => v.Width),
									  Constraint.Constant(1));
			var noteLabel = new Label
			{
				Style = Styles.Verbosa,
				FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
				Text = "Al menos una tag del tema relacionado a tu pregunta.\nPara múltiles tags separa con comas."
			};
			inputsLayout.Children.Add(noteLabel,
									  Constraint.RelativeToView(tagsSeparator, (p, v) => v.X),
									  Constraint.RelativeToView(tagsSeparator, (p, v) => v.Y + v.Height + 5),
									  Constraint.RelativeToView(tagsSeparator, (p, v) => v.Width),
									  Constraint.Constant(40));
			_tagsLayout = new StackLayout();
			inputContentLayout.Children.Add(_tagsLayout);
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

		private StackLayout TagLayout(string tag)
		{
			var layout = new StackLayout
			{
				HorizontalOptions = LayoutOptions.StartAndExpand,
				BackgroundColor = Styles.Colors.Placeholder,
				Padding = new Thickness(5),
			};
			var label = new Label
			{
				Style = Styles.Subtitle,
				Text = tag,
			};
			layout.Children.Add(label);
			layout.AddTapHandler((s, e) =>
			{
				_tagsLayout.Children.Remove(layout);
				RemoveTag(label.Text);
			});
			return layout;
		}
	}
}


