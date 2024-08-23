using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Skia.Helpers;
using SkiaSharp;

namespace AvaloniaUI.PrintToPDF
{
  public static class Print
  {
    [Obsolete("Use async alternative")]
    public static void ToFile(string fileName, params Visual[] visuals) => ToFileAsync(fileName, visuals).GetAwaiter().GetResult();
    [Obsolete("Use async alternative")]
    public static void ToFile(string fileName, IEnumerable<Visual> visuals) => ToFileAsync(fileName, visuals).GetAwaiter().GetResult();
    
    public static async Task ToFileAsync(string fileName, params Visual[] visuals) => await ToFileAsync(fileName, visuals.AsEnumerable());
    public static async Task ToFileAsync(string fileName, IEnumerable<Visual> visuals)
    {
      using var doc = SKDocument.CreatePdf(fileName);
      await ToDocument(doc, visuals);
      doc.Close();
    }

    [Obsolete("Use async alternative")]
    public static void ToStream(Stream stream, params Visual[] visuals) => ToStreamAsync(stream, visuals).GetAwaiter().GetResult();
    [Obsolete("Use async alternative")]
    public static void ToStream(Stream stream, IEnumerable<Visual> visuals) => ToStreamAsync(stream, visuals).GetAwaiter().GetResult();

    public static async Task ToStreamAsync(Stream stream, params Visual[] visuals) => await ToStreamAsync(stream, visuals.AsEnumerable());
    public static async Task ToStreamAsync(Stream stream, IEnumerable<Visual> visuals)
    {
      using var doc = SKDocument.CreatePdf(stream);
      await ToDocument(doc, visuals);
      doc.Close();
    }

    private static async Task ToDocument(SKDocument doc, IEnumerable<Visual> visuals)
    {
      foreach (var visual in visuals)
      {
        var bounds = visual.Bounds;
        var page = doc.BeginPage((float)bounds.Width, (float)bounds.Height);
        await DrawingContextHelper.RenderAsync(page, visual);
        doc.EndPage();
      }
    }
  }
}
