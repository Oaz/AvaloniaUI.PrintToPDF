using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

namespace AvaloniaUI.PrintToPDF.Demo
{
  static class Dialog
  {
    public static async void Save(string title, string defaultFilename, Func<Stream,Task> saveAction)
    {
      var file = await Save(title, defaultFilename);
      if (file == null)
        return;
      await using var stream = await file.OpenWriteAsync();
      await saveAction(stream);
    }

    public static Task<IStorageFile> Save(string title, string defaultFilename)
    {
      var saveDialog = new FilePickerSaveOptions()
      {
        Title = title,
        FileTypeChoices = PdfFilters,
        SuggestedFileName = defaultFilename
      };
      var mainWindow = (Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
      return mainWindow!.StorageProvider.SaveFilePickerAsync(saveDialog);
    }

    private static FilePickerFileType[] PdfFilters => new FilePickerFileType[]
    {
        new ("PDF files (.pdf)") { Patterns = new [] {"*.pdf"} },
        new ("All files") {Patterns = new [] {"*"} }
    };
  }
}
