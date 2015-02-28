using System;
using Xamarin.Forms;
using XamarinArcheryApp.CustomObjects;
using XamarinArcheryApp.Model;

namespace XamarinArcheryApp.Pages
{
  class ViewRoundPage : ContentPage
  {
    private ImageWithMagnify targetImage;

    public ViewRoundPage()
    {
      this.SetBinding(TitleProperty, "Name");

      var nameLabel = new Label {Text = "Name: "};
      
      var nameEntry = new Entry();
      nameEntry.SetBinding(Entry.TextProperty, "Name");

      targetImage = new ImageWithMagnify
      {
        Source = "Vegas3SpotTarget",
        Aspect = Aspect.AspectFit,
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
          targetImage,
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
      //Possibly misguided attempts to clear memory. Still need to learn more about this.
      base.OnDisappearing();

      targetImage = null;

      Content = null;
      GC.Collect();
    }

   }
}
