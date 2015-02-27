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
        var roundPage = new ViewRoundPage {BindingContext = roundItem};

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

      #region Toolbar (Android Specific)

      if (Device.OS == TargetPlatform.Android)
      {
        //Add Round Toolbar Button
        ToolbarItem addRoundToolbarItem = new ToolbarItem("Start New Round", "ic_action_new", () =>
        {
          var newRound = new Round();
          App.Database.SaveRound(newRound);

          Navigation.PushAsync(new ViewRoundPage { BindingContext = newRound });
        }, ToolbarItemOrder.Primary, 0);

        ToolbarItems.Add(addRoundToolbarItem);
      }
      #endregion
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();
      listView.ItemsSource = App.Database.GetRounds();
    }
  }
}