using KaiokenMod.Utils;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace KaiokenMod.Particle;
internal class AuraDust : ModDust {
    public override void OnSpawn(Dust dust) {
        dust.noLight = false;
        dust.scale = 1.2f;
        dust.noGravity = true;
        dust.alpha = 100;
        dust.color = dust.color with { A = 0 };
    }

    public override bool Update(Dust dust) {
        if (dust.color.A < 100)
            dust.color.A += 20;

        dust.scale -= 0.04f;
        if (dust.scale < 0.1f) {
            dust.active = false;
        }

        var data = (KPlayer.AuraData)dust.customData;

        if (data.Gravity != 0 && data.Player.GetModPlayer<KPlayer>().JustFormed != 0) {
            const float far = float.MaxValue;
            dust.position = new Vector2(far, far);
            dust.scale = 0;
            dust.active = false;
            return false;
        }

        dust.position += dust.velocity;
        dust.velocity *= 0.9f;

        dust.position.Y += data.Gravity;

        return false;
    }
}
