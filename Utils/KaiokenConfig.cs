using System.ComponentModel;

using Microsoft.Xna.Framework;

using Terraria.ModLoader.Config;
// ReSharper disable UnusedMember.Global

namespace KaiokenMod.Utils;

[Label("Client Settings")]
public class KaiokenConfig : ModConfig {
    public static KaiokenConfig Instance;

    [Header("Strain UI")]
    [Label("Strain UI Offset X")]
    [SliderColor(225, 25, 25, 128)]
    [Range(0f, 100f)]
    [DefaultValue(53.60f)]
    [Tooltip("The X offset for the Strain UI overlay")]
    public float StrainUIOffsetX { get; set; }

    [Label("Strain UI Offset Y")]
    [SliderColor(225, 25, 25, 128)]
    [Range(0f, 100f)]
    [DefaultValue(44.25f)]
    [Tooltip("The Y offset for the Strain UI overlay")]
    public float StrainUIOffsetY { get; set; }

    [Label("Lock Strain UI offset")]
    [DefaultValue(false)]
    public bool LockStrainUI { get; set; }

    [Label("Strain UI Type")]
    [DefaultValue(StrainUIType.Wheel)]
    [SliderColor(200, 0, 0)]
    public StrainUIType StrainType { get; set; }

    [Label("Strain UI Style")]
    [DefaultValue(StrainUIStyle.Cyan)]
    [SliderColor(200, 0, 0)]
    public StrainUIStyle StrainColor { get; set; }

    public enum StrainUIType {
        Wheel,
        Bar
    }

    public enum StrainUIStyle {
        Cyan,
        White,
        Red,
        Gray,
        Purple
    }

    [Header("Sound")]
    [Label("Play Transformation Noises")]
    [DefaultValue(true)]
    public bool PlayTransformationNoises { get; set; }

    [Label("Play Transformation Effect Sounds")]
    [DefaultValue(true)]
    public bool PlayTransformationEffectSounds { get; set; }

    [Header("Aura Options")]
    [Label("Aura Instead of Aura Particles")]
    [DefaultValue(true)]
    public bool AuraInsteadOfParticles { get; set; }

    [Label("Inner Aura Visible")]
    [DefaultValue(true)]
    public bool InnerAuraVisible { get; set; }

    [Header("Aura Particles")]
    [Label("v2.0.X Visuals")]
    [DefaultValue(false)]
    public bool V20XAuraParticles { get; set; }

    [Label("Spawn Delay")]
    [DefaultValue(3)]
    public int AuraParticleDelay { get; set; }

    [Label("Spawn Count")]
    [DefaultValue(1)]
    public int AuraParticleCount { get; set; }

    [Label("Horizontal Diameter")]
    [DefaultValue(6)]
    public float AuraParticleHorizontalDiameter { get; set; }

    [Label("Vertical Diameter")]
    [DefaultValue(8)]
    public float AuraParticleVerticalDiameter { get; set; }

    [Label("Y Velocity")]
    [DefaultValue(-0.0083333333333333f)]
    [Range(-0.05f, 0.05f)]
    public float AuraParticleYVelocity { get; set; }

    [Header("Colors")]
    [Label("Kaio-ken Overlay Color")]
    [DefaultValue("153, 0, 0, 152")]
    public Color OverlayColor { get; set; }

    [Label("Kaio-ken Aura Color")]
    [DefaultValue("255, 0, 0, 128")]
    public Color AuraColor { get; set; }

    public override ConfigScope Mode => ConfigScope.ClientSide;

    public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message) {
        return true;
    }

    public static void Unload() {
        Instance = null;
    }
}