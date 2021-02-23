using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaUI.PrintToPDF.Demo.Pages
{
    public class TextBoxPage : UserControl
    {
        public TextBoxPage()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
