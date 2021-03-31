using System;
using System.Collections.Generic;
using System.Linq;
using eHS.Portal.Extensions;
using eHS.Portal.Model;
using eHS.Portal.Models.Certificates;
using SkiaSharp;

namespace eHS.Portal.Helpers
{
  public class CertificateHelper
  {
    static Canvas _canvas = new Canvas();

    public static CertificateHolder Convert(Certificate certificate)
    {
      IList<ItemGroup> itemGroups = null;

      if (   certificate.Scheme == Scheme.FoodManufacturing
          && (certificate.Menus?.Any() ?? false))
      {
        itemGroups = new List<ItemGroup>();

        var groups = certificate.Menus.GroupBy(
          e => e.Group,
          e => e,
          (a, b) =>
          {
            return (a, b);
          })
          .OrderBy(e => e.a)
          .ToList();

        groups.ForEach(e =>
        {
          var index = 1;
          var lineCount = 0;

          var group = new ItemGroup
          {
            StartIndex = index
          };
          itemGroups.Add(group);

          var menus = e.b.OrderBy(b => b.Index).ToList();
          foreach (var i in menus)
          {
            var itemLineCount = _canvas.LineCount(i.Text?.Trim());

            if (lineCount + itemLineCount > 10)
            {
              group = new ItemGroup
              {
                StartIndex = index
              };
              itemGroups.Add(group);

              lineCount = 0;
            }

            lineCount += itemLineCount;
            group.Items.Add(i.Text);

            index++;
          }
        });
      }

      var scheme = certificate.SchemeText;
      var subScheme = certificate.SubSchemeText;

      if (certificate.Scheme == Scheme.FoodManufacturing)
      {
        scheme = subScheme;
        subScheme = null;
      }

      return new CertificateHolder
      {
        Template = certificate.Template,
        Number = certificate.Number,
        CustomerName = certificate.CustomerName,
        ExpiresOn = certificate.ExpiresOn,
        IssuedOn = certificate.IssuedOn,
        SerialNo = certificate.SerialNo,
        Scheme = $"{scheme} Scheme",
        SubScheme = subScheme,
        Premise = certificate.Premise,
        ItemGroups = itemGroups
      };
    }
  }

  class Canvas
  {
    const int Dimension = 552;

    readonly SKRect _rect = new SKRect(0, 0, Dimension, Dimension);

    readonly SKPaint _paint = new SKPaint
    {
      TextSize = 19,
      Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Normal)
    };

    readonly SKSurface _surface = SKSurface.Create(new SKImageInfo(Dimension, Dimension));

    public int LineCount(string text)
    {
      return _surface.Canvas.LineCount(text, _rect, _paint);
    }
  }
}
