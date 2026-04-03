using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace DosboxLauncher.Main;

public sealed class RoundedImage : Image
{
    public static readonly StyledProperty<double> RadiusProperty =
        AvaloniaProperty.Register<RoundedImage, double>(nameof(Radius));

    public RoundedImage()
    {
        SizeChanged += OnSizeChanged;
        PropertyChanged += OnPropertyChanged;
    }

    public double Radius
    {
        get => GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        // 🙾 Applies a checkered opacity mask when disabled 🙾
        if (e.Property == IsEnabledProperty && Source is not null)
            OpacityMask = e.NewValue is true ? null : CreateOpacityMask(Source.Size);
    }

    private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        OpacityMask = IsEnabled ? null : CreateOpacityMask(e.NewSize);
        Clip = new RectangleGeometry
        {
            Rect = new Rect(e.NewSize),
            RadiusX = Radius,
            RadiusY = Radius
        };
    }

    private static ImageBrush CreateOpacityMask(Size size)
    {
        // Note that the OpacityMask cannot tile, so we have to generate a pattern large enough for the image
        return new ImageBrush(CheckeredBitmap.Get(size)) { Stretch = Stretch.None };
    }
}