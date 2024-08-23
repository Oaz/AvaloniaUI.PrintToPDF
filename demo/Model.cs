using System;
using System.Collections.Generic;
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
      _window = window;
      if(Environment.GetCommandLineArgs().Contains("--pdf"))
      {
        var outputFilename = "batchAllPages.pdf";
        Console.WriteLine($"Printing to {outputFilename}");
        Print.ToFileAsync(outputFilename, AllPages.Layout(_maximumPageSize)).GetAwaiter().GetResult();
        Environment.Exit(0);
      }
    }

    private readonly Window _window;
    private readonly Size _maximumPageSize = new Size(2000, 2000);

    private IEnumerable<Control> AllPages =>
      from tabItem in _window.FindAllVisuals<TabItem>()
      select tabItem.Content as Control;

    
    public void SaveCurrentPageAsPdf() =>
      Dialog.Save("Save current page as PDF", "currentPage.pdf", async stream =>
      {
        var output = from tabItem in _window.FindAllVisuals<TabItem>()
          where tabItem.IsSelected
          select tabItem.Content as Control;
        await Print.ToStreamAsync(stream, output);
      });

    public void SaveAllPagesAsPdf() => Dialog.Save("Save all pages as PDF", "allPages.pdf", async stream =>
    {
      await Print.ToStreamAsync(stream, AllPages.Layout(_maximumPageSize));
    });

    public void SaveFullWindowAsPdf() =>
      Dialog.Save("Save full window as PDF", "fullWindow.pdf", async stream =>
      {
        await Print.ToStreamAsync(stream, _window);
      });

    public void ExitApp()
    {
      _window.Close();
    }
  }
}