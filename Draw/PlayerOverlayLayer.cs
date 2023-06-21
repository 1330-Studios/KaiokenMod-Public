using System.Collections.Generic;

using KaiokenMod.Buffs;
using KaiokenMod.Utils;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace KaiokenMod.Draw;

// ReSharper disable once UnusedMember.Global
internal class PlayerOverlayLayer : PlayerDrawLayer {

    public override bool IsHeadLayer => false;

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.FrontAccFront);

    protected override void Draw(ref PlayerDrawSet drawInfo) {
        if (Main.netMode == NetmodeID.Server)
            return;
        if (drawInfo.shadow != 0)
            return;
        var plr = drawInfo.drawPlayer;

        if (!plr.HasBuff<KaiokenBuff>()) return;

        var o = new List<DrawData>(drawInfo.DrawDataCache);

        for (var i = 0; i < drawInfo.DrawDataCache.Count; i++) {
            var data = o[i];

            if (data.color != Color.Transparent) {
                data.color = KaiokenConfig.Instance.OverlayColor;
            }

            o[i] = data;
        }

        var newCache = new List<DrawData>();

        for (var i = 0; i < drawInfo.DrawDataCache.Count; i++) {
            newCache.Add(drawInfo.DrawDataCache[i]);
            newCache.Add(o[i]);
        }

        drawInfo.DrawDataCache = newCache;
    }
}