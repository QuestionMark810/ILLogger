using System.Collections.Generic;
using ILLogger.Common;
using Terraria.GameContent;

namespace ILLogger;

/// <summary> Includes static helpers related to logging. See <see cref="LogIL"/>. </summary>
public static class LogUtils
{
    /// <summary> All stored logs resulting from <see cref="LogIL"/>. </summary>
    public static readonly List<string> Logs = [];

    /// <summary> Automatically formats IL logs using <paramref name="title"/> and <paramref name="message"/> and stores them in <see cref="Logs"/>. </summary>
    /// <param name="mod"> The mod responsible for the log. </param>
    /// <param name="title"> The name of this edit. </param>
    /// <param name="message"> The message to log. </param>
    public static void LogIL(this Mod mod, string title, string message)
    {
        mod.Logger.Warn($"IL edit '{title}' failed! " + message);
        Logs.Add(title + " failed to load! " + message);

        MenuErrorPopup.CreatePopup();

        if (!AppendLogHook.Loaded)
            AppendLogHook.LoadStatic(mod);
    }

    /// <summary> Wraps <paramref name="text"/> like <see cref="Utils.WordwrapString"/> but with respect for newline. </summary>
    internal static string[] WrapText(string text, int bounds)
    {
        if (bounds < 0)
            return [text];

        string[] subText = text.Split('\n');
        List<string> result = [];

        foreach (string line in subText)
        {
            string[] wrapped = Utils.WordwrapString(line, FontAssets.MouseText.Value, bounds, 20, out int length);

            for (int i = 0; i < length + 1; i++)
                result.Add(wrapped[i]);
        }

        return [.. result];
    }
}