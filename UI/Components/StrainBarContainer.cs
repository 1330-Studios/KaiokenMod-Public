using System;

using KaiokenMod.Rarity;
using KaiokenMod.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace KaiokenMod.UI.Components {
    internal class StrainBarContainer : DraggableUIImage {
        public StrainBarContainer(Asset<Texture2D> texture) : base(texture) {
        }

        public override void OnInitialize() {
            base.OnInitialize();

            Vector2 screenRatioPosition = new(KaiokenConfig.Instance.StrainUIOffsetX, KaiokenConfig.Instance.StrainUIOffsetY);

            if (screenRatioPosition.X is < 0f or > 100f) {
                screenRatioPosition.X = 50.104603f;
            }
            if (screenRatioPosition.Y is < 0f or > 100f) {
                screenRatioPosition.Y = 54.112984f;
            }

            var screenPos = screenRatioPosition;
            screenPos.X = (int)(screenPos.X * 0.01f * Main.screenWidth);
            screenPos.Y = (int)(screenPos.Y * 0.01f * Main.screenHeight);

            Left.Set(screenPos.X, 0f);
            Top.Set(screenPos.Y, 0f);
        }

        public override void MouseUp(UIMouseEvent evt) {
            base.MouseUp(evt);

            var dimensions = GetDimensions();

            Vector2 Relative = new(dimensions.X / Main.screenWidth * 100f, dimensions.Y / Main.screenHeight * 100f);

            if (!(Math.Abs(KaiokenConfig.Instance.StrainUIOffsetX - Relative.X) > 0.01f) &&
                !(Math.Abs(KaiokenConfig.Instance.StrainUIOffsetY - Relative.Y) > 0.01f)) return;
            KaiokenConfig.Instance.StrainUIOffsetX = Relative.X;
            KaiokenConfig.Instance.StrainUIOffsetY = Relative.Y;
            KaiokenMod.SaveConfig(KaiokenConfig.Instance);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            var player = Main.CurrentPlayer.GetModPlayer<KPlayer>();
            if (player.DampStrain.AverageDoubles() > 0)
                base.Draw(spriteBatch);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            spriteBatch.Draw(StrainUIState.containerTexture[(int)KaiokenConfig.Instance.StrainColor].Value, GetDimensions().ToRectangle(), new Color(255, 255, 255, StrainUIState.Transparency));
        }

        public override void Update(GameTime gameTime) {
            var player = Main.CurrentPlayer.GetModPlayer<KPlayer>();

            base.Update(gameTime);

            Rectangle mouseHitbox = new((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 8, 8);
            if (GetDimensions().ToRectangle().Intersects(mouseHitbox) && player.DampStrain.AverageDoubles() > 0)
                Main.instance.MouseText($"Strain: {player.DampStrain.AverageDoubles():N0}/{player.Data.GetMaxStrain(player):N0} ({player.DampStrain.AverageDoubles() / player.Data.GetMaxStrain(player):P1})", ModContent.RarityType<KaiokenRarity>());
        }
    }
}
