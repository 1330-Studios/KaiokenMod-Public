using System.Collections.Generic;

using KaiokenMod.UI.Strain;
using KaiokenMod.Utils;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace KaiokenMod.UI;

// ReSharper disable once UnusedMember.Global
internal class UIManager : ModSystem {
    private static readonly StrainWUIState StrainWUIState = new();
    public static readonly UserInterface StrainWInterface = new();

    private static readonly StrainUIState StrainUIState = new();
    public static readonly UserInterface StrainInterface = new();

    public override void Load() {
        StrainWUIState.Activate();
        StrainWInterface.SetState(StrainWUIState);

        StrainUIState.Activate();
        StrainInterface.SetState(StrainUIState);

    }
    public override void OnModUnload() {
        StrainWUIState.Deactivate();
        StrainWUIState.Remove();

        StrainUIState.Deactivate();
        StrainUIState.Remove();
    }
    public override void UpdateUI(GameTime gameTime) {
        StrainWInterface?.Update(gameTime);
        StrainInterface?.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        var mouseIndex = layers.FindIndex(layer => layer.Name.Contains("Resource Bars"));
        if (mouseIndex != -1) {
            layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("Kaio-Ken: Strain Bar",
                    () => {
                        if (Main.LocalPlayer.GetModPlayer<KPlayer>().Data.Strain == 0) return true;
                        switch (KaiokenConfig.Instance.StrainType) {
                            default:
                            case KaiokenConfig.StrainUIType.Wheel:
                                StrainWInterface?.Draw(Main.spriteBatch, new GameTime());
                                break;
                            case KaiokenConfig.StrainUIType.Bar:
                                StrainInterface?.Draw(Main.spriteBatch, new GameTime());
                                break;
                        }
                        return true;
                    }, InterfaceScaleType.UI));
        }
    }

    public override void Unload() {
    }
}