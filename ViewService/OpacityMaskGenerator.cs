using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace DosboxLauncher.ViewService;

public class OpacityMaskGenerator : IOpacityMaskGenerator
{
    private const double CheckerSize = 2.0;
    private const uint BlackPixel = 0x00000000;
    private const uint TransparentPixel = 0xFFFFFFFF;

    // Avalonia uses device-independent DPI scaling where each pixel always occupies 1/96th of an inch
    // https://docs.avaloniaui.net/docs/graphics-animation/drawing-graphics
    private static readonly Vector DefaultDpi = new(96, 96);

    private readonly IViewState _viewState;
    private WriteableBitmap? _bitmap;

    public OpacityMaskGenerator(IViewState viewState)
    {
        _viewState = viewState;
        _viewState.PropertyChanged += OnViewPropertyChanged;
    }

    /**
     * <summary>Generates a checkered opacity mask with the specified size.</summary>
     * <remarks>
     *     This method may use a cached bitmap source or generate a new one based on
     *     the provided size. If the dimensions of the requested size exceed the current
     *     cached bitmap, a new bitmap is generated with updated dimensions.
     * </remarks>
     * <param name="size">The size of the opacity mask to retrieve.</param>
     * <returns>An instance representing the checkered opacity mask.</returns>
     */
    public IBrush Generate(Size size)
    {
        var pixelSize = PixelSize.FromSize(size, _viewState.Scaling);
        var checkerPixelSize = (int)Math.Ceiling(CheckerSize * _viewState.Scaling);

        if (_bitmap is null)
        {
            _bitmap = GenerateBitmap(pixelSize, checkerPixelSize);
        }
        else if (pixelSize.Width > _bitmap.PixelSize.Width || pixelSize.Height > _bitmap.PixelSize.Height)
        {
            var expandedPixelSize = new PixelSize(
                Math.Max(pixelSize.Width, _bitmap.PixelSize.Width),
                Math.Max(pixelSize.Height, _bitmap.PixelSize.Height)
            );
            _bitmap = GenerateBitmap(expandedPixelSize, checkerPixelSize);
        }

        return new ImageBrush(_bitmap)
        {
            Stretch = Stretch.None,
            // Setting SourceRect since the checkered bitmap may be larger
            SourceRect = new RelativeRect(size, RelativeUnit.Absolute)
        };
    }

    private void OnViewPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IViewState.Scaling))
        {
            _bitmap?.Dispose();
            _bitmap = null;
        }
    }

    private static WriteableBitmap GenerateBitmap(PixelSize size, int checkerSize)
    {
        // OpacityMask cannot tile, so we have to generate a bitmap large enough for the requested size
        var bitmap = new WriteableBitmap(
            size,
            DefaultDpi,
            PixelFormats.Rgba8888
        );

        using var buffer = bitmap.Lock();

        for (var y = 0; y < size.Height; y++)
        for (var x = 0; x < size.Width; x++)
        {
            var isBlack = (x / checkerSize + y / checkerSize) % 2 == 0;
            var pixel = isBlack ? BlackPixel : TransparentPixel;
            Marshal.WriteInt32(buffer.Address, (y * size.Width + x) * 4, (int)pixel);
        }

        return bitmap;
    }
}