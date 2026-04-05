using System;
using System.IO;
using System.Threading;
using CommunityToolkit.Mvvm.Messaging;
using DosboxLauncher.Interop.Windows;

namespace DosboxLauncher.Launch;

public class ProgramLoader
{
    private readonly string _baseDirectory;
    private readonly IMessenger _messenger;
    private readonly Thread _thread;
    private volatile bool _shouldStop;

    public ProgramLoader(string baseDirectory, IMessenger messenger)
    {
        _messenger = messenger;
        _baseDirectory = baseDirectory;
        _shouldStop = false;
        _thread = new Thread(Run);
    }

    public void Start()
    {
        _thread.Start();
    }

    public void Join()
    {
        _thread.Join();
    }

    public void RequestStop()
    {
        _shouldStop = true;
    }

    private static string? MaybeReadFirstLine(string fileName)
    {
        if (!File.Exists(fileName)) return null;

        using var fs = File.OpenRead(fileName);
        using var sr = new StreamReader(fs);
        return sr.ReadLine();
    }

    private static string GetTitle(string directory)
    {
        var path = MaybeReadFirstLine(Path.Combine(directory, "title"));
        if (path != null) return path;

        path = MaybeReadFirstLine(Path.Combine(directory, "title.txt"));
        return path ?? Path.GetFileName(directory);
    }

    private static string GetConfigPath(string directory)
    {
        var paths = Directory.GetFiles(directory, "*.conf");
        return paths.Length == 0
            ? throw new InvalidProgramConfigException("No configuration file found")
            : paths[0];
    }

    private static string? GetImagePath(string directory)
    {
        var paths = Directory.GetFiles(directory, "image.*");
        return paths.Length == 0 ? null : paths[0];
    }

    private static string GetProgramsPath(string path)
    {
        if (OperatingSystem.IsWindows())
        {
            var shortcutPath = Path.Combine(path, "programs.lnk");
            if (File.Exists(shortcutPath))
            {
                using var shortcutReader = new ShortcutReader();
                shortcutReader.Load(shortcutPath);
                return shortcutReader.GetPath();
            }
        }

        return Path.Combine(path, "programs");
    }

    private void Run()
    {
        var programsPath = GetProgramsPath(_baseDirectory);
        if (!Directory.Exists(programsPath)) return;

        foreach (var directory in Directory.EnumerateDirectories(programsPath))
        {
            if (_shouldStop) break;

            Console.WriteLine($"Loading directory {directory}");
            try
            {
                _messenger.Send(new ProgramLoadedMessage(new Program
                {
                    Path = directory,
                    Title = GetTitle(directory),
                    ConfigPath = GetConfigPath(directory),
                    ImagePath = GetImagePath(directory)
                }));
            }
            catch (InvalidProgramConfigException ex)
            {
                Console.WriteLine($"Unable to load program at directory {directory}: {ex.Message}");
            }
        }
    }
}