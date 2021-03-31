using System;
using System.Text;
using SkiaSharp;

namespace eHS.Portal.Extensions
{
  public static class SkiaSharpExtensions
  {
    public static int LineCount(this SKCanvas _,
      string text,
      SKRect bounds,
      SKPaint paint)
    {
      if (string.IsNullOrEmpty(text?.Trim()))
      {
        return 0;
      }

      var result = 0;

      int i = 0;
      do
      {
        var stripped = StripWord(text, i, bounds.Width, paint);

        i += stripped.Length;

        result++;

      } while (i < text.Length);

      return result;
    }

    static string StripWord(string text, int fromIndex, float width, SKPaint paint)
    {
      var res = text.Substring(fromIndex);

      while(Math.Floor(paint.MeasureText(res)) > width)
      {
        int stripIndex = res.LastIndexOf(" ");
        if (stripIndex == -1)
        {
          break;
        }

        res = res.Substring(0, stripIndex);
      }

      return res;
    }
  }
}
