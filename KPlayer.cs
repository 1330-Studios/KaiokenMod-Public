using System;
using System.Collections.Generic;
using System.Threading;

using KaiokenMod.Buffs;
using KaiokenMod.FormLoader;
using KaiokenMod.Items.Essence;
using KaiokenMod.Particle;
using KaiokenMod.Sounds;
using KaiokenMod.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace KaiokenMod;

public unsafe class KPlayer : ModPlayer {
    public KPlayer_Data Data = new();
    public short Essence;
    private Thread maxStrain;

    private static KaiForm Kaioken => FormRegister.KaiFormInstances["Kaioken"];

    public override void PreUpdate() {
        Essence = KaiokenEssenceHelper.GetKaiokenEssenceStatus(Player);

        Data.Formed = Player.HasBuff(Kaioken.BuffType);

        if (DeformingTime > 0) {
            if (Player.HasBuff(Kaioken.BuffType))
                DeformingTime = 0;
            DeformingTime--;
        } else {
            if (Data.Strain > 0 && !Player.HasBuff<KaiokenBuff>()) {
                Data.SetStrain(Math.Max(0, Data.Strain - Kaioken.GetStrainLoss(Data.Mastery) / 30), this);
            }
        }

        DampStrain.Add(Data.Strain);

        if (DampStrain.Count > 15)
            DampStrain.RemoveAt(0);

        if (Main.dedServ) return;
        if (!IsMainPlayer()) return;
        if (AtMaxStrain()) {
            if (maxStrain != null) return;
            var soundVal = Main.soundVolume;
            Main.soundVolume = 0;
            SoundRegister.Play("KaiForms_SE_04", Player.Center, false);

            void recursiveFunc(int step = 0) {
                while (true) {
                    if (step == 3) {
                        Player.KillMe(PlayerDeathReason.ByCustomReason(string.Format(KaiokenBuff.listOfReasons[KaiForm.random.Next(KaiokenBuff.listOfReasons.Length)], Player.name)), 50000, 0);
                        return;
                    }

                    if (!AtMaxStrain()) return;
                    TimeAtMaxStrain++;
                    if (Player.whoAmI == Main.LocalPlayer.whoAmI) {
                        Main.soundVolume = 0;
                        SoundRegister.Play("KaiForms_SE_04", Player.Center, false);
                    }

                    for (var i = 0; i < 4; i++) {
                        // allows for more accurate overlay
                        if (!AtMaxStrain()) return;
                        TimeAtMaxStrainF += 0.25f;
                        Thread.Sleep(250);
                    }

                    step = ++step;
                }
            }

            (maxStrain = new Thread(() => {
                Thread.Sleep(1000);
                recursiveFunc();
                Main.soundVolume = soundVal;
            }) { IsBackground = true }).Start();
            // awesome code...
        } else {
            TimeAtMaxStrainF = TimeAtMaxStrain = 0;

            maxStrain = null;
        }
    }

    private void HandleEssenceBuffs() {
        var bits = (BitsShort)Essence;
        var maxEssence = bits[15];

        if (bits[14]) {
            var speed = maxEssence ? 1.15f : 1.1f;

            Player.moveSpeed *= speed;
            Player.maxRunSpeed *= speed;
            Player.runAcceleration *= speed;

            if (Player.jumpSpeedBoost < 1)
                Player.jumpSpeedBoost = 1;

            Player.jumpSpeedBoost *= speed;
        }

        if (!Player.HasBuff(Kaioken.BuffType)) return;
        if (bits[4])
            Player.statManaMax2 += maxEssence ? 50 : 25;

        if (bits[5])
            Player.statLifeMax2 += maxEssence ? 150 : 100;

        if (bits[7])
            Player.GetCritChance(DamageClass.Generic) += maxEssence ? 0.25f : 0.15f;

        if (bits[8])
            Player.GetDamage(DamageClass.Generic).Base += maxEssence ? 50 : 25;
    }

    private bool downLast;
    public readonly List<Dust> auraDusts = new();

    public bool IsMainPlayer() => Player.whoAmI == Main.myPlayer;

