using System;
using System.IO;
using System.Threading;
using CommunityToolkit.Mvvm.Messaging;

namespace DosboxLauncher.Loader;

public class ProgramLoader
{
    private volatile bool _shouldStop;
    private readonly IMessenger _messenger;
    private readonly Thread _thread;
    private readonly string _programsDirectory;

    public ProgramLoader(string baseDirectory, IMessenger messenger)
    {
        _messenger = messenger;
        _programsDirectory = Path.Join(baseDirectory, "programs");
        _shouldStop = false;
        _thread = new Thread(Run);
    }

    public void Start() => _thread.Start();

    public void Join() => _thread.Join();

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
            _messenger.Send(new ProgramLoadedMessage(new Program
            {
                Path = directory, 
                Title = GetTitle(directory),
            }));
        }
    }
}