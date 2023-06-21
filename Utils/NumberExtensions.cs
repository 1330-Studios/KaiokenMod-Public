using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;

namespace KaiokenMod.Utils;
internal static class NumberExtensions {
    /// <summary>
    /// Ease In and Out a float value, between 0 and 1
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    internal static float EaseInOut(this float t) {
        return t < 0.5f ? 2.0f * t * t : -1.0f + (4.0f - 2.0f * t) * t;
    }

    /// <summary>
    /// Averages doubles with a base of being 0
    /// </summary>
    /// <param name="doubles"></param>
    /// <returns></returns>
    internal static double AverageDoubles(this IEnumerable<double> doubles) {
        var count = 0;
        double total = 0;

        using var enumerator = doubles.GetEnumerator();

        while (enumerator.MoveNext()) {
            count++;
            total += enumerator.Current;
        }

        if (count != 0)
            return total / count;
        return 0;
    }
    /// <summary>
    /// Converts screen percentages to Screen positions
    /// </summary>
    /// <param name="v2">Percentage on screen</param>
    /// <returns></returns>
    internal static Vector2 ToScreenPos(this Vector2 v2) {
        v2.X = v2.X * 0.01f * Main.screenWidth;
        v2.Y = v2.Y * 0.01f * Main.screenHeight;
        return v2;
    }
    /// <summary>
    /// Reverses ToScreenPos
    /// </summary>
    /// <param name="v2">Screen position</param>
    /// <returns></returns>
    internal static Vector2 FromScreenPos(this Vector2 v2) {
        v2.X = v2.X / 0.01f / Main.screenWidth;
        v2.Y = v2.Y / 0.01f / Main.screenHeight;
        return v2;
    }

    /// <summary>
    /// Keep only specified decimal portions of an input number.
    /// </summary>
    /// <param name="f2">Float to be truncated</param>
    /// <param name="decimalPlaces">Number of decimal places to keep</param>
    /// <returns>Truncated float with <paramref name="decimalPlaces"/> decimal places.</returns>
    internal static float Truncate(this float f2, int decimalPlaces) {
        return float.Parse(f2.ToString($"N{decimalPlaces}"));
    }
}