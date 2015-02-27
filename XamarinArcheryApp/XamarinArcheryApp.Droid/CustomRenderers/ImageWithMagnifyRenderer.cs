using Android.Graphics;
using Android.OS;
using Android.Views;
using Java.Lang;
using XamarinArcheryApp.CustomObjects;
using XamarinArcheryApp.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(ImageWithMagnify), typeof(ImageWithMagnifyRenderer))]

namespace XamarinArcheryApp.Droid.CustomRenderers
{
  public class ImageWithMagnifyRenderer : ImageRenderer, GestureDetector.IOnGestureListener
  {
    private const float SCROLL_THRESHOLD = 100F;

    private Bitmap _mBitmap;
    private BitmapShader _mShader;
    private Paint _mPaint;
    private bool _isZooming;
    private float _xPos, _yPos, _downXPos, _downYPos;

    private GestureDetector _gestureDetector;

    private Handler _handler;
    private Runnable _onLongPressRunnable;

    public ImageWithMagnifyRenderer()
    {
      DrawingCacheEnabled = true;
      this.SetWillNotDraw(false);
      _gestureDetector = new GestureDetector(Context, this);

      _handler = new Handler();
      _onLongPressRunnable = new Runnable(() =>
      {
        _isZooming = true;
        this.Invalidate();
      });
    }

    public override void Draw(Canvas canvas)
    {
      base.Draw(canvas);
      
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
        Matrix.PreScale(magnifyScale, magnifyScale, boxLeft , boxTop);
        Matrix.PostTranslate((-_xPos * magnifyScale) + (boxWidth / 2F) ,  (-_yPos * magnifyScale) + (boxHeight / 2F));
        _mPaint.Shader.SetLocalMatrix(Matrix);

        var borderPaint = new Paint();
        borderPaint.SetStyle(Paint.Style.Stroke);
        borderPaint.Color = Color.Black;
        borderPaint.StrokeWidth = 3.0F;

        canvas.DrawRect(boxLeft, boxTop, boxRight, boxBottom, _mPaint);
        canvas.DrawRect(boxLeft, boxTop, boxRight, boxBottom, borderPaint);

        canvas.DrawCircle(boxLeft + (boxWidth / 2.0F), boxTop + (boxHeight / 2.0F), 4.0F, borderPaint);
      }
    }

    public override bool OnTouchEvent(MotionEvent e)
    {
      if (_mBitmap == null)
      {
        _mBitmap = Bitmap.CreateBitmap(this.GetDrawingCache(true));
        _mShader = new BitmapShader(_mBitmap, Shader.TileMode.Clamp, Shader.TileMode.Clamp);
        _mPaint = new Paint();
        _mPaint.SetShader(_mShader);
      }

      _xPos = e.GetX();
      _yPos = e.GetY();

      _gestureDetector.OnTouchEvent(e);

      switch (e.Action)
      {
        case MotionEventActions.Down:
          _downXPos = _xPos;
          _downYPos = _yPos;
          _handler.PostDelayed(_onLongPressRunnable, 300);
          return true;
          break;
        case MotionEventActions.Move:
          if (Math.Abs(_downXPos - _xPos) > SCROLL_THRESHOLD || Math.Abs(_downYPos - _yPos) > SCROLL_THRESHOLD)
          {
            _handler.RemoveCallbacks(_onLongPressRunnable);
          }
          if (_isZooming)
          {
            this.Invalidate();
            return true;
          }

          break;
        case MotionEventActions.Up:
        case MotionEventActions.Cancel:
          _handler.RemoveCallbacks(_onLongPressRunnable);
          _isZooming = false;
          this.Invalidate();
          break;
      }
      return base.OnTouchEvent(e);
    }

    public bool OnDown(MotionEvent e)
    {
      return true;
    }

    public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
    {
      return false;
    }

    public void OnLongPress(MotionEvent e)
    {
    }

    public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
    {
      return false;
    }

    public void OnShowPress(MotionEvent e)
    {
    }

    public bool OnSingleTapUp(MotionEvent e)
    {
      return false;
    }

    public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
    {
      // Raw height and width of image
      float height = options.OutHeight;
      float width = options.OutWidth;
      double inSampleSize = 1D;

      if (height > reqHeight || width > reqWidth)
      {
        int halfHeight = (int)(height / 2);
        int halfWidth = (int)(width / 2);

        // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
        while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
        {
          inSampleSize *= 2;
        }
      }

      return (int)inSampleSize;
    }
  }

}