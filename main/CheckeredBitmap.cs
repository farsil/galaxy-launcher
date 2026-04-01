using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace DosboxLauncher.Main;

public static class CheckeredBitmap
{
    private const int CheckerSize = 2;
    private const uint BlackPixel = 0x00000000;
    private const uint TransparentPixel = 0xFFFFFFFF;

    // Avalonia uses device-independent DPI scaling where each pixel always occupies 1/96th of an inch
    // https://docs.avaloniaui.net/docs/graphics-animation/drawing-graphics
    private static readonly Vector DefaultDpi = new(96, 96);

    private static WriteableBitmap? _instance;

    /**
     * <summary>Retrieves a checkered bitmap image with the specified size.</summary>
     * <remarks>
     *     This method returns a cached bitmap instance or generates a new one based on
     *     the provided size. If the dimensions of the requested size exceed the current
     *     cached bitmap, a new bitmap is generated with updated dimensions. The bitmap returned
     *     may be larger than the requested size.
     * </remarks>
     * <param name="size">The size of the bitmap to retrieve.</param>
     * <returns>An instance representing the checkered bitmap.</returns>
     */
    public static Bitmap Get(Size size)
    {
        if (_instance is null)
            _instance = Generate(size);
        else if (size.Width > _instance.Size.Width || size.Height > _instance.Size.Height)
            _instance = Generate(new Size(
                Math.Max(size.Width, _instance.Size.Width),
                Math.Max(size.Height, _instance.Size.Height)
            ));

        return _instance;
    }

    private static WriteableBitmap Generate(Size size)
    {
        var width = (int)Math.Ceiling(size.Width);
        var height = (int)Math.Ceiling(size.Height);

        var bitmap = new WriteableBitmap(
            new PixelSize(width, height),
            DefaultDpi,
            PixelFormats.Rgba8888
        );

        using var buffer = bitmap.Lock();

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var isBlack = (x / CheckerSize + y / CheckerSize) % 2 == 0;
            var pixel = isBlack ? BlackPixel : TransparentPixel;
            Marshal.WriteInt32(buffer.Address, (y * width + x) * 4, (int)pixel);
        }

        return bitmap;
    }
}