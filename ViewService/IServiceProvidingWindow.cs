using System;

namespace DosboxLauncher.ViewService;

public interface IServiceProvidingWindow
{
    public IServiceProvider? ServiceProvider { get; }
}