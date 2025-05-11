using Humanizer;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace ILLogger.Common.Elements;

internal class UIErrorLogButton : UIElement
{
	protected Mod Mod { get; private set; }

	public UIErrorLogButton(Mod mod)
	{
		Mod = mod;

		Width.Set(30, 0);
		Height.Set(30, 0);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		var pos = GetDimensions().Center();

		if (IsMouseHovering)
		{
			int count = LogUtils.Logs.Count;
			UICommon.TooltipMouseText(Language.GetTextValue(count + " system(s) failed to load\nClick to open log").FormatWith(count));

			if (Main.mouseLeft && Main.mouseLeftRelease)
				UIInfoMessage.Instance.ShowMessage(Mod);
		}

		var texture = UICommon.ButtonErrorTexture.Value;
		spriteBatch.Draw(texture, pos, null, Color.White, 0, texture.Size() / 2, 1, default, 0);
	}
}