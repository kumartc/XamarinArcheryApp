using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace XamarinArcheryApp.Model
{
  [Table("Ends")]
  public class End : INotifyPropertyChanged
  {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int EndNo { get; set; }

    [ManyToOne(CascadeOperations = CascadeOperation.All)]
    public Round Round { get; set; }

    [ForeignKey(typeof(Round))]
    public int RoundId { get; set; }

    [Ignore]
    //[OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<Shot> Shots { get; set; }

    [Ignore]
    public int EndScore
    {
      get { return Shots.Sum(a => a.Score); }
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
