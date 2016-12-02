using System;
using Xamarin.Forms;

namespace Exchange.Controls
{
	public class Fab : View
	{
		public static readonly BindableProperty ClickProperty = BindableProperty.Create<Fab, EventHandler>(p => p.Click, default(EventHandler));

		public EventHandler Click
		{
			get { return (EventHandler)GetValue(ClickProperty); }
			set { SetValue(ClickProperty, value); }
		}
	}
}
