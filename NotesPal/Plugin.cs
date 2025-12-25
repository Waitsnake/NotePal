using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.Objects;
using NotesPal.Commands;
using NotesPal.Context;
using NotesPal.Data;
using NotesPal.Windows;

namespace NotesPal;

public class Plugin : IDalamudPlugin
{
    public string Name => "NotesPal";
    public const string Version = "0.1.0";

    [PluginService] public static IObjectTable ObjectCollection { get; private set; } = null!;
    [PluginService] public static IPluginLog PluginLog { get; set; } = null!;
    [PluginService] public static IContextMenu ContextMenu { get; set; } = null!;


    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        Services.Instance = pluginInterface.Create<Services>()!;
        Services.Instance.PluginInterface = pluginInterface;
        Services.Instance.WindowSystem = new WindowSystem(Name);
        Services.Instance.PluginInterface.UiBuilder.Draw += Services.Instance.WindowSystem.Draw;
        Services.Instance.PluginInterface.UiBuilder.OpenMainUi += MainWindow.Instance.Toggle;
        
        CommandCreator.Initialize();
        NoteContextAction.Initialize();
        
        Services.Instance.PluginLog.Info($"{NoteDb.Count()} memos where loaded.");
    }

    public void Dispose()
    {
        Services.Instance.PluginInterface.UiBuilder.OpenMainUi -= MainWindow.Instance.Toggle;
        Services.Instance.WindowSystem.RemoveAllWindows();
    }
}

