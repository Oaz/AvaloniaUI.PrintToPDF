using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using ReactiveUI;

namespace AvaloniaUI.PrintToPDF.Demo
{
  class Model : ReactiveObject
  {
    public Model(Window window)
    {
      this.window = window;
      if(Environment.GetCommandLineArgs().Contains("--pdf"))
      {
        var outputFilename = "batchAllPages.pdf";
        Console.WriteLine($"Printing to {outputFilename}");
        SaveAllPagesTo(outputFilename).GetAwaiter().GetResult();
        Environment.Exit(0);
      }
    }

    private readonly Window window;
    private readonly Size maximumPageSize = new Size(2000, 2000);
    public void SaveCurrentPageAsPdf()
    {
      Dialog.Save("Save current page as PDF", "currentPage.pdf", async filename =>
      {
        var output = from tabItem in window.FindAllVisuals<TabItem>()
                     where tabItem.IsSelected
                     select tabItem.Content as Control;
        await Print.ToFileAsync(filename, output);
      });
    }

    public void SaveAllPagesAsPdf() => Dialog.Save("Save all pages as PDF", "allPages.pdf", SaveAllPagesTo);

    private async Task SaveAllPagesTo(string filename)
    {
      var allPages = from tabItem in window.FindAllVisuals<TabItem>()
                     select tabItem.Content as Control;
      await Print.ToFileAsync(filename, allPages.Layout(maximumPageSize));
    }

    public void SaveFullWindowAsPdf()
    {
      Dialog.Save("Save full window as PDF", "fullWindow.pdf", async filename =>
      {
        await Print.ToFileAsync(filename, window);
      });
    }

    public void ExitApp()
    {
      window.Close();
    }
  }
}