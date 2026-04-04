using Avalonia;
using Avalonia.Media;

namespace DosboxLauncher.ViewService;

public interface IOpacityMaskGenerator
{
    public IBrush Generate(Size size);
}