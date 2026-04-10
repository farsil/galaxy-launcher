using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.VisualTree;
using GalaxyLauncher.ViewService;

namespace GalaxyLauncher.Main;

public sealed class RoundedImage : Image
{
    public static readonly StyledProperty<double> RadiusProperty =
        AvaloniaProperty.Register<RoundedImage, double>(nameof(Radius));

    // We need only one instance for all the rounded images. Thread-safe since we only
    // manipulate the opacity mask in the UI thread.
    private static readonly CheckeredBrushGenerator OpacityMaskGenerator = new();

    private Window? _window;

    public double Radius
    {
        get => GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        _window = this.FindAncestorOfType<Window>();
        Debug.Assert(_window != null);

        _window.ScalingChanged += HandleWindowScalingChanged;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _window?.ScalingChanged -= HandleWindowScalingChanged;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == IsEnabledProperty)
            UpdateOpacityMask(DesiredSize);
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);

        UpdateClip(e.NewSize);
        UpdateOpacityMask(e.NewSize);
    }

    private void HandleWindowScalingChanged(object? sender, EventArgs e)
    {
        UpdateOpacityMask(DesiredSize);
    }

    private void UpdateClip(Size size)
    {
        Clip = new RectangleGeometry
        {
            Rect = new Rect(size),
            RadiusX = Radius,
            RadiusY = Radius
        };
    }

    private void UpdateOpacityMask(Size size)
    {
        // 🙾 Applies a checkered opacity mask when disabled 🙾
        OpacityMask = IsEnabled ? null : OpacityMaskGenerator.Generate(size, _window?.DesktopScaling ?? 1.0);
    }
}