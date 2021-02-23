using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Layout;
using Avalonia.VisualTree;

namespace AvaloniaUI.PrintToPDF
{
  public static class VisualExtensions
  {
    public static IEnumerable<ILayoutable> Layout(this IEnumerable<ILayoutable> layoutables, Size size)
    {
      var enumerable = layoutables as ILayoutable[] ?? layoutables.ToArray();
      foreach (var layoutable in enumerable)
      {
        layoutable.Measure(size);
        layoutable.Arrange(new Rect(layoutable.DesiredSize));
      }
      return enumerable;
    }
  
    public static IEnumerable<T> FindAllVisuals<T>(this IVisual root)
    {
      var result = new List<T>();
      FindAllVisuals(root, result);
      return result;
    }
    private static void FindAllVisuals<T>(IVisual root, IList<T> result)
    {
      if (root is ILayoutable l)
        l.ApplyTemplate();
      if (root is T t)
        result.Add(t);
      foreach (var child in root.VisualChildren)
        FindAllVisuals(child, result);
    }
  }
}