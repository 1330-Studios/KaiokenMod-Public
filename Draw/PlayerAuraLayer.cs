using System;
using System.Collections.Generic;

using KaiokenMod.Buffs;
using KaiokenMod.Particle;
using KaiokenMod.Utils;
using KaiokenMod.Utils.DBT;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Draw;

internal sealed class PlayerAuraLayer : PlayerDrawLayer {
    public Dictionary<int, AuraParticles> aura = new();

    public override Transformation Transform => PlayerDrawLayers.TorsoGroup;

    public override bool IsHeadLayer => false;

    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Torso);
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => true;

    public override void Unload() {
        aura = null;
    }

    protected override void Draw(ref PlayerDrawSet drawInfo) {
        if (Main.netMode == NetmodeID.Server)
            return;
        if (drawInfo.shadow != 0)
            return;
        var player = drawInfo.drawPlayer;

        var currentTimeStamp = DateTime.UtcNow - new DateTime(1970, 1, 1);

        var samplerState = Main.DefaultSamplerState;
        if (player.mount.Active)
            samplerState = LegacyPlayerRenderer.MountedSamplerState;

        aura.TryAdd(player.whoAmI, new AuraParticles());

        var ap = aura[player.whoAmI];

        var kaiokenBuffed = player.HasBuff<KaiokenBuff>();

        var mode = KaiokenConfig.Instance.AuraInsteadOfParticles;

        if (player.GetModPlayer<KPlayer>().FormedYet) {
            if (kaiokenBuffed)
                DrawAura(Main.spriteBatch, player, samplerState, mode);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, samplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            DrawParticles(Main.spriteBatch, player, samplerState, mode, ap, currentTimeStamp);
        }

        ap.Update(player, kaiokenBuffed);
        aura[player.whoAmI] = ap;
    }

    private static void DrawParticles(SpriteBatch spriteBatch, Entity player, SamplerState samplerState, bool mode, AuraParticles ap, TimeSpan currentTimeStamp) {
        //expects active spritebatch
        //spritebatch leaves as active AlphaBlend

        var colors = new Gradient(Color.Transparent, (0.25, KaiokenConfig.Instance.AuraColor), (0.75, KaiokenConfig.Instance.AuraColor), (.975, Color.Transparent));

        var auraTexture = ModContent.Request<Texture2D>($"KaiokenMod/Aura/{(KaiokenConfig.Instance.V20XAuraParticles ? "Flames" : "Fire")}", AssetRequestMode.ImmediateLoad).Value;

        const int frameSize = 128;
        const int sheetSize = 512;
        const int numberOfFrames = sheetSize / frameSize * (sheetSize / frameSize);


        foreach (var (position, timestamp) in ap.Particles) {
            var progress = Math.Clamp((currentTimeStamp.TotalMilliseconds - timestamp) / 900.0, 0f, 1f);

            var frameIndex = (int)(progress * numberOfFrames);

            var x = frameIndex % (sheetSize / frameSize) * frameSize;
            var y = frameIndex / (sheetSize / frameSize) * frameSize;

            if (!mode)
                spriteBatch.Draw(auraTexture, position - Main.screenPosition + (player.Center - player.position), new Rectangle(x, y, frameSize, frameSize), colors.GetColor(progress), 0,
                new Vector2(128, 128) * 0.5f, 0.04175F, SpriteEffects.None, 0f);
        }

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, samplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
    }

    private static void DrawAura(SpriteBatch spriteBatch, Player player, SamplerState samplerState, bool mode) {

        // expects active spritebatch
        // spritebatch leaves as active Additive

        var tick = player.GetModPlayer<KPlayer>().tick;

        var tickRate = 4.25f;

        if (DBTCompat.IsCharging(player))
            tickRate /= 1.5f;

        var progress = tick % tickRate / tickRate;

        var auraTextures = new[] {
                ModContent.Request<Texture2D>("KaiokenMod/Aura/Main/f3", AssetRequestMode.ImmediateLoad).Value,
                ModContent.Request<Texture2D>("KaiokenMod/Aura/Main/f2", AssetRequestMode.ImmediateLoad).Value,
                ModContent.Request<Texture2D>("KaiokenMod/Aura/Main/f1", AssetRequestMode.ImmediateLoad).Value,
                ModContent.Request<Texture2D>("KaiokenMod/Aura/Main/f0", AssetRequestMode.ImmediateLoad).Value
            };

        var auraOverlayTextures = new[] {
                ModContent.Request<Texture2D>("KaiokenMod/Aura/Main/fo3", AssetRequestMode.ImmediateLoad).Value,
                ModContent.Request<Texture2D>("KaiokenMod/Aura/Main/fo2", AssetRequestMode.ImmediateLoad).Value,
                ModContent.Request<Texture2D>("KaiokenMod/Aura/Main/fo1", AssetRequestMode.ImmediateLoad).Value,
                ModContent.Request<Texture2D>("KaiokenMod/Aura/Main/fo0", AssetRequestMode.ImmediateLoad).Value
            };


        var rotation = DBTCompat.GetRotation(player);
        var index = (int)(tick / tickRate) % auraTextures.Length;

        var curTexture = auraTextures[index % auraTextures.Length];
        var nextTexture = auraTextures[(index + 1) % auraTextures.Length];

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, samplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

        var drawColor = KaiokenConfig.Instance.AuraColor with { A = 255 };

        drawColor.R = (byte)Math.Min(drawColor.R + 127, 255);
        drawColor.G = (byte)Math.Min(drawColor.G + 127, 255);
        drawColor.B = (byte)Math.Min(drawColor.B + 127, 255);


        var form = 0;

        try {
            form = (int)DBTCompat.GetCurrentTransformation(player);
        } catch {
            // ignored
        }

        var scale = 0.25f;
        var position = player.Center + new Vector2(0f, (int)(0.0 - (63.5 - player.height * 0.6f)) + 37);
        var origin = new Vector2(300, 420);

        if (form != 0) {
            scale *= 1.75f;
            origin.Y = 460;
        }

        scale *= 1 - (player.GetModPlayer<KPlayer>().JustFormed / 10f).EaseInOut();

        if (mode && KaiokenConfig.Instance.InnerAuraVisible)
            spriteBatch.Draw(nextTexture.InterpolateImages(curTexture, progress.Truncate(2), $"UnderlayTexture_{index}"), position - Main.screenPosition, new Rectangle(0, 0, 600, 600),
                form != 0 ? new Color(1, 0.75f, 0.75f) : Color.White, rotation, origin, scale, 0, 0f);

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, samplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

        curTexture = auraOverlayTextures[index % auraOverlayTextures.Length];
        nextTexture = auraOverlayTextures[(index + 1) % auraOverlayTextures.Length];

        if (mode)
            spriteBatch.Draw(nextTexture.InterpolateImages(curTexture, progress.Truncate(2), $"OverlayTexture_{index}"), position - Main.screenPosition, new Rectangle(0, 0, 600, 600),
            KaiokenConfig.Instance.AuraColor, rotation, origin, scale, 0, 0f);
    }
}