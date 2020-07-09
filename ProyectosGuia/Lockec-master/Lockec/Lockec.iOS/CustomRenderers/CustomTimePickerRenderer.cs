using Lockec.Controllers.CustomElements;
using Lockec.iOS.CustomRenderers;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomTimePicker), typeof(CustomTimePickerRenderer))]
namespace Lockec.iOS.CustomRenderers
{
    class CustomTimePickerRenderer : TimePickerRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);
            if (this.Control == null)
                return;
            var element = e.NewElement as CustomTimePicker;
            if (!string.IsNullOrWhiteSpace(element.Placeholder))
            {
                Control.Text = element.Placeholder;
            }

            Control.ShouldEndEditing += (textField) => {
                var seletedTime = (UITextField)textField;
                var text = seletedTime.Text;
                if (text == element.Placeholder)
                {
                    Control.Text = DateTime.Now.ToString("HH:mm");
                }
                return true;
            };
        }
        private void OnCanceled(object sender, EventArgs e)
        {
            Control.ResignFirstResponder();
        }
        private void OnDone(object sender, EventArgs e)
        {
            Control.ResignFirstResponder();
        }

    }
}