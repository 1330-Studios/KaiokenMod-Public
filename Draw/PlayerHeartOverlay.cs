using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Draw;
internal class PlayerHeartOverlay : PlayerDrawLayer {
    public static Dictionary<int, float> cycles = new();

    public override bool IsHeadLayer => false;
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Torso);

    protected override void Draw(ref PlayerDrawSet drawInfo) {
        if (Main.netMode == NetmodeID.Server || drawInfo.shadow != 0 || drawInfo.drawPlayer.statLife <= 0) {
            cycles[drawInfo.drawPlayer.whoAmI] = 0;
            return;
        }

        var player = drawInfo.drawPlayer;
        var kPlayer = player.GetModPlayer<KPlayer>();

        if (kPlayer.Data.GetStrainPercent(kPlayer) < 0.75)
            return;

        var alpha = (byte)((kPlayer.Data.GetStrainPercent(kPlayer) - 0.75) * 4 * 255);

        cycles.TryAdd(player.whoAmI, 0);

        var frame = (int)Math.Max(0, cycles[player.whoAmI] - 2);
        var texture = ModContent.Request<Texture2D>($"KaiokenMod/Overlays/{(kPlayer.TimeAtMaxStrainF >= 2.75f ? "Dead" : "Healthy")}_Heart", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        drawInfo.DrawDataCache.Add(new DrawData(texture, player.Center - Main.screenPosition,
            new Rectangle(0, kPlayer.TimeAtMaxStrain >= 2.75f ? 0 : frame * 56, 40, 56), Color.White with { A = alpha }, player.headRotation, new Vector2(20, 28), new Vector2(1, 1),
            player.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0));

        cycles[player.whoAmI] += 0.1f + (float)kPlayer.Data.GetStrainPercent(kPlayer) * 0.2f + kPlayer.TimeAtMaxStrain * .25f;
        if (cycles[player.whoAmI] > 5)
            cycles[player.whoAmI] -= 5;
    }
}
