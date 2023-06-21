using System;
using System.Runtime.InteropServices;

using KaiokenMod.Items.Armor;

using Terraria;
using Terraria.ID;

namespace KaiokenMod;

[StructLayout(LayoutKind.Sequential)]
public struct KPlayer_Data {
    private float _mastery = float.Epsilon;
    private double _strain = double.Epsilon;
    private bool _formed = false;

    public KPlayer_Data() {}

    public float Mastery {
        readonly get => _mastery;
        internal set => _mastery = value;
    }

    public double Strain {
        readonly get => _strain;
        internal set => _strain = value;
    }

    public bool Formed {
        readonly get => _formed;
        internal set => _formed = value;
    }

#pragma warning disable IDE0251
    public double GetStrainPercent(KPlayer player) => GetStrainPercent(player.Player);

    public void SetMastery(float mastery) {
        Mastery = mastery;
    }

    public void SetStrain(double strain, KPlayer player) {
        SetStrain(strain, player.Player);
    }

    internal readonly double GetMaxStrain(KPlayer Player) {
        return GetMaxStrain(Player.Player);
    }

    public double GetStrainPercent(Player player) => Strain / GetMaxStrain(player);

    public void SetStrain(double strain, Player player) {
        Strain = Math.Min(strain, GetMaxStrain(player));
    }

    internal readonly double GetMaxStrain(Player Player) {
        var result = Math.Min(400f, 200 + Math.Round(Mastery * 200f / 10f) * 10f);
        try {
            if (Player?.armor[0]?.type == ItemID.None && Player?.armor[1]?.ModItem is TLOWGi &&
                Player?.armor[2]?.ModItem is TLOWPants)
                result *= 1.59;
        } catch {
            // ignored
        }

        return result;
    }
}