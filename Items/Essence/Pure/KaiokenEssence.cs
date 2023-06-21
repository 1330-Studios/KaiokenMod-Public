using System;

using KaiokenMod.Items.Accessories;
using KaiokenMod.Items.Essence.Tier2;
using KaiokenMod.Particle;
using KaiokenMod.Rarity;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Essence.Pure;

internal class KaiokenEssence : ModItem {
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("The pure essence of Kaio-ken.\n+ 1,000% Kaio-ken Mastery Gain.\n+ 70% Chance to not gain Strain when using Kaio-ken.\n+ 10% Dodge Chance while in Kaio-ken.\n+ 7.5% Damage Reduction while in Kaio-ken.\n+ 10 Defense while in Kaio-ken.\n+ 50 Max Mana while in Kaio-ken.\n+ 150 Max Health while in Kaio-ken.\n+ 50 Base Generic Damage while in Kaio-ken.\n+ 25% Crit Chance while in Kaio-ken.\n+ 15% Movement Speed.\n+ Double tap to Dash.\n+ Applies Ichor to enemies hit.");
        DisplayName.SetDefault("Kaio-ken Essence");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Item.type] = 1;

        // Thanks SoulsOfTheOuterGods mod
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
        ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        ItemID.Sets.ItemIconPulse[Item.type] = true;
        ItemID.Sets.ItemNoGravity[Item.type] = true;
    }

    public override void AddRecipes() {
        CreateRecipe().AddTile(TileID.LunarCraftingStation).AddIngredient<ZSoul>().AddIngredient<SuperSoul>().AddIngredient<EnlightenmentSymbol>().AddIngredient<WorldKingSymbol>()
            .AddIngredient(ItemID.FragmentNebula, 10).AddIngredient(ItemID.FragmentSolar, 10).AddIngredient(ItemID.FragmentStardust, 10).AddIngredient(ItemID.FragmentVortex, 10).Register();
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.dashType = 3;

        if (hideVisual) return;
        for (var i = 0; i < 3; i++)
            SpawnParticles(player);
    }

    private int _ParticleTimer;

    public void SpawnParticles(Entity player) {
        if (++_ParticleTimer % 5 == 0)
            return;

        var origin = player.Center + Main.rand.NextVector2Circular(10f, 10f);
        origin.Y += (player.height / 2f);

        var direction = origin.DirectionTo(player.Center);

        var velocity = new Vector2(-direction.X, 0) * .888f;
        velocity /= 2;

        var fadingParticle = ParticleHelper._poolFadingAura.RequestParticle();
        fadingParticle.SetBasicInfo(ModContent.Request<Texture2D>("KaiokenMod/Particle/AuraDust"), null, Vector2.Zero,
            Vector2.Zero);
        fadingParticle.SetTypeInfo(20);
        fadingParticle.Velocity = velocity + new Vector2(0, -0.18f);
        fadingParticle.AccelerationPerFrame = velocity * 0.033f + new Vector2(0, -0.18f);

        var color = new Color(0xFD, 0xE0, 0x22);

        if ((_ParticleTimer + 1) % 2 == 0)
            color = new Color(0xFF, 0x95, 0x62);

        fadingParticle.ColorTint = color;
        fadingParticle.LocalPosition = origin;
        fadingParticle.Rotation = (float)Random.Shared.NextDouble();
        fadingParticle.FadeInNormalizedTime = 0.25f;
        fadingParticle.FadeOutNormalizedTime = 0.35f;
        fadingParticle.Scale = new Vector2(1, 1) * (float)((Random.Shared.NextDouble() * 0.6f) + 0.4f);

        if (_ParticleTimer % 10 <= 4)
            Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
        else
            Main.ParticleSystem_World_BehindPlayers.Add(fadingParticle);
    }

    public override void SetDefaults() {
        Item.color = Color.White;
        Item.width = 18;
        Item.height = 18;
        Item.value = 800000;
        Item.maxStack = 1;
        Item.rare = ModContent.RarityType<EssenceRarity>();
        Item.defense = 0;
        Item.accessory = true;
    }
}