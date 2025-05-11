using MonoMod.Cil;
using System;
using Terraria.ModLoader.UI;

namespace ILLogger.Common;

/// <summary> Handles popups used to indicate errors as they occur. See <see cref="CreatePopup"/>. </summary>
public static class MenuErrorPopup
{
    private static float Opacity;
    private static bool Loaded { get; set; }

    /// <summary> Creates a popup. </summary>
    public static void CreatePopup()
    {
        if (!Loaded)
        {
            Loaded = true;
            IL_Main.DrawMenu += DrawMenu;
            On_Main.UpdateUIStates += On_Main_UpdateUIStates;
        }

        Opacity = 1f;
    }

    private static void DrawMenu(ILContext il)
    {
        ILCursor c = new(il);

        if (c.TryGotoNext(MoveType.After, x => x.MatchCall("Terraria.ModLoader.MenuLoader", "UpdateAndDrawModMenu")))
            c.EmitDelegate(DoDraw); 
    }

    private static void DoDraw()
    {
        if (Opacity == 0)
            return;

        var texture = UICommon.ButtonErrorTexture.Value;
        var position = new Vector2(20);
        float scale = 1 + Math.Max(Opacity - 0.75f, 0) * 0.2f;

        Main.spriteBatch.Draw(texture, position, null, Color.White * Opacity, 0, texture.Size() / 2, scale, default, 0);

        string text = Language.GetTextValue("Encountered an error. See Mods List");
        Utils.DrawBorderString(Main.spriteBatch, text, position + new Vector2(texture.Width / 2 + 4, 0), Main.MouseTextColorReal * Opacity * 2, scale * 0.9f, anchory: 0.4f);
    }

    private static void On_Main_UpdateUIStates(On_Main.orig_UpdateUIStates orig, GameTime gameTime)
    {
        orig(gameTime);
        Opacity = Math.Max(Opacity - 0.01f, 0);
    }
}