using Android.Graphics;
using Android.OS;
using Android.Views;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinArcheryApp.CustomObjects;
using XamarinArcheryApp.Droid.CustomRenderers;
using Color = Android.Graphics.Color;
using Math = System.Math;

[assembly: ExportRenderer(typeof(ImageWithMagnify), typeof(ImageWithMagnifyRenderer))]

namespace XamarinArcheryApp.Droid.CustomRenderers
{
  public class ImageWithMagnifyRenderer : ImageDisposalRenderer
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
      this.DrawingCacheEnabled = true;

      _handler = new Handler();
      _onLongPressRunnable = new Runnable(() =>
      {
        //Triggered after a successful long press. Sets zoom flag to true and calls redraw which will in turn draw the magnification pane
        _isZooming = true;
        this.Invalidate();
      });
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
    {
      base.OnElementChanged(e);

      //On First Load
      if (e.OldElement == null)
      {
        //
      }
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
        borderPaint.Color = Color.Black;
        borderPaint.StrokeWidth = 3.0F;

        //Draw magnified picture
        canvas.DrawRect(boxLeft, boxTop, boxRight, boxBottom, _mPaint);
        //Draw border
        canvas.DrawRect(boxLeft, boxTop, boxRight, boxBottom, borderPaint);
        //Draw indicator in center
        canvas.DrawCircle(boxLeft + (boxWidth / 2.0F), boxTop + (boxHeight / 2.0F), 4.0F, borderPaint);

        borderPaint.Dispose();
      }
    }

    public override bool OnTouchEvent(MotionEvent e)
    {
      //Set up Magnification Shader/Paint
      //I don't think this is an ideal implementation
      //Had to implement here because the drawing cache wasn't available in the constructor.
      //TODO: Check to see if there is a better override for the Paint/Shader initialization
      if (_mPaint == null)
      {
        //TODO: Error handling if the cache/bitmap can't be processed
        //TODO: Verify that this is the best way to access the image bitmap
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

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if(_mShader != null)
          _mShader.Dispose();
        if (_mPaint != null)
        _mPaint.Dispose();
        if (_handler != null)
        _handler.Dispose();
        if (_onLongPressRunnable != null)
        _onLongPressRunnable.Dispose();
      }

      base.Dispose(disposing);
    }
  }
}