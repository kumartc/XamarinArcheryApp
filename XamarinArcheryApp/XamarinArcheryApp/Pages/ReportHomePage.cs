using Xamarin.Forms;

namespace XamarinArcheryApp.Pages
{
  class ReportHomePage : BaseToolbarPage
  {
    public ReportHomePage()
    {
      Title = "Reports";

      Content = new Label
      {
        Text = "Placeholder Reports Home Page",
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.Center
      };
    }
  }
}
