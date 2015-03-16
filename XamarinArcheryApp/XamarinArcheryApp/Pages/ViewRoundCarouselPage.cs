using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinArcheryApp.Model;

namespace XamarinArcheryApp.Pages
{
  public class ViewRoundCarouselPage : CarouselPage
  {
    private Round round;
    private int pageIndex = 0;
    private int i = 0;

    public ViewRoundCarouselPage(int roundId)
    {
      round = App.Database.GetRound(roundId);
      
      Children.Add(new ViewEndPage { BindingContext = round.Ends[i++] });
      Children.Add(new ViewEndPage { BindingContext = round.Ends[i++] });
      Children.Add(new ViewEndPage { BindingContext = round.Ends[i++] });
      
    }

    protected override void OnCurrentPageChanged()
    {
      pageIndex = this.Children.IndexOf(this.CurrentPage);

      //If at end of the list...remove the first, add the second
      if (pageIndex == 2 && i < 10)
      {
        Children.RemoveAt(0);
        Children.Add(new ViewEndPage{ BindingContext = round.Ends[i++] });
      }

      //If at the beginning of the list, remove the end, add at beginning
      if (pageIndex == 0 && i > 3)
      {
        Children.RemoveAt(2);
        Children.Insert(0, new ViewEndPage { BindingContext = round.Ends[(--i) - 3] });
      }

      base.OnCurrentPageChanged();
    }
  }
}
