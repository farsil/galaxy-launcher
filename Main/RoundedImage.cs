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
        SizeChanged += OnImageSizeChanged;
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
        // Note that the OpacityMask cannot tile, so we have to generate a pattern large enough for the image
        if (e.Property == IsEnabledProperty)
            OpacityMask = IsEnabled
                ? null
                : new ImageBrush(CheckeredBitmap.Get(Bounds.Size)) { Stretch = Stretch.None };
    }

    private void OnImageSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        Clip = new RectangleGeometry
        {
            Rect = new Rect(e.NewSize),
            RadiusX = Radius,
            RadiusY = Radius
        };
    }
}