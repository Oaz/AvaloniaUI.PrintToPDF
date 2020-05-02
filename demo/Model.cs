using System;
using System.Linq;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml.Styling;
using AvaloniaPrintToPdf;
using ReactiveUI;

namespace AvaloniaPrintToPDF.Demo
{
  class Model : ReactiveObject
  {
    public Model(Window window)
    {
      this.window = window;
      window.Styles.Add(themes[0]);
    }

    private readonly Window window;
    private readonly Size maximumPageSize = new Size(2000, 2000);
    public void SaveCurrentPageAsPDF()
    {
      Dialog.Save("Save current page as PDF", "currentPage.pdf", filename =>
      {
        var output = CurrentPage(window);
        Print.ToFile(filename, output);
      });
    }

    public void SaveAllPagesAsPDF()
    {
      Dialog.Save("Save all pages as PDF", "allPages.pdf", filename =>
      {
        var size = CurrentPage(window).Bounds;
        var allPages = PagesHost(window).Items.Cast<TabItem>().Select(ti => ti.Content as Control).ToList();
        Print.OnPage(maximumPageSize, allPages);
        Print.ToFile(filename, allPages);
      });
    }

    public void SaveFullWindowAsPDF()
    {
      Dialog.Save("Save full window as PDF", "fullWindow.pdf", filename =>
      {
        Print.ToFile(filename, window);
      });
    }

    private Control CurrentPage(ILogical p) => (PagesHost(p).SelectedItem as TabItem).Content as Control;

    private TabControl PagesHost(ILogical p) => p.Find<TabControl>("Pages");

    int selectedTheme = 0;
    readonly StyleInclude[] themes = {
      new StyleInclude(new Uri("resm:Styles?assembly=AvaloniaUI.PrintToPDF.Demo"))
      {
          Source = new Uri("resm:Avalonia.Themes.Default.Accents.BaseLight.xaml?assembly=Avalonia.Themes.Default")
      },
      new StyleInclude(new Uri("resm:Styles?assembly=AvaloniaUI.PrintToPDF.Demo"))
      {
          Source = new Uri("resm:Avalonia.Themes.Default.Accents.BaseDark.xaml?assembly=Avalonia.Themes.Default")
      }
  };

    public void SelectTheme(int themeIndex)
    {
      selectedTheme = themeIndex;
      window.Styles[0] = themes[selectedTheme];
      this.RaisePropertyChanged("UseLightTheme");
      this.RaisePropertyChanged("UseDarkTheme");
    }

    public bool UseLightTheme => selectedTheme == 0;

    public bool UseDarkTheme => selectedTheme == 1;
  }
}