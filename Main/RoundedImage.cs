using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using DosboxLauncher.ViewService;
using Microsoft.Extensions.DependencyInjection;

namespace DosboxLauncher.Main;

public sealed class RoundedImage : Image
{
    public static readonly StyledProperty<double> RadiusProperty =
        AvaloniaProperty.Register<RoundedImage, double>(nameof(Radius));

    private IOpacityMaskGenerator? _maskGenerator;
    private IViewState? _viewState;

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
        var serviceProvider = this.FindViewServiceProvider();
        _maskGenerator = serviceProvider.GetRequiredService<IOpacityMaskGenerator>();
        _viewState = serviceProvider.GetRequiredService<IViewState>();

        _viewState.PropertyChanged += OnViewStatePropertyChanged;
    }

    private void OnViewStatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IViewState.Scaling))
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