using System.ComponentModel;
using Android.Content;
using Android.Views;
using Android.Widget;
using Exchange.Controls;
using Exchange.Droid.Renderes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ListView = Android.Widget.ListView;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(CustomTableView), typeof(DroidCustomTableView))]
namespace Exchange.Droid.Renderes
{
    public class DroidCustomTableView : TableViewRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        protected override TableViewModelRenderer GetModelRenderer(ListView listView, TableView view)
        {
            return new CustomTableViewModelRenderer(Forms.Context, listView, view);
        }
    }

    public class CustomTableViewModelRenderer : TableViewModelRenderer
    {
        public CustomTableViewModelRenderer(Context context, ListView listView, TableView view)
            : base(context, listView, view)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = base.GetView(position, convertView, parent);
            if (GetCellForPosition(position).GetType() != typeof(TextCell))
                return view;

            var layout = (LinearLayout)view;

            TextView text = (TextView)((LinearLayout)((LinearLayout)layout.GetChildAt(0)).GetChildAt(1)).GetChildAt(0);
            View divider = layout.GetChildAt(1);

            try
            {
                BaseCellView cellView = (BaseCellView)layout.GetChildAt(0);

                Android.Graphics.Color separatorColor = Context.Resources.GetColor(Context.Resources.GetIdentifier("placeholder", "color", Context.PackageName));
                divider.SetBackgroundColor(Color.Transparent.ToAndroid());
                if (string.IsNullOrEmpty(cellView.DetailText))
                {
                    Android.Graphics.Color titleColor = Context.Resources.GetColor(Context.Resources.GetIdentifier("primary", "color", Context.PackageName));
                    text.SetTextColor(titleColor);
                    text.SetTypeface(Android.Graphics.Typeface.Default, Android.Graphics.TypefaceStyle.Bold);
                }                
            }
            catch (System.Exception e)
            {
                var error = e.Message;
            }
            return view;
        }
    }
}