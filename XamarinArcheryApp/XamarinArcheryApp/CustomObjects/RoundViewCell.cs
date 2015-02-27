using XamarinArcheryApp.ValueConverters;
using Xamarin.Forms;

namespace XamarinArcheryApp.CustomObjects
{
  class RoundViewCell : ViewCell
  {
    public RoundViewCell()
    {
      var roundName = new Label
      {
        HorizontalOptions = LayoutOptions.Start,
        Text = "Round Name",
        FontSize = Font.SystemFontOfSize(NamedSize.Large).FontSize,
        Font = Font.SystemFontOfSize(NamedSize.Large)
      };

      roundName.SetBinding(Label.TextProperty, new Binding("Name"));

      var roundDateTime = new Label
      {
        HorizontalOptions = LayoutOptions.EndAndExpand,
        XAlign = TextAlignment.End,
        Text = "Round DateTime",
        FontSize = Font.SystemFontOfSize(NamedSize.Small).FontSize,
        Font = Font.SystemFontOfSize(NamedSize.Small)
      };

      roundDateTime.SetBinding(Label.TextProperty, new Binding("DateTime", BindingMode.OneWay, converter: new DateTimeToStringValueConverter()));

      View = new StackLayout
      {
        Orientation = StackOrientation.Horizontal,
        VerticalOptions = LayoutOptions.StartAndExpand,
        Padding = new Thickness(15, 5, 5, 5),
        Children =
        {
          roundName,
          roundDateTime
        }
      };
    }
  }
}
