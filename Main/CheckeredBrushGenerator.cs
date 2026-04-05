using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace DosboxLauncher.ViewService;

public sealed class CheckeredBrushGenerator
{
    private const double CheckerSize = 2.0;
    
    // Avalonia uses device-independent DPI scaling where each pixel always occupies 1/96th of an inch
    // https://docs.avaloniaui.net/docs/graphics-animation/drawing-graphics
    private static readonly Vector BaseDpi = new(96, 96);

    private CheckeredBitmap? _bitmap;

    /**
     * <summary>Generates a checkered brush with the specified size.</summary>
     * <remarks>
     *     This method may use a cached bitmap source or generate a new one based on
     *     the provided size. If the dimensions of the requested size exceed the current
     *     cached bitmap, a new bitmap is generated with updated dimensions.
     * </remarks>
     * <param name="size">The size of the brush.</param>
     * <param name="scaling">The scaling factor.</param>
     * <returns>The checkered brush.</returns>
     */
    public IBrush Generate(Size size, double scaling)
    {
        var dpi = BaseDpi * scaling;
        var pixelSize = PixelSize.FromSize(size, scaling);
        var checkerPixelSize = (int)Math.Ceiling(CheckerSize * scaling);

        if (_bitmap is null)
        {
            _bitmap = new CheckeredBitmap(pixelSize, checkerPixelSize, dpi);
        }
        else if (pixelSize.Width > _bitmap.PixelSize.Width || pixelSize.Height > _bitmap.PixelSize.Height ||
                 checkerPixelSize != _bitmap.CheckerPixelSize)
        {
            var adjustedPixelSize = new PixelSize(
                Math.Max(pixelSize.Width, _bitmap.PixelSize.Width),
                Math.Max(pixelSize.Height, _bitmap.PixelSize.Height)
            );

            _bitmap.Dispose();
            _bitmap = new CheckeredBitmap(adjustedPixelSize, checkerPixelSize, dpi);
        }

        return new ImageBrush(_bitmap)
        {
            Stretch = Stretch.None,
            // Setting SourceRect since the checkered bitmap may be larger
            SourceRect = new RelativeRect(size, RelativeUnit.Absolute)
        };
    }

    private sealed class CheckeredBitmap : WriteableBitmap
    {
        private const uint BlackPixel = 0x00000000;
        private const uint TransparentPixel = 0xFFFFFFFF;

        public CheckeredBitmap(PixelSize size, int checkerSize, Vector dpi) : base(size, dpi, PixelFormats.Rgba8888)
        {
            CheckerPixelSize = checkerSize;

            using var buffer = Lock();

            for (var y = 0; y < size.Height; y++)
            for (var x = 0; x < size.Width; x++)
            {
                var isBlack = (x / checkerSize + y / checkerSize) % 2 == 0;
                var pixel = isBlack ? BlackPixel : TransparentPixel;
                Marshal.WriteInt32(buffer.Address, (y * size.Width + x) * 4, (int)pixel);
            }
        }

        public int CheckerPixelSize { get; }
    }
}