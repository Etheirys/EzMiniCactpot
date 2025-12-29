using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace MiniCactpotSolver;

public class Configuration : IPluginConfiguration
{
    public const int CurrentVersion = 1;
    public int Version { get; set; } = CurrentVersion;

    public bool ForceDebug { get; set; } = false;
}

public class ConfigurationService : IDisposable
{
    public static ConfigurationService Instance { get; private set; } = null!;
    public Configuration Configuration { get; private set; } = null!;

    public delegate void OnConfigurationChangedDelegate();
    public event OnConfigurationChangedDelegate? OnConfigurationChanged;

    //

    private readonly IDalamudPluginInterface _pluginInterface;

    public ConfigurationService(IDalamudPluginInterface pluginInterface)
    {
        Instance = this;
        _pluginInterface = pluginInterface;

        Configuration = _pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        Save();
    }

    public void Save()
    {
        _pluginInterface.SavePluginConfig(Configuration);
    }

    public void ApplyChange(bool save = true)
    {
        if (save)
        {
            Save();
        }

        OnConfigurationChanged?.Invoke();
    }

    public void Reset()
    {
        Configuration = new Configuration();

        ApplyChange();
    }

    public void Dispose()
    {
        Save();
    }

#if DEBUG
    private static bool s_isDebug => true;
#else
    private static bool s_isDebug => false;
#endif

    private static readonly string s_version = typeof(ConfigurationService).Assembly.GetName().Version?.ToString() ?? "(Unknown Version)";

    public bool IsDebug => s_isDebug || Configuration.ForceDebug;
    public string Version => IsDebug ? "(Debug)" : $"v{s_version}";
}