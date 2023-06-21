using System.ComponentModel;

using KaiokenMod.Forms;

using Terraria.ModLoader.Config;

namespace KaiokenMod.Utils;

[Label("Server Settings")]
internal class KaiokenServerConfig : ModConfig {
    public static KaiokenServerConfig Instance;

    [Header("Kaio-ken Balance")]
    [Label("Allow Kaioken Stacking with DBT Kaioken?")]
    [DefaultValue(false)]
    public bool AllowStacking { get; set; }


    [Header("Kaio-ken Defense Stats")]
    [Label("0% Mastery")]
    [Range(0f, 50f)]
    [Increment(0.5f)]
    [DefaultValue(10)]
    public float DefenseStat_0 { get; set; }

    [Label("25% Mastery")]
    [Range(0f, 50f)]
    [Increment(0.5f)]
    [DefaultValue(11)]
    public float DefenseStat_25 { get; set; }

    [Label("50% Mastery")]
    [Range(0f, 50f)]
    [Increment(0.5f)]
    [DefaultValue(13f)]
    public float DefenseStat_50 { get; set; }

    [Label("75% Mastery")]
    [Range(0f, 50f)]
    [Increment(0.5f)]
    [DefaultValue(14f)]
    public float DefenseStat_75 { get; set; }

    [Label("100% Mastery")]
    [Range(0f, 50f)]
    [Increment(0.5f)]
    [DefaultValue(15)]
    public float DefenseStat_100 { get; set; }



    [Header("Kaio-ken Damage Reduction Stats")]
    [Label("0% Mastery")]
    [Range(0f, 1f)]
    [Increment(0.005f)]
    [DefaultValue(0.01f)]
    public float DRStat_0 { get; set; }

    [Label("25% Mastery")]
    [Range(0f, 1f)]
    [Increment(0.005f)]
    [DefaultValue(0.02f)]
    public float DRStat_25 { get; set; }

    [Label("50% Mastery")]
    [Range(0f, 1f)]
    [Increment(0.005f)]
    [DefaultValue(0.03f)]
    public float DRStat_50 { get; set; }

    [Label("75% Mastery")]
    [Range(0f, 1f)]
    [Increment(0.005f)]
    [DefaultValue(0.04f)]
    public float DRStat_75 { get; set; }

    [Label("100% Mastery")]
    [Range(0f, 1f)]
    [Increment(0.005f)]
    [DefaultValue(0.05f)]
    public float DRStat_100 { get; set; }



    [Header("Kaio-ken Strength Stats")]
    [Label("0% Mastery")]
    [Range(0f, 5f)]
    [Increment(0.05f)]
    [DefaultValue(0.2f)]
    public float StrengthStat_0 { get; set; }

    [Label("25% Mastery")]
    [Range(0f, 5f)]
    [Increment(0.05f)]
    [DefaultValue(0.4f)]
    public float StrengthStat_25 { get; set; }

    [Label("50% Mastery")]
    [Range(0f, 5f)]
    [Increment(0.05f)]
    [DefaultValue(0.6f)]
    public float StrengthStat_50 { get; set; }

    [Label("75% Mastery")]
    [Range(0f, 5f)]
    [Increment(0.05f)]
    [DefaultValue(0.8f)]
    public float StrengthStat_75 { get; set; }

    [Label("100% Mastery")]
    [Range(0f, 5f)]
    [Increment(0.05f)]
    [DefaultValue(1f)]
    public float StrengthStat_100 { get; set; }



    [Header("Kaio-ken Health Drain Stats")]
    [Label("0% Mastery")]
    [Range(0f, 1f)]
    [Increment(0.0025f)]
    [DefaultValue(.03f)]
    public float HealthDrainStat_0 { get; set; }

    [Label("25% Mastery")]
    [Range(0f, 1f)]
    [Increment(0.0025f)]
    [DefaultValue(.0275f)]
    public float HealthDrainStat_25 { get; set; }

    [Label("50% Mastery")]
    [Range(0f, 1f)]
    [Increment(0.0025f)]
    [DefaultValue(.02f)]
    public float HealthDrainStat_50 { get; set; }

    [Label("75% Mastery")]
    [Range(0f, 1f)]
    [Increment(0.0025f)]
    [DefaultValue(.0175f)]
    public float HealthDrainStat_75 { get; set; }

    [Label("100% Mastery")]
    [Range(0f, 1f)]
    [Increment(0.0025f)]
    [DefaultValue(.01f)]
    public float HealthDrainStat_100 { get; set; }



    [Header("Kaio-ken Speed Stats")]
    [Label("0% Mastery")]
    [Range(0f, 5f)]
    [Increment(0.01f)]
    [DefaultValue(.15f)]
    public float SpeedStat_0 { get; set; }

    [Label("25% Mastery")]
    [Range(0f, 5f)]
    [Increment(0.01f)]
    [DefaultValue(.25f)]
    public float SpeedStat_25 { get; set; }

    [Label("50% Mastery")]
    [Range(0f, 5f)]
    [Increment(0.01f)]
    [DefaultValue(.35f)]
    public float SpeedStat_50 { get; set; }

    [Label("75% Mastery")]
    [Range(0f, 5f)]
    [Increment(0.01f)]
    [DefaultValue(.45f)]
    public float SpeedStat_75 { get; set; }

    [Label("100% Mastery")]
    [Range(0f, 5f)]
    [Increment(0.01f)]
    [DefaultValue(.59f)]
    public float SpeedStat_100 { get; set; }



    [Header("Kaio-ken Strain Loss Stats")]
    [Label("0% Mastery")]
    [Range(0f, 50f)]
    [Increment(0.1f)]
    [DefaultValue(2)]
    public float StrainStat_0 { get; set; }

    [Label("25% Mastery")]
    [Range(0f, 50f)]
    [Increment(0.1f)]
    [DefaultValue(4)]
    public float StrainStat_25 { get; set; }

    [Label("50% Mastery")]
    [Range(0f, 50f)]
    [Increment(0.1f)]
    [DefaultValue(6)]
    public float StrainStat_50 { get; set; }

    [Label("75% Mastery")]
    [Range(0f, 50f)]
    [Increment(0.1f)]
    [DefaultValue(8)]
    public float StrainStat_75 { get; set; }

    [Label("100% Mastery")]
    [Range(0f, 50f)]
    [Increment(0.1f)]
    [DefaultValue(10)]
    public float StrainStat_100 { get; set; }

    public override ConfigScope Mode => ConfigScope.ServerSide;

    public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message) {
        Kaioken.Rebuild();
        return true;
    }

    public static void Unload() {
        Instance = null;
    }
}
