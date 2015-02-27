using System.Collections.Generic;
using System.ComponentModel;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace XamarinArcheryApp.Model
{
  [Table("Targets")]
  public class Target : INotifyPropertyChanged
  {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }

    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<Round> Rounds { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}