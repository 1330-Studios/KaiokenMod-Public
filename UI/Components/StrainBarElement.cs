using KaiokenMod.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.GameContent.UI.Elements;

namespace KaiokenMod.UI.Components {
    public class StrainBarElement : UIImage {
        public readonly Asset<Texture2D> Texture;

        public StrainBarElement(Asset<Texture2D> texture) : base(texture) {
            Texture = texture;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch) {
            var player = Main.CurrentPlayer.GetModPlayer<KPlayer>();
            if (!(player.DampStrain.AverageDoubles() > 0)) return;
            var rectangle = GetDimensions().ToRectangle();
            var quotient = Terraria.Utils.Clamp(player.DampStrain.AverageDoubles() / player.Data.GetMaxStrain(player), 0f, 1f);

            spriteBatch.Draw(Texture.Value, GetDimensions().Position(), new Rectangle(0, 0, (int)(rectangle.Width * quotient), rectangle.Height), KaiokenConfig.Instance.OverlayColor with { A = (byte)StrainUIState.Transparency });
        }
    }
}
