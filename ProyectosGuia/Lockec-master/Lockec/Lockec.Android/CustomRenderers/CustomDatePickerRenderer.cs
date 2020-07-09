using Android.Content;
using Android.Graphics.Drawables;
using Lockec.Controllers.CustomElements;
using Lockec.Droid.CustomRenderers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace Lockec.Droid.CustomRenderers
{
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        public CustomDatePickerRenderer(Context context) : base(context)
        { }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement != null)
            {
                var element = e.NewElement as CustomDatePicker;
                GradientDrawable gd = new GradientDrawable();
                //gd.SetColor(global::Android.Graphics.Color.Black);
                if (!string.IsNullOrWhiteSpace(element.Placeholder))
                {
                    //this.Control.Text = element.Placeholder;
                    Control.Text = element.Placeholder;
                    
                }
                if (!string.IsNullOrWhiteSpace(element.PlaceholderColor))
                {
                    Control.SetTextColor(Android.Graphics.Color.ParseColor(element.PlaceholderColor));
                }
                if (!element.Underline)
                {
                    gd.SetColor(global::Android.Graphics.Color.Transparent);
                    Control.SetBackground(gd);
                }

                this.Control.TextChanged += (sender, arg) => {
                    var selectedDate = arg.Text.ToString();
                    if (selectedDate == element.Placeholder)
                    {
                        Control.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    Control.SetTextColor(Android.Graphics.Color.Black);
                };
            }
        }

    }
}