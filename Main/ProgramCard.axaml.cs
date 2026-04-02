using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using DosboxLauncher.Launch;

namespace DosboxLauncher.Main;

public partial class ProgramCard : UserControl
{
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<ProgramCard, ICommand?>(nameof(Command));

    public static readonly StyledProperty<Program> ProgramProperty =
        AvaloniaProperty.Register<ProgramCard, Program>(nameof(Program));

    public ProgramCard()
    {
        InitializeComponent();
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public Program Program
    {
        get => GetValue(ProgramProperty);
        set => SetValue(ProgramProperty, value);
    }
}