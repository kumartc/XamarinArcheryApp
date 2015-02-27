using System.Collections.Generic;
using System.ComponentModel;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace XamarinArcheryApp.Model
{
  [Table("Arrows")]
  public class Arrow : INotifyPropertyChanged
  {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<Shot> Shots { get; set; }

    public string Name { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
