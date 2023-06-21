using System;

using KaiokenMod.FormLoader;
using KaiokenMod.Rarity;
using KaiokenMod.UI.Components;
using KaiokenMod.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace KaiokenMod.UI.Strain;
internal class StrainWUIState : UIState {
    internal static int CountdownState = -1;

    #region Assets
    internal static Asset<Texture2D>[] wheelTexture;

    internal static Asset<Texture2D> innerTexture;
    internal static Asset<Texture2D> glowTexture;
    internal static Asset<Texture2D> iconsTexture;
    #endregion

    internal static ulong tick;

    public override void OnInitialize() {
        var count = Enum.GetValues(typeof(KaiokenConfig.StrainUIStyle)).Length;

        wheelTexture = new Asset<Texture2D>[count];

        for (var i = 0; i < count; i++)
            wheelTexture[i] = ModContent.Request<Texture2D>($"KaiokenMod/UI/Strain/Wheel{i}", AssetRequestMode.ImmediateLoad);

        innerTexture = ModContent.Request<Texture2D>("KaiokenMod/UI/Strain/WheelInner", AssetRequestMode.ImmediateLoad);
        glowTexture = ModContent.Request<Texture2D>("KaiokenMod/UI/Strain/WheelGlow", AssetRequestMode.ImmediateLoad);
        iconsTexture = ModContent.Request<Texture2D>("KaiokenMod/UI/Strain/WheelIcons", AssetRequestMode.ImmediateLoad);

        var wheel = new StrainWheel();

        Append(wheel);
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);

        var player = Main.CurrentPlayer.GetModPlayer<KPlayer>();

        CountdownState = player.AtMaxStrain() ? CountdownState = 3 - player.TimeAtMaxStrain : -1;

        tick++;
    }

    internal class StrainWheel : DraggableUIImage {
        public StrainWheel() : base(wheelTexture[0]) { }

        public override void OnInitialize() {
            base.OnInitialize();

            Vector2 screenRatioPosition = new(KaiokenConfig.Instance.StrainUIOffsetX, KaiokenConfig.Instance.StrainUIOffsetY);

            if (screenRatioPosition.X is < 0f or > 100f) {
                screenRatioPosition.X = 53.60f;
            }
            switch (screenRatioPosition.Y) {
                case < 0f:
                case > 100f:
                    screenRatioPosition.Y = 44.25f;
                    break;
            }

            var screenPos = screenRatioPosition.ToScreenPos();

            Left.Set(screenPos.X, 0f);
            Top.Set(screenPos.Y, 0f);
        }

        public override void MouseDown(UIMouseEvent evt) {
            if (Main.LocalPlayer.GetModPlayer<KPlayer>().Data.Strain == 0)
                return;

            base.MouseDown(evt);
        }

        public override void MouseUp(UIMouseEvent evt) {
            if (Main.LocalPlayer.GetModPlayer<KPlayer>().Data.Strain == 0)
                return;

            base.MouseUp(evt);

            var dimensions = GetDimensions();

            var Relative = dimensions.Position().FromScreenPos();

            if (!(Math.Abs(KaiokenConfig.Instance.StrainUIOffsetX - Relative.X) > 0.01f) &&
                !(Math.Abs(KaiokenConfig.Instance.StrainUIOffsetY - Relative.Y) > 0.01f)) return;
            KaiokenConfig.Instance.StrainUIOffsetX = Relative.X.Truncate(2);
            KaiokenConfig.Instance.StrainUIOffsetY = Relative.Y.Truncate(2);
            KaiokenMod.SaveConfig(KaiokenConfig.Instance);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (Main.LocalPlayer.GetModPlayer<KPlayer>().Data.Strain == 0)
                return;

            var player = Main.CurrentPlayer.GetModPlayer<KPlayer>();
            if (player.DampStrain.AverageDoubles() > 0 && player.Player.statLife > 0)
                base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch) {
            if (Main.LocalPlayer.GetModPlayer<KPlayer>().Data.Strain == 0)
                return;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);

            DrawGlow(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);

            DrawBars(spriteBatch);
            spriteBatch.Draw(wheelTexture[(int)KaiokenConfig.Instance.StrainColor].Value, GetDimensions().ToRectangle(), new Color(255, 255, 255));
            DrawIcons(spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            if (Main.LocalPlayer.GetModPlayer<KPlayer>().Data.Strain == 0)
                return;

            var player = Main.CurrentPlayer.GetModPlayer<KPlayer>();

            base.Update(gameTime);

            var avg = player.DampStrain.AverageDoubles();

            Rectangle mouseHitbox = new((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 8, 8);
            if (GetDimensions().ToRectangle().Intersects(mouseHitbox) && avg > 0)
                Main.instance.MouseText($"Strain: {avg:N0}/{player.Data.GetMaxStrain(player):N0} ({avg / player.Data.GetMaxStrain(player) /* Can't be converted to player.Data.StrainPercent due to slow updates. */:P1})",
                    ModContent.RarityType<KaiokenRarity>());
        }

        private const int frameSize = 64;

        private static KaiForm Kaioken => FormRegister.KaiFormInstances["Kaioken"];

        private void DrawGlow(SpriteBatch spriteBatch) {
            const int sheetSize = 128;
            const int numberOfFrames = sheetSize / frameSize * (sheetSize / frameSize);

            var frameIndex = (int)(tick / 5f % 4 * 0.25 * numberOfFrames);

            var x = frameIndex % (sheetSize / frameSize) * frameSize;
            var y = frameIndex / (sheetSize / frameSize) * frameSize;

            var color = new Color(255, 255, 255, 48);

            if (Main.CurrentPlayer.HasBuff(Kaioken.BuffType))
                color = KaiokenConfig.Instance.AuraColor with { A = 255 };

            spriteBatch.Draw(glowTexture.Value, GetDimensions().ToRectangle(), new Rectangle(x, y, frameSize, frameSize), color);
        }

        private void DrawBars(SpriteBatch spriteBatch) {
            var player = Main.CurrentPlayer.GetModPlayer<KPlayer>();
            var bCount = (int)Math.Floor(Math.Clamp(player.Data.GetStrainPercent(player) * 10, 0, 10));

            var color = new Color(200, 200, 200);

            if (Main.CurrentPlayer.HasBuff(Kaioken.BuffType))
                color = KaiokenConfig.Instance.OverlayColor with { A = 255 };

            for (var i = 0; i < 10; i++)
                spriteBatch.Draw(innerTexture.Value, GetDimensions().ToRectangle(), new Rectangle(0, i * frameSize, frameSize, frameSize), i <= bCount ? color : new Color(128, 128, 128));

        }

        private void DrawIcons(SpriteBatch spriteBatch) {
            var idx = 0;

            if (Main.CurrentPlayer.HasBuff(Kaioken.BuffType))
                idx = 1;

            spriteBatch.Draw(iconsTexture.Value, GetDimensions().ToRectangle(), new Rectangle(64 * idx, 0, frameSize, frameSize), idx != 0 ? KaiokenConfig.Instance.OverlayColor with { A = 255 } : Color.White);
        }
    }
}