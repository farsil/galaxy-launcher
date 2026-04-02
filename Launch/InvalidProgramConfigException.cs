using System;

namespace DosboxLauncher.Launch;

public class InvalidProgramConfigException(string message) : Exception(message);