using System;
using Xamarin.Forms;

namespace Exchange.Controls
{
	public class CustomEntry : Entry
	{
		public static readonly BindableProperty ValueProperty = 
			BindableProperty.Create<CustomEntry,string> (
				p => p.Value, string.Empty
			);

		public string Value {
			get { return (string)GetValue (ValueProperty); }
			set { SetValue (ValueProperty, value); }
		}

		protected override void OnPropertyChanged (string propertyName)
		{
			base.OnPropertyChanged (propertyName);
			if (propertyName == Entry.TextProperty.PropertyName)
				SetValue (ValueProperty, this.Text);
			if (propertyName == CustomEntry.ValueProperty.PropertyName)
				Text = Value;
		}

	}
}


