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
          var newRound = new Round();
          App.Database.SaveRound(newRound);

          Navigation.PushAsync(new ViewRoundPage { BindingContext = newRound });
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