    public override void PostUpdateBuffs() {
        auraDusts.RemoveAll(a => !a.active);
        HandleEssenceBuffs();

        if (KaiokenKeybinds.Toggle.Current && IsMainPlayer()) {
            if (!Deformed) {
                if (!Player.HasBuff<KaiokenBuff>()) {
                    if (FormingProgress > 30) {
                        Player.AddBuff(Kaioken.BuffType, 999999999);
                        SoundRegister.Play("KaiForms_SE_00", Player.Center);
                        JustFormed = 10;
                        DeformingTime = 0;
                        Main.GameZoomTarget = Zoom;

                        var col = KaiokenConfig.Instance.AuraColor;
                        col.R = (byte)Math.Max(col.R - 5, 0);
                        col.G = (byte)Math.Max(col.G - 5, 0);
                        col.B = (byte)Math.Max(col.B - 5, 0);

                        for (var i = 0; i < 150; i++) {
                            var spawnPos = AuraParticles.GetStartPos() * 5 + Player.Center + Player.velocity * 3.3333f;
                            var direction = Player.Center.DirectionTo(spawnPos);
                            direction.Normalize();

                            var dust = Dust.NewDustPerfect(spawnPos, ModContent.DustType<AuraDust>(), direction * 5, 160, col);
                            dust.rotation = spawnPos.AngleTo(Player.Center);
                            dust.customData = new AuraData(0, Player);
                        }


                    } else {
                        FormingProgress++;
                    }
                } else {
                    Main.GameZoomTarget = Zoom;
                }
            }
        } else {
            if (Deformed)
                Deformed = false;

            if (downLast)
                Main.GameZoomTarget = Zoom;

            if (!Player.HasBuff<KaiokenBuff>()) {
                if (FormingProgress > 0)
                    FormingProgress = 0;
            }
        }

        static bool Remove(IParticle a) => a is FadingParticle;
        if (JustFormed > 0) {
            Main.ParticleSystem_World_OverPlayers.Particles.RemoveAll(Remove);
            Main.ParticleSystem_World_BehindPlayers.Particles.RemoveAll(Remove);
        }

        if (Player.HasBuff<KaiokenBuff>()) {
            if (FormingProgress > 0)
                FormingProgress -= 4;
        }

        if (FormingProgress < 0)
            FormingProgress = 0;

        if (FormingProgress != 0) {
            Zoom = Math.Min(Math.Max(Zoom, 1), 2);

            var eased = (FormingProgress / 30f).EaseInOut();

            var zoom = Zoom + (0.5f * eased);

            Main.GameZoomTarget = zoom;
        }

        if (FormingProgress > 0) {
            const float speed = 0.005f;

            Player.moveSpeed = speed;
            Player.maxRunSpeed = speed;

            if (FormingProgress >= 5) {
                for (var i = 0; i < 6; i++) {
                    var origin = Player.Center;
                    origin.Y += (Player.height / 2f);
                    origin.Y += 2;

                    var spawnPos = new Vector2((AuraParticles.GetStartPos() * 10).X, 0) + origin + Player.velocity * 3.3333f;
                    var direction = spawnPos.DirectionTo(origin);
                    direction.Normalize();

                    var velocity = direction * .888f;

                    var fadingParticle = ParticleHelper._poolFadingAura.RequestParticle();
                    fadingParticle.SetBasicInfo(ModContent.Request<Texture2D>("KaiokenMod/Particle/AuraDust"), null, Vector2.Zero, Vector2.Zero);
                    fadingParticle.SetTypeInfo(20);
                    fadingParticle.Velocity = velocity + new Vector2(0, -0.33f);
                    fadingParticle.AccelerationPerFrame = velocity * 0.033f + new Vector2(0, -0.33f);
                    fadingParticle.ColorTint = Color.Lerp(KaiokenConfig.Instance.AuraColor, Color.White, 0.65f);
                    fadingParticle.LocalPosition = spawnPos;
                    fadingParticle.Rotation = (float)Random.Shared.NextDouble();
                    fadingParticle.FadeInNormalizedTime = 0.15f;
                    fadingParticle.FadeOutNormalizedTime = 0.35f;
                    fadingParticle.Scale = new Vector2(1, 1) * (float)((Random.Shared.NextDouble() * 0.6f) + 0.4f);

                    if (Random.Shared.NextDouble() >= 0.5)
                        Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
                    else
                        Main.ParticleSystem_World_BehindPlayers.Add(fadingParticle);
                }
            }
        }

        downLast = KaiokenKeybinds.Toggle.Current;
    }

    internal record struct AuraData(float Gravity, Player Player);

    public override void PostUpdateMiscEffects() {
        if (JustFormed > 0)
            JustFormed--;
    }

    public override void UpdateDead() {
        Data.SetStrain(0, this);
        maxStrain = null;
    }

    public List<double> DampStrain = new();

    public long tick;

    public int JustFormed;

    public override void SaveData(TagCompound tag) {
        tag.Set("KaiokenMod_Mastery", Data.Mastery);
        tag.Set("KaiokenMod_Strain", Data.Strain);
    }

