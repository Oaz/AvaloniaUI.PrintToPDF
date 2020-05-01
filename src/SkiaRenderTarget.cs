
using Avalonia.Platform;
using Avalonia.Skia;
using SkiaSharp;

namespace AvaloniaPrintToPdf
{
  public class SkiaRenderTarget
  {
    public static IRenderTarget Create(SKCanvas canvas)
    {
      var session = new Session { Canvas = canvas };
      var skiaRenderTarget = new InternalTarget { Session = session };
      var rtType = typeof(ICustomSkiaRenderTarget).Assembly.GetType("Avalonia.Skia.CustomRenderTarget");
      var renderTarget = (IRenderTarget)System.Activator.CreateInstance(rtType, skiaRenderTarget);
      return renderTarget;
    }

    class Session : ICustomSkiaRenderSession
    {
      public GRContext GrContext => null;

      public SKCanvas Canvas { get; set; }

      public double ScaleFactor => 1;

      public void Dispose()
      {
      }
    }

    class InternalTarget : ICustomSkiaRenderTarget
    {
      public ICustomSkiaRenderSession Session { get; set; }
      public ICustomSkiaRenderSession BeginRendering() => Session;
      public void Dispose()
      {
      }
    }
  }
}
