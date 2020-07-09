namespace Lockec.Controllers.CustomElements
{
    using Xamarin.Forms;
    public class CustomTimePicker : TimePicker
    {

        public static readonly BindableProperty EnterTextProperty =
            BindableProperty.Create(propertyName: "Placeholder", returnType: typeof(string),
                declaringType: typeof(CustomTimePicker), defaultValue: default(string));

        public static readonly BindableProperty UnderLineProperty =
            BindableProperty.Create(propertyName: "Underline", returnType: typeof(bool),
                declaringType: typeof(CustomTimePicker), defaultValue: default(bool));

        public static readonly BindableProperty EnterTextColorProperty =
            BindableProperty.Create(propertyName: "PlaceholderColor", returnType: typeof(string),
                declaringType: typeof(CustomTimePicker), defaultValue: default(string));

        public string Placeholder
        {
            get;
            set;
        }
        public bool Underline
        {
            get;
            set;
        }
        public string PlaceholderColor
        {
            get;
            set;
        }

    }

}
