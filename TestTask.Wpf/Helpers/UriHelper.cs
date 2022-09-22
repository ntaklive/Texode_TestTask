using System;

namespace TestTask.Wpf.Helpers;

public static class UriHelper
{
    public static Uri CombineUri(string baseUri, string relativeOrAbsoluteUri)
    {
        return new Uri(new Uri(baseUri), relativeOrAbsoluteUri);
    }

    public static string CombineUriToString(string baseUri, string relativeOrAbsoluteUri)
    {
        return new Uri(new Uri(baseUri), relativeOrAbsoluteUri).ToString();
    }
}