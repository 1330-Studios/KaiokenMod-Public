using System;
using System.Reflection;

using KaiokenMod.FormLoader;
using KaiokenMod.Items.Essence;
using KaiokenMod.Particle;
using KaiokenMod.Utils;
using KaiokenMod.Utils.DBT;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace KaiokenMod.Buffs;

internal unsafe class KaiokenBuff : ModBuff {
    private readonly record struct BuffReflection(FieldInfo BuffNameCacheFieldInfo, ConstructorInfo LocalizedTextConstructorInfo);

    private KaiForm KaiokenForm;
    private int _tick;
    private BuffReflection _buffReflection;

    public override void SetStaticDefaults() {
        FormRegister.KaiFormInstances["Kaioken"].BuffType = Type;
        KaiokenForm = FormRegister.KaiFormInstances["Kaioken"];

        DisplayName.SetDefault(KaiokenForm.DisplayName);

        Main.buffNoTimeDisplay[Type] = true;
        Main.buffNoSave[Type] = true;
        Main.debuff[Type] = false;
    }

    public override void Unload() {
        KaiokenForm = null;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams) {
        var kPlayer = Main.LocalPlayer.GetModPlayer<KPlayer>();
        var idx = KaiokenEssenceHelper.GetKaiokenMultiplierStatus(kPlayer.Essence);
        drawParams.Texture = ModContent.Request<Texture2D>($"KaiokenMod/Buffs/KaiokenBuff{(idx > 0 ? idx : "")}", AssetRequestMode.ImmediateLoad).Value;
        return base.PreDraw(spriteBatch, buffIndex, ref drawParams);
    }

    private static readonly AuraParticles ap = new();

