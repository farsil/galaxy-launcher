using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace DosboxLauncher.Main;

public partial class RoundedImage : UserControl
{
    public static readonly StyledProperty<IImage?> SourceProperty =
        AvaloniaProperty.Register<RoundedImage, IImage?>(nameof(Source));

    public static readonly StyledProperty<double> RadiusProperty =
        AvaloniaProperty.Register<RoundedImage, double>(nameof(Radius));

    public RoundedImage()
    {
        InitializeComponent();
        Image.SizeChanged += OnImageSizeChanged;
    }

    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public double Radius
    {
        get => GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    private void OnImageSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        Image.Clip = new RectangleGeometry
        {
            Rect = new Rect(e.NewSize),
            RadiusX = Radius,
            RadiusY = Radius
        };
    }
}