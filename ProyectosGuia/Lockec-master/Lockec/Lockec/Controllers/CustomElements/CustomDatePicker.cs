namespace Lockec.Controllers.CustomElements
{
    using Xamarin.Forms;
    public class CustomDatePicker : DatePicker
    {

        public static readonly BindableProperty EnterTextProperty =
            BindableProperty.Create(propertyName: "Placeholder", returnType: typeof(string),
                declaringType: typeof(CustomDatePicker), defaultValue: default(string));

        public static readonly BindableProperty UnderLineProperty =
            BindableProperty.Create(propertyName: "Underline", returnType: typeof(bool),
                declaringType: typeof(CustomDatePicker), defaultValue: default(bool));

        public static readonly BindableProperty EnterTextColorProperty =
            BindableProperty.Create(propertyName: "PlaceholderColor", returnType: typeof(string),
                declaringType: typeof(CustomDatePicker), defaultValue: default(string));

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
