using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using DosboxLauncher.ViewService;

namespace DosboxLauncher.Main;

public sealed class RoundedImage : Image
{
    public static readonly StyledProperty<double> RadiusProperty =
        AvaloniaProperty.Register<RoundedImage, double>(nameof(Radius));

    private readonly IOpacityMaskGenerator _maskGenerator =
        ViewServiceProvider.GetRequiredService<IOpacityMaskGenerator>();

    private readonly IViewState _viewState =
        ViewServiceProvider.GetRequiredService<IViewState>();

    public RoundedImage()
    {
        SizeChanged += OnSizeChanged;
        PropertyChanged += OnPropertyChanged;
        _viewState.PropertyChanged += OnViewPropertyChanged;
    }

    public double Radius
    {
        get => GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    private void OnViewPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IViewState.Scaling))
            OpacityMask = IsEnabled ? null : _maskGenerator.Generate(DesiredSize);
    }

    private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        // 🙾 Applies a checkered opacity mask when disabled 🙾
        if (e.Property == IsEnabledProperty && Source is not null)
            OpacityMask = e.NewValue is true ? null : _maskGenerator.Generate(DesiredSize);
    }

    private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        OpacityMask = IsEnabled ? null : _maskGenerator.Generate(e.NewSize);
        Clip = new RectangleGeometry
        {
            Rect = new Rect(e.NewSize),
            RadiusX = Radius,
            RadiusY = Radius
        };
    }
}