global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Terraria;
global using Terraria.Localization;
global using Terraria.ModLoader;
using ILLogger.Common;

namespace ILLogger;

/// <summary> The Core of ILLogger. </summary>
public class Logger : Mod
{
    /// <summary> Manually loads required data to <paramref name="mod"/> if <see cref="LogUtils.LogIL"/> isn't called first. </summary>
    /// <param name="mod"> Your mod. </param>
    public static void Load(Mod mod) => mod.AddContent<AppendLogHook>();
}