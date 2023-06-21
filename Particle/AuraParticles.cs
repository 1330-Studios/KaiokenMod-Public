using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Utilities;

namespace KaiokenMod.Particle;
internal class AuraParticles {
    internal List<(Vector2 position, long timestamp)> Particles = new();

    internal static UnifiedRandom UnifiedRandom = new();
    internal static Vector2 GetStartPos() => new((float)Random.Shared.NextDouble() * 6 - 3, (float)Random.Shared.NextDouble() * 8 - 4);
    internal const float Y_Gain = -0.0083333333333333f;

    internal ulong tick;

    internal void Update(Player player, bool add = true) {
        var currentTimeStamp = DateTime.UtcNow - new DateTime(1970, 1, 1);

        Particles.RemoveAll(particle => particle.timestamp + 1000 < currentTimeStamp.TotalMilliseconds);

        for (var i = 0; i < Particles.Count; i++) {
            var particle = Particles[i];

            particle.position.Y += Y_Gain;

            Particles[i] = particle;
        }

        if (add && tick % 3 == 0)
            Particles.Add(((GetStartPos() * 8) + player.position + player.velocity * 3.3333f, (long)currentTimeStamp.TotalMilliseconds));

        tick++;
    }

    internal void Update(bool add = true) {
        var currentTimeStamp = DateTime.UtcNow - new DateTime(1970, 1, 1);

        Particles.RemoveAll(particle => particle.timestamp + 1000 < currentTimeStamp.TotalMilliseconds);

        for (var i = 0; i < Particles.Count; i++) {
            var particle = Particles[i];

            particle.position.Y += Y_Gain;

            Particles[i] = particle;
        }

        if (add && tick % 7 == 0)
            Particles.Add((UnifiedRandom.NextVector2Circular(32, 32), (long)currentTimeStamp.TotalMilliseconds));

        tick++;
    }
}