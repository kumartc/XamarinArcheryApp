using System;
using XamarinArcheryApp.Model;
using Xamarin.Forms;

namespace XamarinArcheryApp.Pages
{
  public class StartPage : BaseToolbarPage
  {
    public StartPage()
    {
      Title = "Start Page";     

      Content = new Label
      {
        Text = "Placeholder Start Page. Will show stats like average scores, number of shots.",
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.Center,
        XAlign = TextAlignment.Center
      };
    }
  }
}