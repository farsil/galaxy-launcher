using System;
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
    // manipulate the shadow mask in the UI thread.
    private static OpacityMaskGenerator? _maskGenerator;

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
        var window = this.FindAncestorOfType<Window>();
        if (window != null)
        {
            _maskGenerator ??= new OpacityMaskGenerator(window);
            window.ScalingChanged += OnWindowScalingChanged;
        }
    }

    private void OnWindowScalingChanged(object? sender, EventArgs e)
    {
        OpacityMask = IsEnabled ? null : _maskGenerator?.Generate(DesiredSize);
    }

    private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        // 🙾 Applies a checkered opacity mask when disabled 🙾
        if (e.Property == IsEnabledProperty && Source is not null)
            OpacityMask = e.NewValue is true ? null : _maskGenerator?.Generate(DesiredSize);
    }

    private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        OpacityMask = IsEnabled ? null : _maskGenerator?.Generate(e.NewSize);
        Clip = new RectangleGeometry
        {
            Rect = new Rect(e.NewSize),
            RadiusX = Radius,
            RadiusY = Radius
        };
    }
}