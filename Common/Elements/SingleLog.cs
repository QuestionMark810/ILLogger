using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace ILLogger.Common.Elements;

internal class SingleLog : UIElement
{
	private const float TextScale = 0.95f;
	private const int Spacing = 30;

	private bool Extended => Height.Pixels != Spacing;
	private int WrapSpace
	{
		get
		{
			float width = GetDimensions().Width;
			return (int)(width * (2f - TextScale)) - 38;
		}
	}

	private readonly UIPanel _panel;
	private UIImageButton _extend;

	private readonly string _text;

	public SingleLog(string text)
	{
		_text = text;

		Width.Set(0, 1);
		Height.Set(30, 0);

        _panel = new()
        {
            Width = Width,
            Height = Height,
            BackgroundColor = Color.DarkSlateGray * 0.1f,
            BorderColor = Color.Black * 0.2f
        };

        Append(_panel);
	}

	public override void OnActivate() //_extend can't be appended in the constructor because it relies on Parent dimensions to initialize
	{
		int numLines = LogUtils.WrapText(_text, WrapSpace).Length;
		if (numLines > 1)
		{
            _extend = new(UICommon.ButtonUpDownTexture)
            {
                HAlign = 1,
                VAlign = 0.5f
            };

            _extend.OnLeftClick += (evt, listeningElement) => OnExtend();
			Append(_extend);
		}
	}

	private void OnExtend()
	{
		int numLines = LogUtils.WrapText(_text, WrapSpace).Length;
		AdjustHeight(Extended ? Spacing : Spacing * numLines);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		string[] wrappedText = LogUtils.WrapText(_text, WrapSpace);
		for (int i = 0; i < (Extended ? wrappedText.Length : 1); i++)
		{
			var position = GetDimensions().Position() + new Vector2(4, Spacing * 0.6f + Spacing * i);
			Utils.DrawBorderString(spriteBatch, wrappedText[i], position, Main.MouseTextColorReal, TextScale, anchory: 0.5f);
		}
	}

	private void AdjustHeight(int pixels = 30)
	{
		Height.Set(pixels, 0);
		_panel.Height = Height;
	}
}