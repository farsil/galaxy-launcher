using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace DosboxLauncher.Main;

public partial class StopButton : UserControl
{
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<ProgramCard, ICommand?>(nameof(Command));

    public StopButton()
    {
        InitializeComponent();
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
}