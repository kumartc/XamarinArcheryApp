using System.Linq;
using XamarinArcheryApp.CustomObjects;
using XamarinArcheryApp.Model;
using Xamarin.Forms;

namespace XamarinArcheryApp.Pages
{
  public class ManageRoundsPage : BaseToolbarPage
  {
    readonly ListView _listView;

    public ManageRoundsPage()
    {
      Title = "Rounds";

      _listView = new ListView
      {
        ItemTemplate = new DataTemplate(typeof (RoundViewCell))
      };

      _listView.ItemSelected += (sender, args) =>
      {
        var round = (Round)args.SelectedItem;

        var roundPage = new ViewRoundCarouselPage(round.Id);

        Navigation.PushAsync(roundPage);
      };
      
      Content = new StackLayout
      {
        Children =
        {
          _listView
        },
        VerticalOptions = LayoutOptions.FillAndExpand
      };
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();
      _listView.ItemsSource = App.Database.GetRounds();
    }
  }
}