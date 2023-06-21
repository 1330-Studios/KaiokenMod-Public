using System.Collections.Generic;

using KaiokenMod.Items.Accessories;
using KaiokenMod.Items.Essence.Pure;
using KaiokenMod.Items.Essence.Tier2;
using KaiokenMod.Utils;

using Terraria;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Essence;
internal static class KaiokenEssenceHelper {
    internal const int IDX_ENLIGHTENMENT = 0;
    internal const int IDX_WORLDKING = 1;

    internal const int IDX_ROYALTAIL = 2; // 5% DR
    internal const int IDX_CYBERNETICENHANCEMENTS = 3; // 5 Defense
    internal const int IDX_SCIENTISTBRAIN = 4; // 25 Max Mana
    internal const int IDX_FRUITOFMIGHT = 5; // 100 Max Health

    internal const int IDX_ASSASSINSBELT = 6; // 7% Dodge
    internal const int IDX_COLOREXPLOSION = 7; // 15% Crit Chance
    internal const int IDX_HALOOFLIGHT = 8; // 25 Generic Damage

    internal const int IDX_ZSOUL = 13; // Double Tap to Dash
    internal const int IDX_SUPERSOUL = 14; // 10% Movement Speed

    internal const int IDX_KAIOKENESSENCE = 15; // Ichor affliction


    internal static readonly Dictionary<int, int[]> EssenceIdx = new();

    static KaiokenEssenceHelper() {
        EssenceIdx[ModContent.ItemType<EnlightenmentSymbol>()] = new[] { IDX_ENLIGHTENMENT };
        EssenceIdx[ModContent.ItemType<WorldKingSymbol>()] = new[] { IDX_WORLDKING };
        EssenceIdx[ModContent.ItemType<RoyalTail>()] = new[] { IDX_ROYALTAIL };
        EssenceIdx[ModContent.ItemType<CyberneticEnhancements>()] = new[] { IDX_CYBERNETICENHANCEMENTS };
        EssenceIdx[ModContent.ItemType<ScientistBrain>()] = new[] { IDX_SCIENTISTBRAIN };
        EssenceIdx[ModContent.ItemType<FruitOfMight>()] = new[] { IDX_FRUITOFMIGHT };

        EssenceIdx[ModContent.ItemType<AssassinsBelt>()] = new[] { IDX_ASSASSINSBELT };
        EssenceIdx[ModContent.ItemType<HaloOfLight>()] = new[] { IDX_HALOOFLIGHT };
        EssenceIdx[ModContent.ItemType<ColorExplosion>()] = new[] { IDX_COLOREXPLOSION };

        EssenceIdx[ModContent.ItemType<ZSoul>()] = new[] { IDX_ROYALTAIL, IDX_CYBERNETICENHANCEMENTS, IDX_SCIENTISTBRAIN, IDX_FRUITOFMIGHT, IDX_ZSOUL };
        EssenceIdx[ModContent.ItemType<SuperSoul>()] = new[] { IDX_ASSASSINSBELT, IDX_COLOREXPLOSION, IDX_SUPERSOUL };

        EssenceIdx[ModContent.ItemType<KaiokenEssence>()] = new[] { IDX_ENLIGHTENMENT, IDX_WORLDKING, IDX_ROYALTAIL, IDX_CYBERNETICENHANCEMENTS, IDX_SCIENTISTBRAIN, IDX_FRUITOFMIGHT, IDX_ASSASSINSBELT, IDX_COLOREXPLOSION, IDX_ZSOUL, IDX_SUPERSOUL, IDX_KAIOKENESSENCE };
    }

    public static BitsShort GetKaiokenEssenceStatus(Player player) {
        BitsShort returnBitsShort = new();

        for (var j = 3; j < 10; j++) {
            if (!player.IsAValidEquipmentSlotForIteration(j)) continue;

            if (player.armor[j].vanity || !player.armor[j].accessory)
                continue;

            if (player.armor[j].ModItem == null || !EssenceIdx.TryGetValue(player.armor[j].ModItem.Type, out var intsToSet)) continue;

            foreach (var intToSet in intsToSet) {
                returnBitsShort[intToSet] = true;
            }
        }

        return returnBitsShort;
    }

    internal static readonly string[] _multipliers = { "", " x3", " x4", " x10", " x20", " x100" };

    public static int GetKaiokenMultiplierStatus(BitsShort bits) {
        var idx = 0;

        if (bits[IDX_ENLIGHTENMENT] || bits[IDX_WORLDKING] || bits[IDX_ROYALTAIL] || bits[IDX_CYBERNETICENHANCEMENTS] ||
            bits[IDX_SCIENTISTBRAIN] || bits[IDX_FRUITOFMIGHT])
            idx = 1;

        if (bits[IDX_ASSASSINSBELT] || bits[IDX_HALOOFLIGHT] || bits[IDX_COLOREXPLOSION])
            idx = 2;

        if (bits[IDX_ZSOUL])
            idx = 3;

        if (bits[IDX_SUPERSOUL])
            idx = 4;

        if (bits[IDX_KAIOKENESSENCE])
            idx = 5;

        return idx;
    }
}