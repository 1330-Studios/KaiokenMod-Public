using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.GameContent.UI.Elements;

namespace KaiokenMod.UI.Components {
    public class CountdownElement : UIImage {
        private readonly int Frames;
        private readonly Asset<Texture2D> Texture;
        public CountdownElement(Asset<Texture2D> texture, int frames) : base(texture) {
            Frames = frames;
            Texture = texture;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch) {
            if (StrainUIState.CountdownState <= -1) return;
            var dimensions = GetDimensions();
            var height = (int)(Height.Pixels / Frames);
            var rectangle = new Rectangle(0, height * StrainUIState.CountdownState, (int)Width.Pixels, height);
            var position = new Vector2(dimensions.X, dimensions.Y);

            spriteBatch.Draw(Texture.Value, position + new Vector2(Width.Pixels * .5f, 32), rectangle, Color.White, 0f, Texture.Size() * 0.5f, Main.UIScale / 2, 0, 0f);
        }
    }
}
