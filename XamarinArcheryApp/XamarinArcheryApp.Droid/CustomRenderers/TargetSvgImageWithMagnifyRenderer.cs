using System;
using Android.Graphics;
using Android.Views;
using Xamarin.Forms;
using XamarinArcheryApp.CustomObjects;
using XamarinArcheryApp.Droid.CustomRenderers;
using XamarinArcheryApp.Enums;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(TargetSvgImageWithMagnify), typeof(TargetSvgImageWithMagnifyRenderer))]

namespace XamarinArcheryApp.Droid.CustomRenderers
{
  public class TargetSvgImageWithMagnifyRenderer : SvgImageWithMagnifyRenderer
  {
    private VegasScore _score;
    private bool _showScore;

    public override bool OnTouchEvent(MotionEvent e)
    {
      //Get Touch Location
      var xPos = e.GetX();
      var yPos = e.GetY();

      switch (e.Action)
      {
        case MotionEventActions.Down:
          _showScore = true;
          break;
        case MotionEventActions.Move:
          //If down or move, get the score value for the x/y and display somewhere
          _score = GetScoreFromCoordinates(xPos, yPos);
          break;
        case MotionEventActions.Up:
        //Will trigger the score logging action later on
        case MotionEventActions.Cancel:
          _showScore = false;
          break;
      }

      return base.OnTouchEvent(e);
    }

    protected override void DispatchDraw(Canvas canvas)
    {
      base.DispatchDraw(canvas);

      if (_showScore)
      {
        using (var paint = new Paint())
        {
          paint.Color = Android.Graphics.Color.Black;
          paint.TextSize = 50F;
          paint.FakeBoldText = true;
          canvas.DrawText(_score.ToString(), 15, 55, paint);
        }
      }
    }

    //Find the score associted with the color in the color map at the x/y coordinates. 
    private VegasScore GetScoreFromCoordinates(float xPos, float yPos)
    {
      var result = VegasScore.Miss; 

      var pixel = ScoreMaskBitmap.GetPixel((int)Math.Round(xPos), (int)Math.Round(yPos));

      var red = Color.GetRedComponent(pixel);
      var green = Color.GetGreenComponent(pixel);
      var blue = Color.GetBlueComponent(pixel);

      if (blue == 255 && red == 153 && green == 153)
      {
        result = VegasScore.X;
      }
      else if (blue > 230)
      {
        result = VegasScore.Ten;
      }
      else if (blue > 204)
      {
        result = VegasScore.Nine;
      }
      else if (blue > 178)
      {
        result = VegasScore.Eight;
      }
      else if (blue > 153)
      {
        result = VegasScore.Seven;
      }
      else if (blue > 0)
      {
        result = VegasScore.Six;
      }

      return result;
    }
  }
}