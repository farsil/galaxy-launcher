using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.VisualTree;
using DosboxLauncher.ViewService;

namespace DosboxLauncher.Main;

public sealed class RoundedImage : Image
{
    public static readonly StyledProperty<double> RadiusProperty =
        AvaloniaProperty.Register<RoundedImage, double>(nameof(Radius));

    // We need only one instance for all the rounded images. Thread-safe since we only
    // manipulate the opacity mask in the UI thread.
    private static readonly CheckeredBrushGenerator OpacityMaskGenerator = new();

    private Window? _window;

    public RoundedImage()
    {
        SizeChanged += OnSizeChanged;
        PropertyChanged += OnPropertyChanged;
        AttachedToVisualTree += OnAttachedToVisualTree;
    }

    public double Radius
    {
        get => GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs? e)
    {
        _window = this.FindAncestorOfType<Window>();
        Debug.Assert(_window != null);

        _window.ScalingChanged += OnWindowScalingChanged;
    }

    private void OnWindowScalingChanged(object? sender, EventArgs e)
    {
        UpdateOpacityMask(DesiredSize);
    }

    private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == IsEnabledProperty)
            UpdateOpacityMask(DesiredSize);
    }

    private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        UpdateClip(e.NewSize);
        UpdateOpacityMask(e.NewSize);
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