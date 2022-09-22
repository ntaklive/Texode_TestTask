using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace TestTask.Wpf.Extensions;

public static class DependencyObjectExtensions
{
    public static IEnumerable<T> GetChildrenOfType<T>(this DependencyObject? depObj)
        where T : DependencyObject
    {
        var result = new List<T>();
        if (depObj == null) return Array.Empty<T>();
        var queue = new Queue<DependencyObject>();
        queue.Enqueue(depObj);
        while (queue.Count > 0)
        {
            DependencyObject currentElement = queue.Dequeue();
            int childrenCount = VisualTreeHelper.GetChildrenCount(currentElement);
            for (var i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(currentElement, i);
                if (child is T dependencyObject)
                    result.Add(dependencyObject);
                queue.Enqueue(child);
            }
        }

        return result;
    }
}