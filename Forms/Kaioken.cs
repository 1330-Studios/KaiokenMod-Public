using System;
using System.Text;
using System.Threading.Tasks;

using KaiokenMod.Buffs;
using KaiokenMod.FormLoader;
using KaiokenMod.Utils;

using Terraria;

namespace KaiokenMod.Forms;

internal class Kaioken : KaiForm {
    internal override string DisplayName => "Kaio-ken";
    internal override int BuffType { get; set; }
    private static string BuffTip = string.Empty;

    #region Stat Arrays

    private static float[] Defenses = { 10, 11, 12.5f, 13.75f, 15 };
    private static float[] DamageReductions = { .01f, .02f, .03f, .04f, .05f };
    private static float[] Damages = { .2f, .4f, .6f, .8f, 1 };
    private static float[] HealthDrains = { .03f, .0275f, .02f, .0175f, .01f };
    private static float[] Speeds = { .15f, .25f, .35f, .45f, .59f };
    private static float[] Strains = { 2, 4, 6, 8, 10 };

    public static async void Rebuild() {
        while (KaiokenServerConfig.Instance == null)
            await Task.Delay(100);

        var serverConfig = KaiokenServerConfig.Instance;

        Defenses = new[] { serverConfig.DefenseStat_0, serverConfig.DefenseStat_25, serverConfig.DefenseStat_50, serverConfig.DefenseStat_75, serverConfig.DefenseStat_100 };
        DamageReductions = new[] { serverConfig.DRStat_0, serverConfig.DRStat_25, serverConfig.DRStat_50, serverConfig.DRStat_75, serverConfig.DRStat_100 };
        Damages = new[] { serverConfig.StrengthStat_0, serverConfig.StrengthStat_25, serverConfig.StrengthStat_50, serverConfig.StrengthStat_75, serverConfig.StrengthStat_100 };
        HealthDrains = new[] { serverConfig.HealthDrainStat_0, serverConfig.HealthDrainStat_25, serverConfig.HealthDrainStat_50, serverConfig.HealthDrainStat_75, serverConfig.HealthDrainStat_100 };
        Speeds = new[] { serverConfig.SpeedStat_0, serverConfig.SpeedStat_25, serverConfig.SpeedStat_50, serverConfig.SpeedStat_75, serverConfig.SpeedStat_100 };
        Strains = new[] { serverConfig.StrainStat_0, serverConfig.StrainStat_25, serverConfig.StrainStat_50, serverConfig.StrainStat_75, serverConfig.StrainStat_100 };
    }

    #endregion Stat Arrays

    #region delegate* implementations

    private static int GetMasteryIndex(float Mastery) {
        return (int)Math.Clamp(MathF.Floor(Mastery * 4f), 0, 4);
    }
    private static string GetBuffTip_Impl(KPlayer Player) {
        if (!BuffTipDirty && !string.IsNullOrEmpty(BuffTip)) return BuffTip;
        BuffTipDirty = false;
        StringBuilder sb = new();

        sb.Append("+ ").Append($"{GetDamage_Impl(Player.Data.Mastery):P0}").AppendLine(" Damage");
        sb.Append("+ ").Append($"{GetSpeed_Impl(Player.Data.Mastery, Player):P0}").AppendLine(" Speed");
        sb.Append("+ ").Append((int)GetDefense_Impl(Player.Data.Mastery, Player)).AppendLine(" Defense");
        sb.Append("+ ").Append($"{GetDamageReduction_Impl(Player.Data.Mastery, Player):P0}").AppendLine(" Damage Reduction");
        sb.Append("- ").Append(
            $"{GetHealthDrain_Impl(Player.Data.Mastery) * (Player.Player.statLifeMax + Player.Player.statLifeMax2)
                                                        * (1f + Player.Data.GetStrainPercent(Player) * 2f):N0}").AppendLine(" Health/s");
        if (Player.Essence != 0)
            sb.AppendLine("+ Extra benefits, see accessories equipped.");

        sb.Append("Kaioken Mastery: ")
            .Append($"{Player.Data.Mastery:P2} (+ {KaiokenBuff.GetMasteryGain(Main.LocalPlayer):P3} per Second)").AppendLine();

        BuffTip = sb.ToString();

        return BuffTip;
    }

    private static float GetDamage_Impl(float Mastery) {
        var baseDamage = Damages[GetMasteryIndex(Mastery)];

        return baseDamage;
    }

    private static float GetDefense_Impl(float Mastery, KPlayer kPlayer) {
        var baseDefense = Defenses[GetMasteryIndex(Mastery)];

        return baseDefense + (((BitsShort)kPlayer.Essence)[15] ? 10 : ((BitsShort)kPlayer.Essence)[3] ? 5 : 0);
    }

    private static float GetDamageReduction_Impl(float Mastery, KPlayer kPlayer) {
        var baseDamageReduction = DamageReductions[GetMasteryIndex(Mastery)];

        return baseDamageReduction + (((BitsShort)kPlayer.Essence)[15] ? 0.075f : ((BitsShort)kPlayer.Essence)[2] ? 0.05f : 0);
    }

    private static float GetHealthDrain_Impl(float Mastery) {
        var baseHealthDrain = HealthDrains[GetMasteryIndex(Mastery)];

        return baseHealthDrain;
    }

    private static float GetSpeed_Impl(float Mastery, KPlayer kPlayer) {
        var baseSpeed = Speeds[GetMasteryIndex(Mastery)];

        return baseSpeed + (((BitsShort)kPlayer.Essence)[15] ? .15f : ((BitsShort)kPlayer.Essence)[14] ? .1f : 0);
    }

    private static float GetStrainLoss_Impl(float Mastery) {
        var baseStrain = Strains[GetMasteryIndex(Mastery)];

        return baseStrain;
    }

    #endregion delegate* implementations

    public override void Load() {
        unsafe {
            GetDefense = &GetDefense_Impl;
            GetDamageReduction = &GetDamageReduction_Impl;
            GetDamage = &GetDamage_Impl;
            GetHealthDrain = &GetHealthDrain_Impl;
            GetSpeed = &GetSpeed_Impl;
            GetStrainLoss = &GetStrainLoss_Impl;
            GetBuffTip = &GetBuffTip_Impl;
        }
    }

    public override void Unload() {
        unsafe {
            GetStrainLoss = GetHealthDrain = null;
            GetDamageReduction = GetSpeed = GetDefense = null;
            GetBuffTip = null;
        }
    }
}