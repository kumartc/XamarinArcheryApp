using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace XamarinArcheryApp.Model
{
  [Table("Rounds")]
  public class Round : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public Round()
    {
      DateTime = DateTime.UtcNow;
      Name = "Vegas Round";
    }

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public DateTime DateTime { get; set; }

    public string Name { get; set; }

    [ManyToOne(CascadeOperations = CascadeOperation.All)]
    public Target Target { get; set; }

    [ForeignKey(typeof(Target))]
    public int TargetId { get; set; }
   
    //[OneToMany(CascadeOperations = CascadeOperation.All)]
    [Ignore]
    public List<End> Ends { get; set; }

    [Ignore]
    public int RoundScore
    {
      get { return Ends.Sum(e => e.EndScore); }
    }

  }
}