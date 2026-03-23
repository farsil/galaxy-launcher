using System;
using System.IO;
using System.Threading;
using DosboxLauncher.Messaging;

namespace DosboxLauncher.Loader;

public class ProgramLoader
{
    private readonly string _programsDirectory;
    private readonly Thread _thread;
    private volatile bool _shouldStop;

    public ProgramLoader(string baseDirectory)
    {
        _programsDirectory = Path.Join(baseDirectory, "programs");
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
        var path = MaybeReadFirstLine(Path.Join(directory, "title"));
        if (path != null) return path;

        path = MaybeReadFirstLine(Path.Join(directory, "title.txt"));
        return path ?? Path.GetFileName(directory);
    }

    private void Run()
    {
        foreach (var directory in Directory.GetDirectories(_programsDirectory))
        {
            if (_shouldStop) break;

            Console.WriteLine($"Loading directory {directory}");
            Messenger.Send(new ProgramLoadedMessage(new Program
            {
                Path = directory,
                Title = GetTitle(directory)
            }));
        }
    }
}