using Terraria.Graphics.Renderers;

namespace KaiokenMod.Particle;

internal static class ParticleHelper {
    internal static ParticlePool<FadingParticle> _poolFadingAura = new(200, () => new FadingParticle());
}