    public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams) {
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

        var colors = new Gradient(Color.Transparent, (0.25, KaiokenConfig.Instance.AuraColor), (0.75, KaiokenConfig.Instance.AuraColor), (.975, Color.Transparent));

        var auraTexture = ModContent.Request<Texture2D>($"KaiokenMod/Aura/{(KaiokenConfig.Instance.V20XAuraParticles ? "Flames" : "Fire")}", AssetRequestMode.ImmediateLoad).Value;

        const int frameSize = 128;
        const int sheetSize = 512;
        const int numberOfFrames = sheetSize / frameSize * (sheetSize / frameSize);
        
        var currentTimeStamp = DateTime.UtcNow - new DateTime(1970, 1, 1);

        foreach (var (position, timestamp) in ap.Particles) {
            var progress = Math.Clamp((currentTimeStamp.TotalMilliseconds - timestamp) / 900.0, 0f, 1f);

            var frameIndex = (int)(progress * numberOfFrames);

            var x = frameIndex % (sheetSize / frameSize) * frameSize;
            var y = frameIndex / (sheetSize / frameSize) * frameSize;
            
            spriteBatch.Draw(auraTexture, position + (drawParams.Texture.Size() / 2f) + drawParams.Position, new Rectangle(x, y, frameSize, frameSize), colors.GetColor(progress), 0,
                new Vector2(128, 128) * 0.5f, 0.167f, SpriteEffects.None, 0f);
        }
        
        ap.Update();

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
    }

    public override void ModifyBuffTip(ref string tip, ref int rare) {
        var kPlayer = Main.LocalPlayer.GetModPlayer<KPlayer>();

        tip = KaiokenForm.GetBuffTip(kPlayer);

        if (_tick % 10 != 0) return;
        if (_buffReflection == default || _buffReflection.BuffNameCacheFieldInfo == null || _buffReflection.LocalizedTextConstructorInfo == null) {
            _buffReflection = new BuffReflection {
                BuffNameCacheFieldInfo = typeof(Lang).GetField("_buffNameCache", ReflectionHelpers.ALL),
                LocalizedTextConstructorInfo = typeof(LocalizedText).GetConstructor(ReflectionHelpers.ALL, new[] { typeof(string), typeof(string) })
            };
        }

        var newName = KaiokenForm.DisplayName + KaiokenEssenceHelper._multipliers[KaiokenEssenceHelper.GetKaiokenMultiplierStatus(kPlayer.Essence)];
        var _buffNameCache = (LocalizedText[])_buffReflection.BuffNameCacheFieldInfo!.GetValue(null);
        var localizedText = (LocalizedText)_buffReflection.LocalizedTextConstructorInfo!.Invoke(new object[] { new string(_buffNameCache![Type].Key), newName });
        _buffNameCache[Type] = localizedText;
        _buffReflection.BuffNameCacheFieldInfo!.SetValue(null, _buffNameCache);
    }

    public override void Update(Player player, ref int buffIndex) {
        Lighting.AddLight(player.position + new Vector2(0, (player.GetHairDrawOffset(player.hair, false) / 2).Y) + player.velocity / 1.25f,
            new Vector3(KaiokenConfig.Instance.OverlayColor.R, KaiokenConfig.Instance.OverlayColor.G, KaiokenConfig.Instance.OverlayColor.B) / 255f);
        _tick++;

        var kPlayer = player.GetModPlayer<KPlayer>();

        kPlayer.FormedYet = true;

        var speed = KaiokenForm.GetSpeed(kPlayer.Data.Mastery, kPlayer) + 1f;
        var defense = KaiokenForm.GetDefense(kPlayer.Data.Mastery, kPlayer);
        var damage = KaiokenForm.GetDamage(kPlayer.Data.Mastery);
        var healthDrain = KaiokenForm.GetHealthDrain(kPlayer.Data.Mastery) * (player.statLifeMax + player.statLifeMax2);

        player.lifeRegen = 0;

        player.statDefense += (int)MathF.Round(defense);

        player.moveSpeed *= speed;
        player.maxRunSpeed *= speed;
        player.runAcceleration *= speed;

        if (player.jumpSpeedBoost < 1)
            player.jumpSpeedBoost = 1;

        player.jumpSpeedBoost *= speed;

        player.GetDamage(DamageClass.Generic) += damage;

        DBTCompat.KaiokenUpdate(player, KaiokenForm);

        var strainPercent = (float)kPlayer.Data.GetStrainPercent(kPlayer);
        healthDrain *= 1f + strainPercent * 2f;

        if (kPlayer.tick % 15 != 0) return;
        var quarterHealthDrain = (int)healthDrain / 4;

        var healthDrainToUse = quarterHealthDrain;

        if (strainPercent < 0.66)
            healthDrainToUse /= 2;

        if (strainPercent < 0.33)
            healthDrainToUse /= 2;

        if (strainPercent < 0.1)
            healthDrainToUse /= 2;

        if (healthDrainToUse > 0) {
            var rng = Random.Shared.Next(0, 5);

            if (quarterHealthDrain >= rng) {
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), CombatText.DamagedFriendly, healthDrainToUse);

                if ((player.statLife -= healthDrainToUse) <= 0) {
                    player.statLife = 0;
                    player.KillMe(PlayerDeathReason.ByCustomReason(string.Format(listOfReasons[KaiForm.random.Next(listOfReasons.Length)], player.name)), healthDrainToUse, 0);
                }
            }
        }

        kPlayer.Data.SetMastery(Math.Min(kPlayer.Data.Mastery + GetMasteryGain(player), 1));
        KaiForm.MarkBuffTipDirty();
    }

    internal static float GetMasteryGain(Player player) {
        const double baseGain = 0.00003;
        double multiplicative = 1;
        double additive = 0;

        if (player == null) goto results;

        var kPlayer = player.GetModPlayer<KPlayer>();
        var strainPercent = (float)kPlayer.Data.GetStrainPercent(kPlayer);
        additive += strainPercent * 0.00003;

        if (((BitsShort)kPlayer.Essence)[0])
            multiplicative++;

        if (((BitsShort)kPlayer.Essence)[15])
            multiplicative += 9;

        results:

        return (float)(baseGain * multiplicative + additive);
    }

    internal static double GetStrainGain(Player player) {
        const double baseGain = 5;
        const double additive = 0f;
        var multiplicative = 1f;

        var kPlayer = player.GetModPlayer<KPlayer>();
        var essence = (BitsShort)kPlayer.Essence;

        if (essence[0] && !essence[15])
            multiplicative += 0.25f;

        return baseGain * multiplicative + additive;
    }

    public static readonly string[] listOfReasons = {
        "{0} was playing with fire.", "The strain was too much for {0}!", "The burden of the power was too great for {0}.",
        "{0} was burning up.", "{0} used too high of a multiplier.", "Kaioken engulfed {0} in flames.", "{0}'s body is on fire!", "{0} couldn't handle it.",
        "{0}'s heart couldn't handle it.", "{0}'s body failed them.", "{0} couldn't handle the Kaioken strain and succumbed to its overwhelming power.",
        "The Kaioken strain proved too much for {0}'s mortal body, sealing their fate.", "{0} pushed their limits with Kaioken, only to find their limits pushed back.",
        "{0}'s body couldn't handle the explosive surge of power from Kaioken.", "In their reckless pursuit of power, {0} became a casualty of the deadly Kaioken strain.",
        "The Kaioken strain exacted its toll on {0}."
    };
}