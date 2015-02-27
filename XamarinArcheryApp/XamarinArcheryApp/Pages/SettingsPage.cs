using Xamarin.Forms;

namespace XamarinArcheryApp.Pages
{
  public class SettingsPage : BaseToolbarPage
  {
    public SettingsPage()
    {
      Title = "Settings";

      Content = new Label
      {
        Text = "Placeholder Settings Page",
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.Center
      };
    }
  }
}