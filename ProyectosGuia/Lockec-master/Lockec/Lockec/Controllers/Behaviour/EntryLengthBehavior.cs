namespace Lockec.Controllers.Behaviour
{
    using Xamarin.Forms;

    public class EntryLengthBehavior : Behavior<Entry>
    {
        public int MaxLength { get; set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            var isValid = (entry.Text.Length == this.MaxLength);
            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        }
    }
}
