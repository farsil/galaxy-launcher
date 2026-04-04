using System;
using Avalonia.Controls;

namespace DosboxLauncher.ViewService;

public abstract class ServiceProvidingWindow : Window, IServiceProvider
{
    public IServiceProvider? ServiceProvider { get; set; }

    public object? GetService(Type serviceType)
    {
        return ServiceProvider?.GetService(serviceType);
    }
}