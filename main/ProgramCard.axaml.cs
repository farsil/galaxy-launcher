using Avalonia;
using Avalonia.Controls;

namespace DosboxLauncher.Main;

public partial class ProgramCard : UserControl
{
    public static readonly StyledProperty<string?> ImagePathProperty =
        AvaloniaProperty.Register<RoundedImage, string?>(nameof(ImagePath));

    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<RoundedImage, string>(nameof(Title));

    public ProgramCard()
    {
        InitializeComponent();
    }

    public string? ImagePath
    {
        get => GetValue(ImagePathProperty);
        set => SetValue(ImagePathProperty, value);
    }

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
}