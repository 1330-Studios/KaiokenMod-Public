using KaiokenMod.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace KaiokenMod.UI.Components {
    public class DraggableUIImage : UIImage {
        // Stores the offset from the top left of the UIPanel while dragging.
        private Vector2 offset;
        public bool dragging;

        public DraggableUIImage(Asset<Texture2D> texture) : base(texture) { }


        public override void MouseDown(UIMouseEvent evt) {
            if (KaiokenConfig.Instance.LockStrainUI ||
                Main.LocalPlayer.GetModPlayer<KPlayer>().Data.Strain == 0) return;
            base.MouseDown(evt);
            DragStart(evt);
        }

        public override void MouseUp(UIMouseEvent evt) {
            if (KaiokenConfig.Instance.LockStrainUI ||
                Main.LocalPlayer.GetModPlayer<KPlayer>().Data.Strain == 0) return;
            base.MouseUp(evt);
            DragEnd(evt);
        }

        public void DragStart(UIMouseEvent evt) {
            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            dragging = true;
        }

        public void DragEnd(UIMouseEvent evt) {
            var end = evt.MousePosition;
            dragging = false;

            Left.Set(end.X - offset.X, 0f);
            Top.Set(end.Y - offset.Y, 0f);

            Recalculate();

        }
        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);

            Vector2 mousePos = new(Main.mouseX, Main.mouseY);

            if (ContainsPoint(mousePos)) {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (!dragging) return;
            Left.Set(mousePos.X - offset.X, 0f);
            Top.Set(mousePos.Y - offset.Y, 0f);
            Recalculate();
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (ContainsPoint(Main.MouseScreen)) {
                Main.LocalPlayer.mouseInterface = true;
            }
        }
    }
}
