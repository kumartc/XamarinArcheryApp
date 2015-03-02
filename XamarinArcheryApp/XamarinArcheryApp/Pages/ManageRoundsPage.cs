using System.Linq;
using XamarinArcheryApp.CustomObjects;
using XamarinArcheryApp.Model;
using Xamarin.Forms;

namespace XamarinArcheryApp.Pages
{
  public class ManageRoundsPage : BaseToolbarPage
  {
    ListView listView;

    public ManageRoundsPage()
    {
      Title = "Rounds";

      listView = new ListView
      {
        ItemTemplate = new DataTemplate(typeof (RoundViewCell))
      };

      listView.ItemSelected += (sender, args) =>
      {
        var roundItem = (Round) args.SelectedItem;
        var roundPage = new ViewRoundPage() {BindingContext = roundItem};

        Navigation.PushAsync(roundPage);
      };
      
      Content = new StackLayout
      {
        Children =
        {
          listView
        },
        VerticalOptions = LayoutOptions.FillAndExpand
      };
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();
      listView.ItemsSource = App.Database.GetRounds();
    }
  }
}