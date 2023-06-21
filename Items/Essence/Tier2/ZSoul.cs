using System;

using KaiokenMod.Particle;
using KaiokenMod.Rarity;
using KaiokenMod.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Items.Essence.Tier2;
internal class ZSoul : ModItem {
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("The fused power of all Z essence.\n+ 5% Damage Reduction while in Kaio-ken.\n+ 5 Defense while in Kaio-ken.\n+ 25 Max Mana while in Kaio-ken.\n+ 100 Max Health while in Kaio-ken.\n+ Double tap to Dash.");
        DisplayName.SetDefault("Z Soul");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Item.type] = 1;
    }

    public override void AddRecipes() {
        CreateRecipe().AddTile(TileID.AdamantiteForge).AddIngredient<RoyalTail>().AddIngredient<CyberneticEnhancements>()
            .AddIngredient<ScientistBrain>().AddIngredient<FruitOfMight>().AddIngredient(ItemID.SoulofMight, 5).Register();
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        var kPlayer = player.GetModPlayer<KPlayer>();

        if (((BitsShort)kPlayer.Essence)[13])
            player.dashType = ((BitsShort)kPlayer.Essence)[15] ? 3 : 2;

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
        fadingParticle.SetTypeInfo(25);
        fadingParticle.Velocity = velocity + new Vector2(0, -0.16f);
        fadingParticle.AccelerationPerFrame = velocity * 0.033f + new Vector2(0, -0.16f);
        fadingParticle.ColorTint = new Color(0xFD, 0xE0, 0x22);
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