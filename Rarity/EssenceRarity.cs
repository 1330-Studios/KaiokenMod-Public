﻿using System;

using Microsoft.Xna.Framework;

using Terraria.ModLoader;

namespace KaiokenMod.Rarity;
internal class EssenceRarity : ModRarity {
    public override Color RarityColor => GetColor();

    private static Color GetColor() {
        var currentTime = DateTime.Now.TimeOfDay;
        var progress = currentTime.TotalSeconds % 2 / 2.0;
        var idx = ((int)(progress * _colors.Length) % _colors.Length);
        var color = _colors[idx];
        var r = (color >> 16) & 0xFF;
        var g = (color >> 8) & 0xFF;
        var b = color & 0xFF;
        return new Color(r, g, b);
    }

    // It works...
    private static readonly int[] _colors = {
        0xff1000, 0xff1101, 0xff1302, 0xfe1503, 0xff1704, 0xff1905, 0xff1b06, 0xff1d07, 0xff1f08, 0xff2109, 0xff230a,
        0xff250b, 0xff270c, 0xff290d, 0xff2b0e, 0xff2d0f, 0xfe2f10, 0xff3111, 0xff3312, 0xff3513, 0xff3714, 0xfe3915,
        0xff3b16, 0xff3d17, 0xff3f18, 0xff4119, 0xfe431a, 0xff451b, 0xff471c, 0xff491d, 0xff4a1f, 0xff4c20, 0xff4e21,
        0xff5022, 0xff5223, 0xff5424, 0xff5625, 0xff5826, 0xff5a27, 0xff5c28, 0xff5e29, 0xff602a, 0xff622b, 0xff642c,
        0xff662d, 0xff682e, 0xff6a2f, 0xff6c30, 0xff6e31, 0xff7032, 0xff7233, 0xff7434, 0xff7635, 0xff7836, 0xff7a37,
        0xff7c38, 0xff7e39, 0xff803a, 0xff823b, 0xff843d, 0xff823b, 0xff803a, 0xfe7e39, 0xff7c38, 0xff7a37, 0xff7836,
        0xff7635, 0xff7434, 0xff7233, 0xff7032, 0xff6e31, 0xff6c30, 0xff6a2f, 0xff682e, 0xff662d, 0xfe642c, 0xff622b,
        0xff602a, 0xff5e29, 0xff5c28, 0xfe5a27, 0xff5826, 0xff5625, 0xff5424, 0xff5223, 0xfe5022, 0xff4e21, 0xff4c20,
        0xff4a1f, 0xff491d, 0xff471c, 0xff451b, 0xff431a, 0xff4119, 0xff3f18, 0xff3d17, 0xff3b16, 0xff3915, 0xff3714,
        0xff3513, 0xff3312, 0xff3111, 0xff2f10, 0xff2d0f, 0xff2b0e, 0xff290d, 0xff270c, 0xff250b, 0xff230a, 0xff2109,
        0xff1f08, 0xff1d07, 0xff1b06, 0xff1905, 0xff1704, 0xff1503, 0xff1302, 0xff1101
    };
}