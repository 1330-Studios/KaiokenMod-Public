using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace KaiokenMod.Utils;
public record Gradient {
    private readonly List<Tuple<double, Color>> gradientStops;

    public Gradient() {
        gradientStops = new List<Tuple<double, Color>>();
    }

    public Gradient(Color startColor, params (double percent, Color color)[] subsequentColors) {
        gradientStops = new List<Tuple<double, Color>>() { new(0, startColor) };
        gradientStops.AddRange(from color_value_pair in subsequentColors select new Tuple<double, Color>(color_value_pair.percent, color_value_pair.color));
    }

    public Color GetColor(double percent) {
        if (percent is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(percent), "Percent must be between 0 and 1");

        switch (gradientStops.Count) {
            case 0:
                return Color.Black;
            case 1:
                return gradientStops[0].Item2;
        }

        // Find the two nearest gradient stops
        Tuple<double, Color> stop1 = null!;
        Tuple<double, Color> stop2 = null!;
        foreach (var stop in gradientStops) {
            if (stop.Item1 <= percent) {
                stop1 = stop;
            } else {
                stop2 = stop;
                break;
            }
        }

        if (stop1 == null && stop2 == null)
            throw new Exception("How did we get here?");

        // If there is no stop2, then the percent is greater than all of the gradient stops, so return the last stop's color
        if (stop2 == null)
            return stop1!.Item2;

        // Find the percent distance between the two nearest stops
        var percentDistance = stop2.Item1 - stop1!.Item1;
        var percentThroughStops = (percent - stop1.Item1) / percentDistance;

        // Find the R, G, and B values of the color between the two stops
        var r = stop1.Item2.R + (stop2.Item2.R - stop1.Item2.R) * percentThroughStops;
        var g = stop1.Item2.G + (stop2.Item2.G - stop1.Item2.G) * percentThroughStops;
        var b = stop1.Item2.B + (stop2.Item2.B - stop1.Item2.B) * percentThroughStops;
        var a = stop1.Item2.A + (stop2.Item2.A - stop1.Item2.A) * percentThroughStops;

        return new Color((int)r, (int)g, (int)b, (int)a);
    }
}