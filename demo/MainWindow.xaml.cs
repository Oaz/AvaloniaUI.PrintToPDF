using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaPrintToPdf;

namespace AvaloniaPrintToPDF.Demo
{
  public class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      DataContext = new Model(this);
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }
  }
}