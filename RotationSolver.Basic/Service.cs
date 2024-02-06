using Dalamud.Utility.Signatures;
using ECommons.DalamudServices;
using FFXIVClientStructs.Attributes;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Excel;
using RotationSolver.Basic.Configuration;

namespace RotationSolver.Basic;

internal class Service : IDisposable
{
    public const string COMMAND = "/rotation", USERNAME = "Jaksuhn", REPO = "RotationSolver", BRANCH = "testing";

    static bool _canMove = true;
    internal static unsafe bool CanMove
    {
        set
        {
            var realCanMove = value || DataCenter.NoPoslock;
            if (_canMove == realCanMove) return;
            _canMove = realCanMove;
        }
    }

    public static float CountDownTime => Countdown.TimeRemaining;
    public static PluginConfig Config { get; set; } = new PluginConfig();

    public Service()
    {
        Svc.Hook.InitializeFromAttributes(this);
    }
    public static ActionID GetAdjustedActionId(ActionID id)
        => (ActionID)GetAdjustedActionId((uint)id);

    public static unsafe uint GetAdjustedActionId(uint id)
    => ActionManager.Instance()->GetAdjustedActionId(id);

    public unsafe static IEnumerable<IntPtr> GetAddons<T>() where T : struct
    {
        if (typeof(T).GetCustomAttribute<Addon>() is not Addon on) return Array.Empty<nint>();

        return on.AddonIdentifiers
            .Select(str => Svc.GameGui.GetAddonByName(str, 1))
            .Where(ptr => ptr != IntPtr.Zero);
    }

    public static ExcelSheet<T> GetSheet<T>() where T : ExcelRow => Svc.Data.GetExcelSheet<T>();

    public void Dispose() { }
}
