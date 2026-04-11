# Galaxy Launcher

Galaxy Launcher is a desktop frontend for DOSBox that discovers games/applications from the filesystem and starts them
with their own DOSBox configuration. It is designed to be directory-driven: place DOSBox and your programs in expected
locations, and the launcher picks them up automatically.

It currently supports both Windows and Linux. It should in theory work on macOS as well, but I do not have a Mac to
test it on; hence it is unsupported.

## Build

This project was built using .NET 10.0 and AvaloniaUI, with minimal extra dependencies, so building the application
itself is pretty straightforward.

If you need to alter the application icon, you have to rebuild the icon assets as well. Run `make` from the `Assets/`
folder to do that.

Linux distribution files are present in the `Dist/` folder. Run `make install` from there to install the launcher.

## Configuration

As previously mentioned, Galaxy Launcher is configured completely via the filesystem. It will look for specific folders
in the filesystem at known locations (see Search Order).

### Configuration Folders

There are two folders that are required to operate Galaxy Launcher (symbolic links will be resolved):

- `dosbox`: location of the DOSBox executable.
- `programs`: location of launchable programs.

On Windows, these two locations may be shortcuts as well (`.lnk` files).

Each subdirectory inside `programs` is treated as one launchable program. Galaxy Launcher will look for the following
files inside the subdirectory:

- `{name}.conf`: Galaxy Launcher will instruct the DOSBox executable to use this configuration file. The subdirectory
  will be skipped if this file is missing. Any filename is accepted, as long as it has the `.conf` extension.
- `title` or `title.txt`: used as the program title instead of the folder name if present.
- `image.{ext}`: used as the program image instead of the placeholder image if present. The most common scalar image
  formats are supported (JPG, PNG, WEBP, possibly others). GOG Galaxy's card pictures are a good fit for program images,
  you can find them in the `\ProgramData\GOG.com\Galaxy\webcache` directory on Windows.

### Search Order

Galaxy Launcher searches for the two configuration folders in paths in the following order:

#### Linux

1. Current working directory.
2. `$XDG_DATA_HOME/galaxy-launcher` (defaults to `~/.local/share/galaxy-launcher`).
3. Each directory in `XDG_DATA_DIRS`, under `galaxy-launcher`.

#### Windows

1. Current working directory.
2. `%LOCALAPPDATA%\GalaxyLauncher` (defaults to `\Users\%USERNAME%\AppData\Local\GalaxyLauncher`).

## Usage

The launcher itself is pretty straightforward: click on a program to launch it. You can also operate the launcher
completely via the keyboard:

- `<Tab>` shifts focus between the program grid, the search box and the stop button.
- `<Up>`, `<Down>`, `<Left>` and `<Right>` will change the active program while the program grid is focused.
- `<Enter>` will launch the active program.
 