using System;

using KaiokenMod.Particle;
using KaiokenMod.Rarity;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Essence.Tier2;
internal class SuperSoul : ModItem {
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("The fused power of all Super essence.\n+ 7% Dodge Chance while in Kaio-ken.\n+ 25 Base Generic Damage while in Kaio-ken.\n+ 15% Crit Chance while in Kaio-ken.\n+ 10% Movement Speed.");
        DisplayName.SetDefault("Super Soul");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Item.type] = 1;
    }

    public override void AddRecipes() {
        CreateRecipe().AddTile(TileID.AdamantiteForge).AddIngredient<AssassinsBelt>().AddIngredient<HaloOfLight>().AddIngredient<ColorExplosion>()
            .AddIngredient(ItemID.SoulofSight, 5).AddIngredient(ItemID.SoulofFright, 5).Register();
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        if (hideVisual) return;
        SpawnParticles(player);
    }

    private int _ParticleTimer;
    public void SpawnParticles(Entity player) {
        if (++_ParticleTimer % 3 != 0)
            return;

        var origin = player.Center + Main.rand.NextVector2Circular(10f, 10f);
        origin.Y += (player.height / 2f);

        var direction = origin.DirectionTo(player.Center);

        var velocity = new Vector2(-direction.X, 0) * .888f;
        velocity /= 2;

        var fadingParticle = ParticleHelper._poolFadingAura.RequestParticle();
        fadingParticle.SetBasicInfo(ModContent.Request<Texture2D>("KaiokenMod/Particle/AuraDust"), null, Vector2.Zero, Vector2.Zero);
        fadingParticle.SetTypeInfo(20);
        fadingParticle.Velocity = velocity + new Vector2(0, -0.18f);
        fadingParticle.AccelerationPerFrame = velocity * 0.033f + new Vector2(0, -0.18f);
        fadingParticle.ColorTint = new Color(0xFF, 0x95, 0x62);
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
        Item.width = 38;
        Item.height = 34;
        Item.value = 800000;
        Item.maxStack = 1;
        Item.rare = ModContent.RarityType<ChromaRarity>();
        Item.defense = 0;
        Item.accessory = true;
    }
}