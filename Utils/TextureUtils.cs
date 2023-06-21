using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KaiokenMod.Utils;
internal static class TextureUtils {
    private static readonly Dictionary<(string Identifier, float Progress), Texture2D> textureCache = new();

    public static Texture2D InterpolateImages(this Texture2D texture1, Texture2D texture2, float percentage, string ID) {
        if (textureCache.TryGetValue((ID, percentage), out var texture))
            return texture;

        var progress = percentage.EaseInOut();
        var width = texture1.Width;
        var height = texture1.Height;

        // Ensure that the input images have the same dimensions
        if (texture1.Width != texture2.Width || texture1.Height != texture2.Height)
            throw new ArgumentException("Input images must have the same dimensions.");

        // Create a new bitmap for the interpolated image
        var outputTexture = new Texture2D(texture1.GraphicsDevice, width, height);

        var outputColors = new Color[width * height];

        var colors1 = new Color[width * height];
        var colors2 = new Color[width * height];

        texture1.GetData(colors1);
        texture2.GetData(colors2);

        Parallel.For(0, outputColors.Length, i => {
            if (colors1[i].A == 0 && colors2[i].A == 0)
                return;

            // Get the color of the corresponding pixels in the input images
            var color1 = colors1[i];
            var color2 = colors2[i];

            // Calculate the weighted average of the two colors based on the percentage
            var red = (int)((color1.R * progress) + (color2.R * (1 - progress)));
            var green = (int)((color1.G * progress) + (color2.G * (1 - progress)));
            var blue = (int)((color1.B * progress) + (color2.B * (1 - progress)));
            var alpha = (int)((color1.A * progress) + (color2.A * (1 - progress)));

            outputColors[i] = new Color(red, green, blue, alpha);
        });

        outputTexture.SetData(outputColors);

        textureCache.TryAdd((ID, percentage), outputTexture);

        return outputTexture;
    }
}