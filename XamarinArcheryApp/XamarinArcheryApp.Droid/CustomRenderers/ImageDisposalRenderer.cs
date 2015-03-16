using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinArcheryApp.CustomObjects;
using XamarinArcheryApp.Droid.CustomRenderers;

[assembly: ExportRenderer(typeof(ImageWithDispose), typeof(ImageDisposalRenderer))]

namespace XamarinArcheryApp.Droid.CustomRenderers
{
  public class ImageDisposalRenderer : ImageRenderer
  {
    Page _page;
    NavigationPage _navigPage;

    protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
    {
      base.OnElementChanged(e);
      //On initial load
      if (e.OldElement == null)
      {
        //If in a view - Not sure if this check is absolutely necessary
        if (GetContainingView(e.NewElement) != null)
        {
          //Get the Page
          _page = GetParentPage(e.NewElement);

          //If Tabbed, set the Tabbed Disappearing Handler
          if (_page.Parent is TabbedPage || _page.Parent is CarouselPage)
          {
            _page.Disappearing += PageContainedInTabbedPageDisapearing;
            return;
          }

          //Else a Nav Page, set the Nav Disappearing Handler
          _navigPage = GetParentNavigationPage(_page);
          if (_navigPage != null)
            _navigPage.Popped += OnNavPagePopped;

          //TODO: Need to handle single page disposal of image?
          //TODO: Need to handle unfocus events?
        }
        else if ((_page = GetParentTabbedPage(e.NewElement)) != null)
        {
          _page.Disappearing += PageContainedInTabbedPageDisapearing;
        }
      }
    }

    void PageContainedInTabbedPageDisapearing(object sender, EventArgs e)
    {
      this.Dispose(true);
      _page.Disappearing -= PageContainedInTabbedPageDisapearing;
    }

    private void OnNavPagePopped(object s, NavigationEventArgs e)
    {
      //Make sure the page is the parent page
      if (e.Page == _page)
      {
        this.Dispose(true);
        _navigPage.Popped -= OnNavPagePopped;
      }
    }

    private Page GetParentPage(Element element)
    {
      Element parentElement = element.ParentView;

      if (parentElement is Page)
        return (Page)parentElement;
      else
        return GetParentPage(parentElement);
    }

    private View GetContainingView(Element element)
    {
      Element parentElement = element.Parent;

      if (parentElement == null)
        return null;

      if (parentElement is View)
        return (View)parentElement;
      else
        return GetContainingView(parentElement);
    }

    private TabbedPage GetParentTabbedPage(Element element)
    {
      Element parentElement = element.Parent;

      if (parentElement == null)
        return null;

      if (parentElement is TabbedPage)
        return (TabbedPage)parentElement;
      else
        return GetParentTabbedPage(parentElement);
    }

    private NavigationPage GetParentNavigationPage(Element element)
    {
      Element parentElement = element.Parent;

      if (parentElement == null)
        return null;

      if (parentElement is NavigationPage)
        return (NavigationPage)parentElement;
      else
        return GetParentNavigationPage(parentElement);
    }
  }
}