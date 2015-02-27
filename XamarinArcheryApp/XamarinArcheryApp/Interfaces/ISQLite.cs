using SQLite.Net;

namespace XamarinArcheryApp.Interfaces
{
  public interface ISQLite
  {
    SQLiteConnection GetConnection();
  }
}