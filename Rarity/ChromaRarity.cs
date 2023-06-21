using System;

using Microsoft.Xna.Framework;

using Terraria.ModLoader;

namespace KaiokenMod.Rarity;
internal class ChromaRarity : ModRarity {
    public override Color RarityColor => GetColor();

    private static Color GetColor() {
        var currentTime = DateTime.Now.TimeOfDay;
        var hue = currentTime.TotalSeconds % 5 / 5.0;
        return HSLToColor(hue, 1.0, 0.5);
    }

    private static Color HSLToColor(double hue, double saturation, double lightness) {
        var chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;
        var huePrime = hue * 6;
        var x = chroma * (1 - Math.Abs(huePrime % 2 - 1));

        double red = 0, green = 0, blue = 0;
        switch (huePrime) {
            case >= 0 and < 1:
                red = chroma;
                green = x;
                break;
            case >= 1 and < 2:
                red = x;
                green = chroma;
                break;
            case >= 2 and < 3:
                green = chroma;
                blue = x;
                break;
            case >= 3 and < 4:
                green = x;
                blue = chroma;
                break;
            case >= 4 and < 5:
                red = x;
                blue = chroma;
                break;
            case >= 5 and < 6:
                red = chroma;
                blue = x;
                break;
        }

        var m = lightness - chroma / 2;
        red += m;
        green += m;
        blue += m;

        var r = (int)Math.Round(red * 255);
        var g = (int)Math.Round(green * 255);
        var b = (int)Math.Round(blue * 255);

        return new Color(r, g, b);
    }
}
