using System;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Java.Lang;
using XamarinArcheryApp.CustomObjects;
using XamarinArcheryApp.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Math = Java.Lang.Math;

[assembly: ExportRenderer(typeof(ImageWithMagnify), typeof(ImageWithMagnifyRenderer))]

namespace XamarinArcheryApp.Droid.CustomRenderers
{
  public class ImageWithMagnifyRenderer : ImageRenderer
  {
    //Amount of finger movement allowed in a single long press
    private const float SCROLL_THRESHOLD = 100F;

    private BitmapShader _mShader;
    private Paint _mPaint;
    private bool _isZooming;
    private float _xPos, _yPos, _downXPos, _downYPos;

    //Handler and Runnable to help catch a long press event
    private Handler _handler;
    private Runnable _onLongPressRunnable;

    public ImageWithMagnifyRenderer()
    {
      //Enable drawing cache since we're using it to pull magnification bitmap
      DrawingCacheEnabled = true;
      
      _handler = new Handler();
      _onLongPressRunnable = new Runnable(() =>
      {
        //Triggered after a successful long press. Sets zoom flag to true and calls redraw which will in turn draw the magnification pane
        _isZooming = true;
        this.Invalidate();
      });
    }

    protected override void DispatchDraw(Canvas canvas)
    {
      base.DispatchDraw(canvas);

      //If zooming is enabled, draw the magnification frame
      if (_isZooming)
      {
        float magnifyScale = 4.0F;
        float boxWidth = 400F;
        float boxHeight = 400F;
        float boxLeft = 0F;
        float boxTop = 0F;
        float boxRight = boxLeft + boxWidth;
        float boxBottom = boxTop + boxHeight;


        Matrix.Reset();
        //Magnify Transformation
        Matrix.PreScale(magnifyScale, magnifyScale, boxLeft, boxTop);
        //Translate matrix to account for pivot point (easier with circles...)
        Matrix.PostTranslate((-_xPos * magnifyScale) + (boxWidth / 2F), (-_yPos * magnifyScale) + (boxHeight / 2F));
        _mPaint.Shader.SetLocalMatrix(Matrix);

        var borderPaint = new Paint();
        borderPaint.SetStyle(Paint.Style.Stroke);
        borderPaint.Color = Android.Graphics.Color.Black;
        borderPaint.StrokeWidth = 3.0F;

        //Draw magnified picture
        canvas.DrawRect(boxLeft, boxTop, boxRight, boxBottom, _mPaint);
        //Draw border
        canvas.DrawRect(boxLeft, boxTop, boxRight, boxBottom, borderPaint);
        //Draw indicator in center
        canvas.DrawCircle(boxLeft + (boxWidth / 2.0F), boxTop + (boxHeight / 2.0F), 4.0F, borderPaint);
      }
    }

    public override bool OnTouchEvent(MotionEvent e)
    {
      //Set up Magnification Shader/Paint
      //I don't think this is an ideal implementation
      //The references are too strong and are not being removed during GC
      //Had to implement here because the drawing cache wasn't available in the constructor.
      //TODO: Implement Paint/Shader properly as to allow GC
      //TODO: Check to see if there is a better override for the Paint/Shader initialization
      if (_mPaint == null)
      {
        //TODO: Error handling if the cache/bitmap can't be processed
        _mShader = new BitmapShader(this.GetDrawingCache(true), Shader.TileMode.Clamp, Shader.TileMode.Clamp);
        _mPaint = new Paint();
        _mPaint.SetShader(_mShader);
      }

      //Get Touch Location
      _xPos = e.GetX();
      _yPos = e.GetY();

      switch (e.Action)
      {
        case MotionEventActions.Down:
          //User has started touching the image. Log the initial coordinates and set handler to trigger after a short delay
          //The handler will set the zoom flag and force a redraw if the callback hasn't been removed by then (up/cancel)
          _downXPos = _xPos;
          _downYPos = _yPos;
          _handler.PostDelayed(_onLongPressRunnable, 300);
          return true;
          break;
        case MotionEventActions.Move:
          //User has moved their finger. Get the difference from the original down position and compare to the SCROLL_THRESHOLD
          //If it exceeds the scroll threshold, then remove the callback on the handler to prevent the magnification from triggering
          if (Math.Abs(_downXPos - _xPos) > SCROLL_THRESHOLD || Math.Abs(_downYPos - _yPos) > SCROLL_THRESHOLD)
          {
            _handler.RemoveCallbacks(_onLongPressRunnable);
          }

          //If the zoom flag is true, redraw to account for new finger coordinates
          if (_isZooming)
          {
            this.Invalidate();
          }
          break;
        case MotionEventActions.Up:
        case MotionEventActions.Cancel:
          //If user has lifted their finger/cancelled: remove the callback on the handler, set zoom to false, redraw
          _handler.RemoveCallbacks(_onLongPressRunnable);
          _isZooming = false;
          this.Invalidate();
          break;
      }
      return base.OnTouchEvent(e);
    }

