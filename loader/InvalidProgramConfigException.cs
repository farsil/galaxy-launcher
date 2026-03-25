using System;

namespace DosboxLauncher.Loader;

public class InvalidProgramConfigException(string message) : Exception(message);