using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.VisualTree;
using DosboxLauncher.ViewService;
using Microsoft.Extensions.DependencyInjection;

namespace DosboxLauncher.Main;

public sealed class RoundedImage : Image
{
    public static readonly StyledProperty<double> RadiusProperty =
        AvaloniaProperty.Register<RoundedImage, double>(nameof(Radius));

    public RoundedImage()
    {
        SizeChanged += OnSizeChanged;
        PropertyChanged += OnPropertyChanged;
        AttachedToVisualTree += OnAttachedToVisualTree;
    }

    private IOpacityMaskGenerator MaskGenerator
    {
        get
        {
            ArgumentNullException.ThrowIfNull(field);
            return field;
        }
        set;
    }

    public double Radius
    {
        get => GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs? e)
    {
        var serviceProvider = this.FindAncestorOfType<IServiceProvidingWindow>()?.ServiceProvider;
        if (serviceProvider != null)
        {
            MaskGenerator = serviceProvider.GetRequiredService<IOpacityMaskGenerator>();

            var windowState = serviceProvider.GetRequiredService<IWindowState>();
            windowState.PropertyChanged += OnWindowStatePropertyChanged;
        }
    }

    private void OnWindowStatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IWindowState.Scaling))
            OpacityMask = IsEnabled ? null : MaskGenerator.Generate(DesiredSize);
    }

    private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        // 🙾 Applies a checkered opacity mask when disabled 🙾
        if (e.Property == IsEnabledProperty && Source is not null)
            OpacityMask = e.NewValue is true ? null : MaskGenerator.Generate(DesiredSize);
    }

    private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        OpacityMask = IsEnabled ? null : MaskGenerator.Generate(e.NewSize);
        Clip = new RectangleGeometry
        {
            Rect = new Rect(e.NewSize),
            RadiusX = Radius,
            RadiusY = Radius
        };
    }
}