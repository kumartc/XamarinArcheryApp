using System;
using Android.Graphics;
using Android.Views;
using Xamarin.Forms;
using XamarinArcheryApp.CustomObjects;
using XamarinArcheryApp.Droid.CustomRenderers;
using XamarinArcheryApp.Enums;

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
          //_score = GetScoreFromCoordinates(xPos, yPos);
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
          paint.Color = Android.Graphics.Color.Black,
          paint.TextSize = 50F,
          paint.FakeBoldText = true;
          canvas.DrawText(_score.ToString(), 15, 55, paint);
        }
      }
    }

    //Find the score associted with the color in the color map at the x/y coordinates. 
    private VegasScore GetScoreFromCoordinates(float xPos, float yPos)
    {
      throw new System.NotImplementedException();
    }
  }
}