    public override void LoadData(TagCompound tag) {
        if (tag.ContainsKey("KaiokenMod_Mastery"))
            Data.SetMastery(tag.GetFloat("KaiokenMod_Mastery"));
        if (tag.ContainsKey("KaiokenMod_Strain"))
            Data.SetStrain(tag.GetDouble("KaiokenMod_Strain"), this);
    }

    public override void PlayerDisconnect(Player player) {
        if (Player.HasBuff(Kaioken.BuffType)) {
            Player.DelBuff(Player.FindBuffIndex(Kaioken.BuffType));
        }
    }

    public bool FormedYet;
    public int FormingProgress;
    private bool Deformed;
    private int DeformingTime;
    private float Zoom;

    public override void ProcessTriggers(TriggersSet triggersSet) {
        if (!IsMainPlayer()) return;
        if (Player.statLife <= 0) return;
        if (!KaiokenKeybinds.Toggle.JustPressed) return;

        Zoom = Main.GameZoomTarget;

        if (!Player.HasBuff(Kaioken.BuffType)) return;
        Player.DelBuff(Player.FindBuffIndex(Kaioken.BuffType));
        SoundRegister.Play("KaiForms_SE_02", Player.Center);
        Deformed = true;
        DeformingTime = (int)(360 - (Data.Mastery * (Random.Shared.NextDouble() * 60f)));

        for (var i = 0; i < 150; i++) {
            var spawnPos = AuraParticles.GetStartPos() * (i < 75 ? 10 : 5) + Player.Center + Player.velocity * 3.3333f;
            var direction = spawnPos.DirectionTo(Player.Center);

            var dust = Dust.NewDustPerfect(spawnPos, ModContent.DustType<AuraDust>(), direction * 5, 160, KaiokenConfig.Instance.AuraColor);
            dust.rotation = (float)Random.Shared.NextDouble();
            dust.customData = new AuraData(0, Player);
        }
    }

    public override void OnHitAnything(float x, float y, Entity victim) {
        if (!((BitsShort)Essence)[15] || victim is null) return;
        switch (victim) {
            case NPC npc:
                npc.AddBuff(BuffID.Ichor, 60 * Main.rand.Next(10, 20));
                break;
            case Player player:
                player.AddBuff(BuffID.Ichor, 60 * Main.rand.Next(10, 20));
                break;
        }
    }

    public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter) {
        if (!Player.HasBuff(Kaioken.BuffType)) return true;

        if ((((BitsShort)Essence)[6] && Random.Shared.NextDouble() <= (0.07f + (((BitsShort)Essence)[15] ? 0.03f : 0)))) {
            for (var i = 0; i < 20; i++) {
                var spawnPos = AuraParticles.GetStartPos() * 5 + Player.Center + Player.velocity * 3.3333f;
                var direction = Player.Center.DirectionTo(spawnPos);
                direction.Normalize();

                var smoke = Color.WhiteSmoke;

                var dust = Dust.NewDustPerfect(spawnPos, ModContent.DustType<AuraDust>(), direction * 1.25f, 160, new Color(smoke.R - 20 * i, smoke.G - 20 * i, smoke.B - 20 * i));
                dust.rotation = spawnPos.AngleTo(Player.Center);
                dust.customData = new AuraData(0, Player);
            }

            damage = 0;
            playSound = false;
            return false;
        }

        var dr = Kaioken.GetDamageReduction(Data.Mastery, this);

        var damageDouble = (double)damage;
        damageDouble -= (damageDouble * dr);
        damage = (int)damageDouble;

        return true;

    }

    public override void PostUpdate() {
        tick++;
        if (!Player.HasBuff(Kaioken.BuffType) || tick % 15 != 0)
            return;
        if (((BitsShort)Essence)[1] && Random.Shared.NextDouble() < .7)
            return;

        AddStrain(KaiokenBuff.GetStrainGain(Player));
    }

    internal void AddStrain(double add) => Data.SetStrain(Data.Strain + add, this);

    internal bool AtMaxStrain() {
        return Math.Abs(Math.Round(Data.Strain) - Math.Round(Data.GetMaxStrain(this))) < 0.01f && Player.HasBuff<KaiokenBuff>();
    }

    public override void Unload() {
        Data = default;
        DampStrain = null;
        maxStrain = null;
        TimeAtMaxStrainF = tick = TimeAtMaxStrain = 0;
        FormedYet = false;
    }

    internal int TimeAtMaxStrain;
    internal float TimeAtMaxStrainF;
}