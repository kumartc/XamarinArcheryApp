using System;
using System.IO;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using XamarinArcheryApp.Droid.DataAccess;
using XamarinArcheryApp.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(implementorType: typeof(SQLiteDroid))]

namespace XamarinArcheryApp.Droid.DataAccess
{
  public class SQLiteDroid : ISQLite
  {
    public SQLiteConnection GetConnection()
    {
      const string sqliteFilename = "ArcheryDatabase.db3";
      string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); 
      var path = Path.Combine(documentsPath, sqliteFilename);

//TODO: Handle DB Reset differently
#if DEBUG
      if (File.Exists(path))
      {
        File.Delete(path);
      }
#endif
      var conn = new SQLiteConnection(new SQLitePlatformAndroid(), path);  

      return conn;
    }
  }
}