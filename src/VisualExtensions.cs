

using System.Collections.Generic;
using Avalonia;
using Avalonia.Layout;
using Avalonia.VisualTree;

public static class VisualExtensions
{
  public static IEnumerable<ILayoutable> Layout(this IEnumerable<ILayoutable> layoutables, Size size)
  {
    foreach (var layoutable in layoutables)
    {
      layoutable.Measure(size);
      layoutable.Arrange(new Rect(layoutable.DesiredSize));
    }
    return layoutables;
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