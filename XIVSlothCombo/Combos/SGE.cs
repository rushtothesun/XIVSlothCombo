﻿using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVSlothComboPlugin.Combos
{
    internal static class SGE
    {
        public const byte JobID = 40;

        public const uint
            Diagnosis = 24284,
            Holos = 24310,
            Ixochole = 24299,
            Egeiro = 24287,
            Kardia = 24285,
            Soteria = 24294,
            Phlegma = 24289,
            Phlegmara = 24307,
            Phlegmaga = 24313,
            Dyskrasia = 24297,
            Dyskrasia2 = 24315,
            Eukrasia = 24290,

            // dps
            Dosis1 = 24283,
            Dosis2 = 24306,
            Dosis3 = 24312,
            EukrasianDosis1 = 24293,
            EukrasianDosis2 = 24308,
            EukrasianDosis3 = 24314,
            Toxicon = 24304,
            Toxicon2 = 24316,
            // Other
            Swiftcast = 7561,
            LucidDreaming = 7562;

        public static class Buffs
        {
            public const ushort
                Kardia = 2604,
                Eukrasia = 2606,
                Swiftcast = 167;
        }

        public static class Debuffs
        {
            public const ushort
            EukrasianDosis1 = 2614,
            EukrasianDosis2 = 2615,
            EukrasianDosis3 = 2616;
        }

        public static class Levels
        {
            public const ushort
                Dosis = 1,
                Prognosis = 10,
                Druochole = 45,
                Kerachole = 50,
                Taurochole = 62,
                Ixochole = 52,
                Toxicon = 66,
                Dosis2 = 72,
                Holos = 76,
                Rizomata = 74,
                Toxicon2 = 82,
                Dosis3 = 82;
        }
    }

    internal class SageKardiaFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SageKardiaFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SGE.Soteria)
            {
                var soteriaCD = GetCooldown(SGE.Soteria);
                if (HasEffect(SGE.Buffs.Kardia) && !soteriaCD.IsCooldown)
                    return SGE.Soteria;
                return SGE.Kardia;
            }

            return actionID;
        }
    }

    internal class SagePhlegmaFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SagePhlegmaFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SGE.Phlegma || actionID == SGE.Phlegmara || actionID == SGE.Phlegmaga)
            {
                var lucidDreaming2 = GetCooldown(SGE.LucidDreaming);
                if (!lucidDreaming2.IsCooldown && LocalPlayer.CurrentMp <= 8000)
                {
                    return SGE.LucidDreaming;
                }

                if (level >= SGE.Levels.Dosis3)
                {
                    if (GetCooldown(SGE.Phlegmaga).CooldownRemaining > 45)
                    {
                        var dgauge = GetJobGauge<SGEGauge>();
                        if (dgauge.Addersting >= 1)
                        {
                            return SGE.Toxicon2;
                        }

                        return SGE.Dyskrasia2;
                    }
                }

                if (level >= SGE.Levels.Dosis2)
                {
                    if (GetCooldown(SGE.Phlegmara).CooldownRemaining > 45)
                    {
                        var dgauge = GetJobGauge<SGEGauge>();
                        if (dgauge.Addersting >= 1)
                        {
                            return SGE.Toxicon;
                        }
                        return SGE.Dyskrasia;
                    }
                }

                if (GetCooldown(SGE.Phlegma).CooldownRemaining > 45)
                {
                    if (level >= SGE.Levels.Toxicon)
                    {
                        var dgauge = GetJobGauge<SGEGauge>();
                        if (dgauge.Addersting >= 1)
                        {
                            return SGE.Toxicon;
                        }
                    }
                    return SGE.Dyskrasia;
                }
            }
            return actionID;
        }
    }

    internal class SageDPSFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SageDPSFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SGE.Dosis1 || actionID == SGE.Dosis2 || actionID == SGE.Dosis3)
            {
                var incombat = HasCondition(Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat);
                var dosis3CD = GetCooldown(SGE.Dosis3);
                var dosis1Debuff = FindTargetEffect(SGE.Debuffs.EukrasianDosis1);
                var dosis2Debuff = FindTargetEffect(SGE.Debuffs.EukrasianDosis2);
                var dosis3Debuff = FindTargetEffect(SGE.Debuffs.EukrasianDosis3);

                if (IsEnabled(CustomComboPreset.SageDPSFeature) && level >= 82 && incombat)
                {
                    if (HasEffect(SGE.Buffs.Eukrasia))
                        return SGE.EukrasianDosis3;
                    if ((dosis3Debuff is null) || (dosis3Debuff.RemainingTime <= 4))
                        return SGE.Eukrasia;
                }

                if (IsEnabled(CustomComboPreset.SageDPSFeature) && level >= 72 && level <= 81 && incombat)
                {
                    if (HasEffect(SGE.Buffs.Eukrasia))
                        return SGE.EukrasianDosis2;
                    if ((dosis2Debuff is null) || (dosis2Debuff.RemainingTime <= 4))
                        return SGE.Eukrasia;
                }

                if (IsEnabled(CustomComboPreset.SageDPSFeature) && level >= 30 && level <= 71 && incombat)
                {
                    if (HasEffect(SGE.Buffs.Eukrasia))
                        return SGE.EukrasianDosis1;
                    if ((dosis1Debuff is null) || (dosis1Debuff.RemainingTime <= 4))
                        return SGE.Eukrasia;
                }
                if (IsEnabled(CustomComboPreset.SageLucidFeature))
                {
                    var lucidDreaming = GetCooldown(SGE.LucidDreaming);
                    var actionIDCD = GetCooldown(actionID);
                    if (!lucidDreaming.IsCooldown && LocalPlayer.CurrentMp <= 8000 && actionIDCD.CooldownRemaining > 0.2)
                        return SGE.LucidDreaming;
                }
            }

            return actionID;
        }
    }

    internal class SageEgeiroFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SageEgeiroFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SGE.Egeiro)
            {
                var swiftCD = GetCooldown(SGE.Swiftcast);

                if (IsEnabled(CustomComboPreset.SageEgeiroFeature))
                {
                    if (HasEffect(SGE.Buffs.Swiftcast))
                        return SGE.Egeiro;
                }

                if (!swiftCD.IsCooldown)
                {
                    return OriginalHook(SGE.Swiftcast);
                }
            }

            return actionID;
        }
    }
    internal class SageDPSFeatureTest : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SageDPSFeatureTest;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SGE.Dosis1 || actionID == SGE.Dosis2 || actionID == SGE.Dosis3)
            {
                var incombat = HasCondition(Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat);
                var dosis3CD = GetCooldown(SGE.Dosis3);
                var dosis1Debuff = FindTargetEffect(SGE.Debuffs.EukrasianDosis1);
                var dosis2Debuff = FindTargetEffect(SGE.Debuffs.EukrasianDosis2);
                var dosis3Debuff = FindTargetEffect(SGE.Debuffs.EukrasianDosis3);
                var isAdd = 0;
                var isBoss = 0;


                if (EnemyHealthPercentage() >= 10 && EnemyHealthMaxHp() < 550000)
                {
                    isAdd = 1;
                    isBoss = 0;
                }

                if (EnemyHealthMaxHp() >= 600000 && EnemyHealthPercentage() >= 1)
                {
                    isBoss = 1;
                    isAdd = 0;
                }

                if (IsEnabled(CustomComboPreset.SageDPSFeatureTest) && level >= 82 && incombat && (isAdd == 1 || isBoss == 1))
                {
                    if (HasEffect(SGE.Buffs.Eukrasia))
                        return SGE.EukrasianDosis3;
                    if ((dosis3Debuff is null) || (dosis3Debuff.RemainingTime <= 4))
                        return SGE.Eukrasia;
                }

                if (IsEnabled(CustomComboPreset.SageDPSFeatureTest) && level >= 72 && level <= 81 && incombat && ((EnemyHealthPercentage() >= 10 && EnemyHealthMaxHp() < 550000) || (EnemyHealthMaxHp() >= 600000 && EnemyHealthPercentage() >= 1)))
                {
                    if (HasEffect(SGE.Buffs.Eukrasia))
                        return SGE.EukrasianDosis2;
                    if ((dosis2Debuff is null) || (dosis2Debuff.RemainingTime <= 4))
                        return SGE.Eukrasia;
                }

                if (IsEnabled(CustomComboPreset.SageDPSFeatureTest) && level >= 30 && level <= 71 && incombat)
                {
                    if (HasEffect(SGE.Buffs.Eukrasia))
                        return SGE.EukrasianDosis1;
                    if ((dosis1Debuff is null) || (dosis1Debuff.RemainingTime <= 4))
                        return SGE.Eukrasia;
                }
                if (IsEnabled(CustomComboPreset.SageDPSFeatureTest))
                {
                    var lucidDreaming = GetCooldown(SGE.LucidDreaming);
                    var actionIDCD = GetCooldown(actionID);
                    if (!lucidDreaming.IsCooldown && LocalPlayer.CurrentMp <= 8000 && actionIDCD.CooldownRemaining > 0.2)
                        return SGE.LucidDreaming;
                }
            }

            return actionID;
        }
    }

}
