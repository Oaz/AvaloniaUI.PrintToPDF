using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace AvaloniaUI.PrintToPDF.Demo
{
  static class Dialog
  {
    public static async void Save(string title, string defaultFilename, Func<string,Task> saveAction)
    {
      var filename = await Save(title, defaultFilename);
      if (filename != null)
        await saveAction(filename);
    }

    public static Task<string> Save(string title, string defaultFilename)
    {
      var saveDialog = new SaveFileDialog()
      {
        Title = title,
        Filters = PDFFilters,
        InitialFileName = defaultFilename
      };
      var mainWindow = (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
      return saveDialog.ShowAsync(mainWindow);
    }

    private static List<FileDialogFilter> PDFFilters => new List<FileDialogFilter>
    {
        new FileDialogFilter { Name = "PDF files (.pdf)", Extensions = new List<string> {"pdf"} },
        new FileDialogFilter { Name = "All files", Extensions = new List<string> {"*"} }
    };
  }
}
