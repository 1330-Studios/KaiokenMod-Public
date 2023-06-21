using System;

namespace KaiokenMod.FormLoader;

internal abstract unsafe class KaiForm {
    internal static readonly Random random = new();

    internal virtual string DisplayName => "";
    internal virtual int BuffType { get; set; } = -1; // -1 if no buff associated

    internal static bool BuffTipDirty { get; set; }

    #region BuffTip

    internal delegate*<KPlayer, string> GetBuffTip;

    internal static void MarkBuffTipDirty() => BuffTipDirty = true;

    #endregion BuffTip

    #region Form Stats

    internal delegate*<float, KPlayer, float> GetDefense;
    internal delegate*<float, KPlayer, float> GetDamageReduction;
    internal delegate*<float, float> GetDamage;
    internal delegate*<float, float> GetHealthDrain;
    internal delegate*<float, KPlayer, float> GetSpeed;
    internal delegate*<float, float> GetStrainLoss;

    #endregion Form Stats

    #region Asset Registration and Deregistration

    public abstract void Load();

    public abstract void Unload();

    #endregion Asset Registration and Deregistration
}