using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Skia;
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
      foreach (var visual in visuals)
      {
        var bounds = visual.Bounds;
        var page = doc.BeginPage((float)bounds.Width, (float)bounds.Height);
        var drawingContext = CreateContext( DrawingContextHelper.WrapSkiaCanvas(page, SkiaPlatform.DefaultDpi) );
        Render(visual, drawingContext);
        doc.EndPage();
      }
      doc.Close();
    }
    
    static Print()
    {
      var types = typeof(DrawingContext).Assembly.GetTypes();
      var pdc = types.First(t => t.Name=="PlatformDrawingContext");
      CreateContext = dci => (DrawingContext) Activator.CreateInstance(pdc!, new object[] {dci, true});
      var imrd = types.First(t => t.Name=="ImmediateRenderer");
      var render = imrd.GetMethod("Render", new Type[]{typeof(Visual), typeof(DrawingContext)})!;
      Render = (visual,context) => render.Invoke(null, new object[] { visual, context });
    }

    public static readonly Func<IDrawingContextImpl, DrawingContext> CreateContext;
    public static readonly Action<Visual, DrawingContext> Render;
  }
}
