using System;

namespace GalaxyLauncher.Launch;

public class InvalidProgramConfigException(string message) : Exception(message);