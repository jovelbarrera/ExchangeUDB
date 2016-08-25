using System;
using Xamarin.Forms;

namespace Exchange.Controls
{
	public class CustomSearchBar : SearchBar
	{
		public static readonly BindableProperty ValueProperty =
			BindableProperty.Create<CustomSearchBar, string>(
				p => p.Value, string.Empty
			);

		public string Value
		{
			get { return (string)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == SearchBar.TextProperty.PropertyName)
				SetValue(ValueProperty, this.Text);
			if (propertyName == CustomSearchBar.ValueProperty.PropertyName)
				Text = Value;
		}

	}
}

