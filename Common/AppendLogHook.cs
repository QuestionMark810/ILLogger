using ILLogger.Common.Elements;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria.UI;

namespace ILLogger.Common;

[Autoload(false)]
internal class AppendLogHook : ILoadable
{
	internal static bool Loaded => Mod != null;

	internal static Mod Mod { get; private set; }

	private static Hook ModUIInitHook = null;
	private static PropertyInfo ModNameInfo;

	internal static void LoadStatic(Mod mod)
	{
        Mod = mod;

        var type = typeof(Mod).Assembly.GetType("Terraria.ModLoader.UI.UIModItem");
        var info = type.GetMethod("OnInitialize");

        ModUIInitHook = new Hook(info, HookModIcon, true);
        ModNameInfo = type.GetProperty("ModName", BindingFlags.Public | BindingFlags.Instance);
    }

	public static void HookModIcon(Action<object> orig, object self)
	{
		orig(self);

		string name = ModNameInfo.GetValue(self) as string;
        if (name == Mod.Name && LogUtils.Logs.Count != 0) //Add the error log
        {
            var menuButton = new UIErrorLogButton(Mod);
            menuButton.Left.Set(184, 0);
            menuButton.Top.Set(38, 0);

            (self as UIElement).Append(menuButton);
        }
    }

	public void Load(Mod _) { }
	public void Unload()
	{
		ModUIInitHook.Undo();
		ModUIInitHook = null;
		ModNameInfo = null;
	}
}