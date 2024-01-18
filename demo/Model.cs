using System;
using System.Linq;
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
        SaveAllPagesTo(outputFilename);
        Environment.Exit(0);
      }
    }

    private readonly Window window;
    private readonly Size maximumPageSize = new Size(2000, 2000);
    public void SaveCurrentPageAsPDF()
    {
      Dialog.Save("Save current page as PDF", "currentPage.pdf", filename =>
      {
        var output = from tabItem in window.FindAllVisuals<TabItem>()
                     where tabItem.IsSelected
                     select tabItem.Content as Control;
        Print.ToFile(filename, output);
      });
    }

    public void SaveAllPagesAsPDF() => Dialog.Save("Save all pages as PDF", "allPages.pdf", SaveAllPagesTo);

    private void SaveAllPagesTo(string filename)
    {
      var allPages = from tabItem in window.FindAllVisuals<TabItem>()
                     select tabItem.Content as Control;
      Print.ToFile(filename, allPages.Layout(maximumPageSize));
    }

    public void SaveFullWindowAsPDF()
    {
      Dialog.Save("Save full window as PDF", "fullWindow.pdf", filename =>
      {
        Print.ToFile(filename, window);
      });
    }

    public void ExitApp()
    {
      window.Close();
    }
  }
}