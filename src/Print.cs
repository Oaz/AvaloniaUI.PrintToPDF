using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using Avalonia.Rendering;
using Avalonia.Skia;
using Avalonia.Skia.Helpers;
using Avalonia.VisualTree;
using SkiaSharp;

namespace AvaloniaUI.PrintToPDF
{
  public static class Print
  {
    public static void ToFile(string fileName, params IVisual[] visuals) => ToFile(fileName, visuals.AsEnumerable());
    public static void ToFile(string fileName, IEnumerable<IVisual> visuals)
    {
      using var doc = SKDocument.CreatePdf(fileName);
      foreach (var visual in visuals)
      {
        var bounds = visual.Bounds;
        var page = doc.BeginPage((float)bounds.Width, (float)bounds.Height);
        using var context = new DrawingContext(DrawingContextHelper.WrapSkiaCanvas(page, SkiaPlatform.DefaultDpi));
        ImmediateRenderer.Render(visual, context);
        doc.EndPage();
      }
      doc.Close();
    }
  }
}