    ///// <summary>
    ///// Determines the proper sample size to scale the picture to the view
    ///// </summary>
    ///// <param name="originalHeight"></param>
    ///// <param name="originalWidth"></param>
    ///// <param name="reqHeight"></param>
    ///// <param name="reqWidth"></param>
    ///// <returns></returns>
    //public static int CalculateSampleSize(int originalHeight, int originalWidth, int reqHeight, int reqWidth)
    //{
    //  //Set default sample size result
    //  int inSampleSize = 1;

    //  //If the image is larger than the requested size, we'll need to calculate the scale. 
    //  if (originalHeight > reqHeight || originalWidth > reqWidth)
    //  {
    //    int halfHeight = originalHeight / 2;
    //    int halfWidth = originalWidth / 2;

    //    // Calculate the largest sample size value that is a power of 2 and keeps both
    //    // height and width larger than the requested height and width.
    //    while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
    //    {
    //      inSampleSize *= 2;
    //    }
    //  }

    //  return inSampleSize;
    //}


    //Thanks to Avrohom for help - Force Dispose of Image
    Page _page;
    NavigationPage _navigPage;

    protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
    {
      base.OnElementChanged(e);
      if (e.OldElement == null)
      {
        if (GetContainingView(e.NewElement) != null)
        {
          _page = GetContainingPage(e.NewElement);
          if (_page.Parent is TabbedPage)
          {
            _page.Disappearing += PageContainedInTabbedPageDisapearing;
            return;
          }

          _navigPage = GetContainingNavigationPage(_page);
          if (_navigPage != null)
            _navigPage.Popped += OnPagePopped;
        }
        else if ((_page = GetContainingTabbedPage(e.NewElement)) != null)
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

    private void OnPagePopped(object s, NavigationEventArgs e)
    {
      if (e.Page == _page)
      {
        this.Dispose(true);
        _navigPage.Popped -= OnPagePopped;
      }
    }

    private Page GetContainingPage(Xamarin.Forms.Element element)
    {
      Element parentElement = element.ParentView;

      if (parentElement is Page)
        return (Page)parentElement;
      else
        return GetContainingPage(parentElement);
    }

    private Xamarin.Forms.View GetContainingView(Xamarin.Forms.Element element)
    {
      Element parentElement = element.Parent;

      if (parentElement == null)
        return null;

      if (parentElement is Xamarin.Forms.View)
        return (Xamarin.Forms.View)parentElement;
      else
        return GetContainingView(parentElement);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
    }

    private TabbedPage GetContainingTabbedPage(Xamarin.Forms.Element element)
    {
      Element parentElement = element.Parent;

      if (parentElement == null)
        return null;

      if (parentElement is TabbedPage)
        return (TabbedPage)parentElement;
      else
        return GetContainingTabbedPage(parentElement);
    }

    private NavigationPage GetContainingNavigationPage(Xamarin.Forms.Element element)
    {
      Element parentElement = element.Parent;

      if (parentElement == null)
        return null;

      if (parentElement is NavigationPage)
        return (NavigationPage)parentElement;
      else
        return GetContainingNavigationPage(parentElement);
    }



  }
}