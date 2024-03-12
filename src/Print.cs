using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Skia.Helpers;
using SkiaSharp;

namespace AvaloniaUI.PrintToPDF
{
  public static class Print
  {
    public static void ToFile(string fileName, params Visual[] visuals) => ToFile(fileName, visuals.AsEnumerable());
    public static void ToFile(string fileName, IEnumerable<Visual> visuals)
    {
      using var doc = SKDocument.CreatePdf(fileName);
      ToDocument(doc, visuals);
      doc.Close();
    }

    public static void ToStream(Stream stream, params Visual[] visuals) => ToStream(stream, visuals.AsEnumerable());
    public static void ToStream(Stream stream, IEnumerable<Visual> visuals)
    {
      using var doc = SKDocument.CreatePdf(stream);
      ToDocument(doc, visuals);
      doc.Close();
    }

    private static void ToDocument(SKDocument doc, IEnumerable<Visual> visuals)
    {
      foreach (var visual in visuals)
        {
          var bounds = visual.Bounds;
          var page = doc.BeginPage((float)bounds.Width, (float)bounds.Height);

          // In 11.0-11.1 this method can be safely block-waited,
          // But it is recommended to make ToFile async, as in the future this API will be GPU accelerated.
          DrawingContextHelper.RenderAsync(page, visual).GetAwaiter().GetResult();

          doc.EndPage();
       }
    }
  }
}
