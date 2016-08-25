using System.ComponentModel;
using Exchange.Droid.Renderers;
using Exchange.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer ( typeof ( CustomEntry ), typeof ( DroidCustomEntry ) )]
namespace Exchange.Droid.Renderers
{
    public class DroidCustomEntry : EntryRenderer
    {
        protected override void OnElementChanged ( ElementChangedEventArgs<Entry> e )
        {
            base.OnElementChanged ( e );
            if ( e.OldElement != null || Element == null )
                return;
            
            Control.SetBackgroundColor ( Element.BackgroundColor.ToAndroid ( ) );
        }

        protected override void OnElementPropertyChanged ( object sender, PropertyChangedEventArgs e )
        {
            base.OnElementPropertyChanged ( sender, e );
            if ( this == null )
                return;
            
            Control.SetBackgroundColor ( Element.BackgroundColor.ToAndroid ( ) );
        }
    }
}


