using System.ComponentModel;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace XamarinArcheryApp.Model
{
  [Table("Shots")]
  public class Shot : INotifyPropertyChanged
  {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [ManyToOne(CascadeOperations = CascadeOperation.All)]
    public Arrow Arrow { get; set; }

    [ForeignKey(typeof(Arrow))]
    public int ArrowId { get; set; }

    [ManyToOne(CascadeOperations = CascadeOperation.All)]
    public End End { get; set; }

    [ForeignKey(typeof(End))]
    public int EndId { get; set; }

    public int Score { get; set; }

    public float XPos { get; set; }
    public float YPos { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}