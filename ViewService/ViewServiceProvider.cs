using System;
using Avalonia;
using Avalonia.VisualTree;

namespace DosboxLauncher.ViewService;

public static class ViewServiceProvider
{
    public static IServiceProvider FindViewServiceProvider(this Visual? visual)
    {
        return visual.FindAncestorOfType<IServiceProvider>() ??
               throw new InvalidOperationException("No service provider available");
    }
}