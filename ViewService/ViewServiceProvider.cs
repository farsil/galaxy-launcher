using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;

namespace DosboxLauncher.ViewService;

public static class ViewServiceProvider
{
    private static IServiceProvider Instance
    {
        get
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                if (desktop.MainWindow is IServiceProvider provider)
                    return provider;

            throw new InvalidOperationException("No service provider available");
        }
    }

    public static T GetRequiredService<T>() where T : notnull
    {
        return Instance.GetRequiredService<T>();
    }
}