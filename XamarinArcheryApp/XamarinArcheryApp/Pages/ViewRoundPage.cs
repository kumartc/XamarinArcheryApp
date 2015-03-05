using System;
using System.Reflection;
using SVG.Forms.Plugin.Abstractions;
using Xamarin.Forms;
using XamarinArcheryApp.CustomObjects;
using XamarinArcheryApp.Model;

namespace XamarinArcheryApp.Pages
{
  class ViewRoundPage : ContentPage
  {
    private TargetSvgImageWithMagnify _targetImage;

    public ViewRoundPage()
    {
      this.SetBinding(TitleProperty, "Name");

      var nameLabel = new Label {Text = "Name: "};
      
      var nameEntry = new Entry();
      nameEntry.SetBinding(Entry.TextProperty, "Name");

      _targetImage = new TargetSvgImageWithMagnify
      {
        SvgPath = "XamarinArcheryApp.Images.VegasTargetSvg.svg",
        ColorMapSvgPath = "XamarinArcheryApp.Images.VegasTargetSvg-ScoreMask.svg",
        SvgAssembly = typeof(App).GetTypeInfo().Assembly,
        HeightRequest = App.ScreenDimensions.Width,
        WidthRequest = App.ScreenDimensions.Width,
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.Center,
        BackgroundColor = Color.White
      };

      var deleteButton = new Button
      {
        Text = "Delete",
        HorizontalOptions = LayoutOptions.Start
      };
      deleteButton.Clicked += (sender, e) =>
      {
        var roundItem = (Round)BindingContext;
        App.Database.DeleteRound(roundItem.Id);
        this.Navigation.PopAsync();
      };

      var saveButton = new Button
      {
        Text = "Save",
        HorizontalOptions = LayoutOptions.EndAndExpand
      };
      saveButton.Clicked += (sender, e) =>
      {
        var roundItem = (Round)BindingContext;
        App.Database.SaveRound(roundItem);
        this.Navigation.PopAsync();
      };

      

      Content = new StackLayout
      {
        VerticalOptions = LayoutOptions.StartAndExpand,
        Padding = new Thickness(0),
        Children =
        {
          nameLabel, nameEntry,
          _targetImage,
          new StackLayout
          {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.Fill,
            Children =
            {
              deleteButton, saveButton
            }
          }
        }
      };
    }

    protected override void OnDisappearing()
    {
      _targetImage = null;
      base.OnDisappearing();

      //This shouldn't be necessary with the svg changes (or at all), but keeping in for now as I continue to optimize
      GC.Collect();
    }
  }
}
