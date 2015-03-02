using Xamarin.Forms;
using XamarinArcheryApp.Data;
using XamarinArcheryApp.Pages;

namespace XamarinArcheryApp
{
  public class App : Application
  {
    private static ArcheryDatabase _database;

    public static Size ScreenDimensions;

    public App()
    {
      MainPage = new NavigationPage(new StartPage());
    }

    public static ArcheryDatabase Database
    {
      get { return _database ?? (_database = new ArcheryDatabase()); }
    }

    protected override void OnStart()
    {
      // Handle when your app starts
    }

    protected override void OnSleep()
    {
      // Handle when your app sleeps
    }

    protected override void OnResume()
    {
      // Handle when your app resumes
    }
  }
}
