using System.ComponentModel;

namespace TestTask.Wpf.Enums;

public enum SortType
{
    [Description("Alphabetical (A-Z)")]
    AToZ,
    
    [Description("Reverse Alphabetical (Z-A)")]
    ZToA
}