using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Lockec.Controllers.CustomElements;
using Lockec.Droid.CustomRenderers;
using System;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomTimePicker), typeof(CustomTimePickerRenderer))]
namespace Lockec.Droid.CustomRenderers
{
    class CustomTimePickerRenderer : ViewRenderer<TimePicker, Android.Widget.EditText>, TimePickerDialog.IOnTimeSetListener, IJavaObject, IDisposable
    {

        public CustomTimePickerRenderer(Context context) : base(context)
        { }

        private TimePickerDialog dialog = null;

        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);
            this.SetNativeControl(new Android.Widget.EditText(this.Context));
            this.Control.Click += Control_Click;
            this.Control.Text = DateTime.Now.ToString("HH:mm");
            this.Control.TextSize = 14;
            this.Control.KeyListener = null;
            this.Control.FocusChange += Control_FocusChange;

            CustomTimePicker element = Element as CustomTimePicker;
            GradientDrawable gd = new GradientDrawable();

            if (!string.IsNullOrWhiteSpace(element.Placeholder))
            {
                this.Control.Text = element.Placeholder;
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

        }

        void Control_FocusChange(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
                ShowTimePicker();
        }

        void Control_Click(object sender, EventArgs e)
        {
            ShowTimePicker();
        }

        private void ShowTimePicker()
        {
            if (dialog == null)
            {
                dialog = new TimePickerDialog(this.Context, this, DateTime.Now.Hour, DateTime.Now.Minute, true);
            }
            dialog.Show();
        }

        public void OnTimeSet(Android.Widget.TimePicker view, int hourOfDay, int minute)
        {
            var time = new TimeSpan(hourOfDay, minute, 0);
            this.Element.SetValue(Xamarin.Forms.TimePicker.TimeProperty, time);
            this.Control.Text = time.ToString(@"hh\:mm");
            var hour = hourOfDay;
            var minutel = minute;
            updateTime(hour, minutel);
        }

        private void updateTime(int hours, int mins)
        {

            String timeSet = "";
            if (hours > 12)
            {
                hours -= 12;
                timeSet = "PM";
            }
            else if (hours == 0)
            {
                hours += 12;
                timeSet = "AM";
            }
            else if (hours == 12)
                timeSet = "PM";
            else
                timeSet = "AM";


            String minutes = "";
            if (mins < 10)
                minutes = "0" + mins;
            else
                minutes = mins.ToString();

            // Append in a StringBuilder
            String aTime = new StringBuilder().Append(hours).Append(':')
                   .Append(minutes).Append(" ").Append(timeSet).ToString();

            this.Control.Text = aTime.ToString();
            this.Control.SetTextColor(Android.Graphics.Color.Black);
        }

    }
}