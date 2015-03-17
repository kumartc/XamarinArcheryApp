using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using XamarinArcheryApp.Interfaces;
using XamarinArcheryApp.Model;
using Xamarin.Forms;

namespace XamarinArcheryApp.Data
{
  public class ArcheryDatabase
  {
    static readonly object _locker = new object();

    private readonly SQLiteConnection _database;

    public ArcheryDatabase()
    {
      _database = DependencyService.Get<ISQLite>().GetConnection();
      
      _database.CreateTable<Round>();
      _database.CreateTable<End>();
      //_database.CreateTable<Shot>();
      //_database.CreateTable<Arrow>();
      _database.CreateTable<Target>();

      //If Round Table is Empty, Create Sample Data
      if (!_database.Table<Round>().Any())
      {
        var sample = new Round()
        {
          Name = "Test Round 1",
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

        _database.RunInTransaction(() =>
        {
          _database.InsertWithChildren(sample, true);
          sample.Name = "Test Round 2";
          _database.InsertWithChildren(sample, true);
          sample.Name = "Test Round 3";
          _database.InsertWithChildren(sample, true);
          sample.Name = "Test Round 4";
          _database.InsertWithChildren(sample, true);
        });
      }
    }

    #region Arrow Operations

    public IEnumerable<Arrow> GetArrows()
    {
      lock (_locker)
      {
        return _database.Table<Arrow>();
      }
    }

    public Arrow GetArrow(int id)
    {
      lock (_locker)
      {
        return _database.Get<Arrow>(id);
      }
    }

    public int SaveArrow(Arrow arrow)
    {
      lock (_locker)
      {
        if (arrow.Id != 0)
        {
          _database.Update(arrow);
          return arrow.Id;
        }
        else
        {
          return _database.Insert(arrow);
        }
      }
    }

    public int DeleteArrow(int id)
    {
      lock (_locker)
      {
        return _database.Delete<Arrow>(id);
      }
    }

    #endregion

    #region End Operations

    public IEnumerable<End> GetEnds()
    {
      lock (_locker)
      {
        return _database.Table<End>();
      }
    }

    public End GetEnd(int id)
    {
      lock (_locker)
      {
        return _database.Get<End>(id);
      }
    }

    public int SaveEnd(End end)
    {
      lock (_locker)
      {
        if (end.Id != 0)
        {
          _database.Update(end);
          return end.Id;
        }
        else
        {
          return _database.Insert(end);
        }
      }
    }

    public int DeleteEnd(int id)
    {
      lock (_locker)
      {
        return _database.Delete<End>(id);
      }
    }

    #endregion

    #region Round Operations

    public IEnumerable<Round> GetRounds()
    {
      lock (_locker)
      {
        return _database.GetAllWithChildren<Round>();
      }
    }
    
    public Round GetRound(int id)
    {
      lock (_locker)
      {
        return _database.GetWithChildren<Round>(id);
      }
    }

    public int SaveRound(Round round)
    {
      lock (_locker)
      {
        if (round.Id != 0)
        {
          _database.UpdateWithChildren(round);
          return round.Id;
        }
        else
        {
          _database.InsertOrReplaceWithChildren(round);
          return round.Id;
        }
      }
    }

    public int DeleteRound(int id)
    {
      lock (_locker)
      {
        return _database.Delete<Round>(id);
      }
    }

    #endregion

    #region Target Operations

    public IEnumerable<Target> GetTargets()
    {
      lock (_locker)
      {
        return _database.Table<Target>();
      }
    }

    public Target GetTarget(int id)
    {
      lock (_locker)
      {
        return _database.Get<Target>(id);
      }
    }

    public int SaveTarget(Target target)
    {
      lock (_locker)
      {
        if (target.Id != 0)
        {
          _database.Update(target);
          return target.Id;
        }
        else
        {
          return _database.Insert(target);
        }
      }
    }

    public int DeleteTarget(int id)
    {
      lock (_locker)
      {
        return _database.Delete<Target>(id);
      }
    }

    #endregion

    #region Shot Operations

    public IEnumerable<Shot> GetShots()
    {
      lock (_locker)
      {
        return _database.Table<Shot>();
      }
    }

    public Shot GetShot(int id)
    {
      lock (_locker)
      {
        return _database.Get<Shot>(id);
      }
    }

    public int SaveShot(Shot shot)
    {
      lock (_locker)
      {
        if (shot.Id != 0)
        {
          _database.Update(shot);
          return shot.Id;
        }
        else
        {
          return _database.Insert(shot);
        }
      }
    }

    public int DeleteShot(int id)
    {
      lock (_locker)
      {
        return _database.Delete<Shot>(id);
      }
    }

    #endregion
  }
}