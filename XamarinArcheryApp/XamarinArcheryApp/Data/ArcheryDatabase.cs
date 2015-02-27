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
    static object _locker = new object();

    private SQLiteConnection database;

    public ArcheryDatabase()
    {
      database = DependencyService.Get<ISQLite>().GetConnection();
      
      database.CreateTable<Round>();
      //database.CreateTable<End>();
      //database.CreateTable<Shot>();
      //database.CreateTable<Arrow>();
      database.CreateTable<Target>();

      //If Round Table is Empty, Create Sample Data
      if (!database.Table<Round>().Any())
      {
        var sample = new Round()
        {
          Name = "Test Round 1",
          Target = new Target { Name = "Vegas 3 Spot Target" },
          Ends = new List<End>()
        };

        database.RunInTransaction(() =>
        {
          database.InsertWithChildren(sample, true);
          sample.Name = "Test Round 2";
          database.InsertWithChildren(sample, true);
          sample.Name = "Test Round 3";
          database.InsertWithChildren(sample, true);
          sample.Name = "Test Round 4";
          database.InsertWithChildren(sample, true);
        });
      }
    }

    #region Arrow Operations

    public IEnumerable<Arrow> GetArrows()
    {
      lock (_locker)
      {
        return database.Table<Arrow>();
      }
    }

    public Arrow GetArrow(int id)
    {
      lock (_locker)
      {
        return database.Get<Arrow>(id);
      }
    }

    public int SaveArrow(Arrow arrow)
    {
      lock (_locker)
      {
        if (arrow.Id != 0)
        {
          database.Update(arrow);
          return arrow.Id;
        }
        else
        {
          return database.Insert(arrow);
        }
      }
    }

    public int DeleteArrow(int id)
    {
      lock (_locker)
      {
        return database.Delete<Arrow>(id);
      }
    }

    #endregion

    #region End Operations

    public IEnumerable<End> GetEnds()
    {
      lock (_locker)
      {
        return database.Table<End>();
      }
    }

    public End GetEnd(int id)
    {
      lock (_locker)
      {
        return database.Get<End>(id);
      }
    }

    public int SaveEnd(End end)
    {
      lock (_locker)
      {
        if (end.Id != 0)
        {
          database.Update(end);
          return end.Id;
        }
        else
        {
          return database.Insert(end);
        }
      }
    }

    public int DeleteEnd(int id)
    {
      lock (_locker)
      {
        return database.Delete<End>(id);
      }
    }

    #endregion

    #region Round Operations

    public IEnumerable<Round> GetRounds()
    {
      lock (_locker)
      {
        return database.Table<Round>();
      }
    }

    public Round GetRound(int id)
    {
      lock (_locker)
      {
        return database.Get<Round>(id);
      }
    }

    public int SaveRound(Round round)
    {
      lock (_locker)
      {
        if (round.Id != 0)
        {
          database.Update(round);
          return round.Id;
        }
        else
        {
          return database.Insert(round);
        }
      }
    }

    public int DeleteRound(int id)
    {
      lock (_locker)
      {
        return database.Delete<Round>(id);
      }
    }

    #endregion

    #region Target Operations

    public IEnumerable<Target> GetTargets()
    {
      lock (_locker)
      {
        return database.Table<Target>();
      }
    }

    public Target GetTarget(int id)
    {
      lock (_locker)
      {
        return database.Get<Target>(id);
      }
    }

    public int SaveTarget(Target target)
    {
      lock (_locker)
      {
        if (target.Id != 0)
        {
          database.Update(target);
          return target.Id;
        }
        else
        {
          return database.Insert(target);
        }
      }
    }

    public int DeleteTarget(int id)
    {
      lock (_locker)
      {
        return database.Delete<Target>(id);
      }
    }

    #endregion

    #region Shot Operations

    public IEnumerable<Shot> GetShots()
    {
      lock (_locker)
      {
        return database.Table<Shot>();
      }
    }

    public Shot GetShot(int id)
    {
      lock (_locker)
      {
        return database.Get<Shot>(id);
      }
    }

    public int SaveShot(Shot shot)
    {
      lock (_locker)
      {
        if (shot.Id != 0)
        {
          database.Update(shot);
          return shot.Id;
        }
        else
        {
          return database.Insert(shot);
        }
      }
    }

    public int DeleteShot(int id)
    {
      lock (_locker)
      {
        return database.Delete<Shot>(id);
      }
    }

    #endregion
  }
}