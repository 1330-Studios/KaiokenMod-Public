using System;

using KaiokenMod.UI.Components;
using KaiokenMod.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace KaiokenMod.UI {
    public class StrainUIState : UIState {
        #region Elements
        private StrainBarContainer BarFrame;
        private CountdownElement Countdown;
        private StrainBarElement StrainBar;
        #endregion

        #region Statics
        internal static int CountdownState = -1;
        internal static int Transparency;
        #endregion

        #region Assets
        internal static Asset<Texture2D>[] containerTexture;

        internal static Asset<Texture2D> barTexture;
        internal static Asset<Texture2D> numberTexture;
        #endregion

        public override void OnInitialize() {
            var count = Enum.GetValues(typeof(KaiokenConfig.StrainUIStyle)).Length;

            containerTexture = new Asset<Texture2D>[count];

            for (var i = 0; i < count; i++)
                containerTexture[i] = ModContent.Request<Texture2D>($"KaiokenMod/UI/StrainContainer{i}", AssetRequestMode.ImmediateLoad);

            barTexture = ModContent.Request<Texture2D>("KaiokenMod/UI/StrainBar", AssetRequestMode.ImmediateLoad);
            numberTexture = ModContent.Request<Texture2D>("KaiokenMod/UI/Numbers", AssetRequestMode.ImmediateLoad);

            BarFrame = new StrainBarContainer(containerTexture[0]);
            Countdown = new CountdownElement(numberTexture, 4);
            StrainBar = new StrainBarElement(barTexture);

            StrainBar.Left.Set(10, 0f);
            StrainBar.Top.Set(1, 0f);
            BarFrame.Append(StrainBar);

            Countdown.Left.Set(27, 0f);
            Countdown.Top.Set(19, 0f);
            BarFrame.Append(Countdown);

            Append(BarFrame);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            KPlayer player = Main.CurrentPlayer.GetModPlayer<KPlayer>();

            var averageStrain = player.DampStrain.AverageDoubles();

            double quotient = averageStrain / player.Data.GetMaxStrain(player);

            if (quotient > 0.25) {
                Transparency = 255;
            } else if (averageStrain > 0f) {
                Transparency = Terraria.Utils.Clamp<byte>((byte)(255 * quotient * 4.0), 0, 255); // Alpha reaches full opacity at 25% strain
            } else
                Transparency = 0;

            if (player.AtMaxStrain())
                CountdownState = 3 - player.TimeAtMaxStrain;
            else
                CountdownState = -1;
        }
    }
}
