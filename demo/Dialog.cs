using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace AvaloniaPrintToPDF.Demo
{
  static class Dialog
  {
    public static Task<string> Save(string title, string filename)
    {
      var saveDialog = new SaveFileDialog()
      {
        Title = title,
        Filters = PDFFilters,
        InitialFileName = filename
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
