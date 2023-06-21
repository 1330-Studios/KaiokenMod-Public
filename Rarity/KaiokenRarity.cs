using KaiokenMod.Utils;

using Microsoft.Xna.Framework;

using Terraria.ModLoader;

namespace KaiokenMod.Rarity;

internal class KaiokenRarity : ModRarity {
    public override Color RarityColor => KaiokenConfig.Instance.AuraColor with { A = 255 };
}