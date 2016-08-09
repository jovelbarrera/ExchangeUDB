using System;
using System.ComponentModel;
using Exchange.Controls;
using Exchange.Droid.Renderes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(DroidCustomSearchBar))]
namespace Exchange.Droid.Renderes
{
	public class DroidCustomSearchBar : SearchBarRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement != null || Element == null)
				return;

			Control.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (this == null)
				return;

			Control.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
		}
	}
}




