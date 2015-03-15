using System;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Widget;
using SVG.Forms.Plugin.Abstractions;
using SVG.Forms.Plugin.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinArcheryApp.CustomObjects;
using XamarinArcheryApp.Droid.CustomRenderers;

[assembly: ExportRenderer(typeof(SvgImageWithMagnify), typeof(SvgImageWithMagnifyRenderer))]

namespace XamarinArcheryApp.Droid.CustomRenderers
{
  public class SvgImageWithMagnifyRenderer : ImageWithMagnifyRenderer
  {
		private static bool _isGetBitmapExecuting;

		private SvgImage _formsControl {
			get {
				return Element as SvgImage;
			}
		}

    protected Bitmap ScoreMaskBitmap;

    protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
    {
      base.OnElementChanged(e);

      if (e.OldElement == null)
      {
        try
        {
          LoadBitmapFromSvg();
          LoadScoreMaskFromSvg();
        }
        catch (Exception ex)
        {
          System.Diagnostics.Debug.WriteLine("Problem setting image source {0}", ex);
        }
      }
    }

    private void LoadScoreMaskFromSvg()
    {
      Task.Run(async () =>
      {
        var width = (int)_formsControl.WidthRequest <= 0 ? 100 : (int)_formsControl.WidthRequest;
        var height = (int)_formsControl.HeightRequest <= 0 ? 100 : (int)_formsControl.HeightRequest;

        //Since you can only load one svg at a time, make sure the method is not already executing before calling it
        //Race?
        while (_isGetBitmapExecuting)
        {
          await Task.Delay(TimeSpan.FromMilliseconds(1));
        }

        _isGetBitmapExecuting = true;


        SvgImage colorMask = new SvgImage
        {
          SvgPath = ((TargetSvgImageWithMagnify)_formsControl).ColorMapSvgPath,
          SvgAssembly = _formsControl.SvgAssembly,
          HeightRequest = _formsControl.HeightRequest,
          WidthRequest = _formsControl.WidthRequest,
          HorizontalOptions = _formsControl.HorizontalOptions,
          VerticalOptions = _formsControl.VerticalOptions,
          BackgroundColor = _formsControl.BackgroundColor

        };

        //Spiking the size of the bitmap to improve quality of the image.
        return await BitmapService.GetBitmapAsync(colorMask, width * 3, height * 3);
      }).ContinueWith(taskResult =>
      {
        ScoreMaskBitmap = taskResult.Result;
        _isGetBitmapExecuting = false;
      }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public override SizeRequest GetDesiredSize (int widthConstraint, int heightConstraint)
		{
			return new SizeRequest (new Size (_formsControl.WidthRequest, _formsControl.WidthRequest));
		}

    private void LoadBitmapFromSvg()
		{
			Task.Run (async() => {
				var width = (int)_formsControl.WidthRequest <= 0 ? 100 : (int)_formsControl.WidthRequest;
				var height = (int)_formsControl.HeightRequest <= 0 ? 100 : (int)_formsControl.HeightRequest;

				//Since you can only load one svg at a time, make sure the method is not already executing before calling it
        //Race?
				while (_isGetBitmapExecuting) {
					await Task.Delay (TimeSpan.FromMilliseconds (1));
				}

				_isGetBitmapExecuting = true;
        //Spiking the size of the bitmap to improve quality of the image.
				return await BitmapService.GetBitmapAsync (_formsControl, width * 3, height * 3);
			}).ContinueWith (taskResult => {
				var imageView = new ImageView (Context);

				imageView.SetScaleType (ImageView.ScaleType.FitStart);
        imageView.DrawingCacheEnabled = true;
        imageView.SetImageBitmap (taskResult.Result);


				SetNativeControl (imageView);
				_isGetBitmapExecuting = false;
			}, TaskScheduler.FromCurrentSynchronizationContext ());
		}
	}
}