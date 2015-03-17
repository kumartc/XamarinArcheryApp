using System.Collections.Generic;
using Xamarin.Forms;
using XamarinArcheryApp.Model;

namespace XamarinArcheryApp.Pages
{
  public class BaseToolbarPage : ContentPage
  {
    public BaseToolbarPage()
    {
      #region Toolbar (Android Specific)

      if (Device.OS == TargetPlatform.Android)
      {
        //Add Round Toolbar Button
        ToolbarItem addRoundToolbarItem = new ToolbarItem("Start New Round", "ic_action_new", () =>
        {
          var newRound = new Round()
          {
            Name = "New Vegas Round",
            Target = new Target { Name = "Vegas 3 Spot Target" },
            Ends = new List<End>
          {
            new End { EndNo = 1 },
            new End { EndNo = 2 },
            new End { EndNo = 3 },
            new End { EndNo = 4 },
            new End { EndNo = 5 },
            new End { EndNo = 6 },
            new End { EndNo = 7 },
            new End { EndNo = 8 },
            new End { EndNo = 9 },
            new End { EndNo = 10 },
          }
          };

          App.Database.SaveRound(newRound);

          Navigation.PushAsync(new ViewRoundCarouselPage(newRound.Id));
        }, ToolbarItemOrder.Primary, 2);

        ToolbarItems.Add(addRoundToolbarItem);

        //Manage Rounds Toolbar Button
        ToolbarItem manageRoundsToolbarItem = new ToolbarItem("Manage Rounds", "ic_action_storage", () =>
        {
          if (this.GetType() != typeof(ManageRoundsPage))
          {
            Navigation.PushAsync(new ManageRoundsPage());
          }
        }, ToolbarItemOrder.Primary, 1);

        ToolbarItems.Add(manageRoundsToolbarItem);

        //Reports Page Toolbar Button
        ToolbarItem reportsPageToolbarItem = new ToolbarItem("Reports", "ic_action_data_usage", () =>
        {
          if (this.GetType() != typeof(ReportHomePage))
          {
            Navigation.PushAsync(new ReportHomePage());
          }
        }, ToolbarItemOrder.Primary, 0);

        ToolbarItems.Add(reportsPageToolbarItem);

        //Settings Page Toolbar Button
        ToolbarItem settingsPageToolbarItem = new ToolbarItem("Settings", "ic_action_settings", () =>
        {
          if (this.GetType() != typeof(SettingsPage))
          {
            Navigation.PushAsync(new SettingsPage());
          }
        }, ToolbarItemOrder.Secondary, 0);

        ToolbarItems.Add(settingsPageToolbarItem);
      }
      #endregion
    }
  }
}
