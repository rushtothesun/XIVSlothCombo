using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVSlothComboPlugin.Combos
{
    internal static class SMN
    {
        public const byte ClassID = 15;
        public const byte JobID = 27;

        public const float CooldownThreshold = 0.5f;

        public const uint
            // summons
            // summons
            SummonRuby = 25802,
            SummonTopaz = 25803,
            SummonEmerald = 25804,

            SummonIfrit = 25805,
            SummonTitan = 25806,
            SummonGaruda = 25807,

            SummonIfrit2 = 25838,
            SummonTitan2 = 25839,
            SummonGaruda2 = 25840,

            SummonCarbuncle = 25798,

            // summon abilities
            Gemshine = 25883,
            PreciousBrilliance = 25884,

            // summon ruins
            RubyRuin1 = 25808,
            RubyRuin2 = 25811,
            RubyRuin3 = 25817,
            TopazRuin1 = 25809,
            TopazRuin2 = 25812,
            TopazRuin3 = 25818,
            EmeralRuin1 = 25810,
            EmeralRuin2 = 25813,
            EmeralRuin3 = 25819,

            // summon outbursts
            RubyOutburst = 25814,
            TopazOutburst = 25815,
            EmeraldOutburst = 25816,

            // summon single targets
            RubyRite = 25823,
            TopazRite = 25824,
            EmeraldRite = 25825,

            // summon aoes
            RubyCata = 25832,
            TopazCata = 25833,
            EmeraldCata = 25834,

            // summon astral flows
            CrimsonCyclone = 25835, // dash
            CrimsonStrike = 25885, // melee
            MountainBuster = 25836,
            Slipstream = 25837,

            // demisummons
            SummonBahamut = 7427,
            SummonPhoenix = 25831,

            // demisummon abilities
            AstralImpulse = 25820, // single target bahamut gcd
            AstralFlare = 25821, // aoe bahamut gcd
            Deathflare = 3582, // damage ogcd bahamut
            EnkindleBahamut = 7429,

            FountainOfFire = 16514, // single target phoenix gcd
            BrandOfPurgatory = 16515, // aoe phoenix gcd
            Rekindle = 25830, // healing ogcd phoenix
            EnkindlePhoenix = 16516,

            // Transformation
            DreadwyrmTrance = 3581,
            Aethercharge = 25800,

            // shared summon abilities
            AstralFlow = 25822,

            // summoner gcds
            Ruin = 163,
            Ruin2 = 172,
            Ruin3 = 3579,
            Ruin4 = 7426,
            Tridisaster = 25826,

            // summoner AoE
            RubyDisaster = 25827,
            TopazDisaster = 25828,
            EmeraldDisaster = 25829,

            // summoner ogcds
            EnergyDrain = 16508,
            Fester = 181,
            EnergySiphon = 16510,
            Painflare = 3578,

            // Swift
            Swiftcast = 7561,

            // revive
            Resurrection = 173;


        public static class Buffs
        {
            public const ushort
                Swiftcast = 167,
                Aetherflow = 304,
                HellishConduit = 1867,
                FurtherRuin = 2701,
                GarudasFavor = 2725,
                TitansFavor = 2853,
                IfritsFavor = 2724,
                EverlastingFlight = 16517;
        }

        public static class Levels
        {
            public const byte
                SummonRuby = 6,
                SummonTopaz = 15,
                SummonEmerald = 22,
                SummonIfrit = 30,
                SummonTitan = 35,
                SummonGaruda = 45,
                SummonIfrit2 = 90,
                SummonTitan2 = 90,
                SummonGaruda2 = 90,
                Painflare = 52,
                Ruin3 = 54,
                EnhancedEgiAssault = 74,
                Ruin4 = 62,
                EnergyDrain = 10,
                EnergySyphon = 52,
                EnhancedFirebirdTrance = 80,
                OutburstMastery2 = 82,
                Slipstream = 86,
                MountainBuster = 86,

                Bahamut = 70,
                Phoenix = 80,

                Raise = 12,

                // summoner ruins lvls
                RubyRuin1 = 22,
                RubyRuin2 = 30,
                RubyRuin3 = 54,
                TopazRuin1 = 22,
                TopazRuin2 = 30,
                TopazRuin3 = 54,
                EmeralRuin1 = 22,
                EmeralRuin2 = 30,
                EmeralRuin3 = 54;
        }
    }

    #region Raise
    internal class SummonerSwiftRaise : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SummonerSwiftRaise;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Resurrection)
            {
                var swiftCD = GetCooldown(SMN.Swiftcast);
                if (swiftCD.CooldownRemaining == 0 || level <= SMN.Levels.Raise)
                    return SMN.Swiftcast;
            }

            return actionID;
        }
    }
    #endregion

    #region EDFester
    internal class SummonerEDFesterCombo : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SummonerEDFesterCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Fester)
            {
                var gauge = GetJobGauge<SMNGauge>();
                var edrainCD = GetCooldown(SMN.EnergyDrain);
                if (level >= SMN.Levels.EnergyDrain && !gauge.HasAetherflowStacks)
                    return SMN.EnergyDrain;
            }

            return actionID;
        }
    }
    #endregion

    #region ESPainflare
    internal class SummonerESPainflareCombo : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SummonerESPainflareCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Painflare)
            {
                var gauge = GetJobGauge<SMNGauge>();
                if (level >= SMN.Levels.EnergySyphon && !gauge.HasAetherflowStacks)
                    return SMN.EnergySiphon;
            }

            return actionID;
        }
    }
    #endregion

    #region EnkindleFeature
    internal class SummonerEnkindleFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SummonerEnkindleFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            // Replace demi summons with enkindle
            if (actionID == SMN.SummonBahamut || actionID == SMN.SummonPhoenix || actionID == SMN.DreadwyrmTrance || actionID == SMN.Aethercharge)
            {
                if (OriginalHook(SMN.Ruin) == SMN.AstralImpulse && level >= SMN.Levels.Bahamut)
                    return SMN.EnkindleBahamut;
                if (OriginalHook(SMN.Ruin) == SMN.FountainOfFire)
                    return SMN.EnkindlePhoenix;
            }

            return actionID;
        }
    }
    #endregion

    #region MegaButton
    internal class SummonMegaButton : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SummonMegaButton;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.SummonBahamut || actionID == SMN.SummonPhoenix || actionID == SMN.DreadwyrmTrance || actionID == SMN.Aethercharge)
            {
                var gauge = GetJobGauge<SMNGauge>();
                var bahamutCD = GetCooldown(SMN.SummonBahamut);
                var phoenixCD = GetCooldown(SMN.SummonPhoenix);

                if (bahamutCD.CooldownRemaining != 0 || phoenixCD.CooldownRemaining != 0)
                {
                    if (gauge.IsIfritReady && gauge.IsTitanReady && gauge.IsGarudaReady)
                    {
                        switch (level)
                        {
                            case <= 34:
                                return SMN.SummonTopaz;
                            case <= 89:
                                return SMN.SummonTitan;
                            case >= 90:
                                return SMN.SummonTitan2;
                        }
                    }
                    if (gauge.IsIfritReady && !gauge.IsTitanReady && gauge.IsGarudaReady)
                    {
                        switch (level)
                        {
                            case <= 44:
                                return SMN.SummonEmerald;
                            case <= 89:
                                return SMN.SummonGaruda;
                            case >= 90:
                                return SMN.SummonGaruda2;
                        }
                    }

                    if (gauge.IsIfritReady && !gauge.IsTitanReady && !gauge.IsGarudaReady)
                    {
                        switch (level)
                        {
                            case <= 29:
                                return SMN.SummonRuby;
                            case <= 89:
                                return SMN.SummonIfrit;
                            case >= 90:
                                return SMN.SummonIfrit2;
                        }
                    }

                    if (!gauge.IsIfritReady && gauge.IsTitanReady && gauge.IsGarudaReady)
                    {
                        switch (level)
                        {
                            case <= 34:
                                return SMN.SummonTopaz;
                            case <= 89:
                                return SMN.SummonTitan;
                            case >= 90:
                                return SMN.SummonTitan2;
                        }
                    }

                    if (!gauge.IsIfritReady && gauge.IsTitanReady && !gauge.IsGarudaReady)
                    {
                        switch (level)
                        {
                            case <= 34:
                                return SMN.SummonTopaz;
                            case <= 89:
                                return SMN.SummonTitan;
                            case >= 90:
                                return SMN.SummonTitan2;
                        }
                    }

                    if (!gauge.IsIfritReady && !gauge.IsTitanReady && gauge.IsGarudaReady)
                    {
                        switch (level)
                        {
                            case <= 44:
                                return SMN.SummonEmerald;
                            case <= 89:
                                return SMN.SummonGaruda;
                            case >= 90:
                                return SMN.SummonGaruda2;
                        }
                    }

                    if (gauge.IsIfritReady && gauge.IsTitanReady && !gauge.IsGarudaReady)
                    {
                        switch (level)
                        {
                            case <= 34:
                                return SMN.SummonTopaz;
                            case <= 89:
                                return SMN.SummonTitan;
                            case >= 90:
                                return SMN.SummonTitan2;
                        }
                    }
                }
            }

            return actionID;
        }
    }
    #endregion

    #region STCombo
    internal class RuinPrimalCombo : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RuinPrimalCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Ruin3 || actionID == SMN.Ruin || actionID == SMN.Ruin2)
            {
                var gauge = GetJobGauge<SMNGauge>();
                if (gauge.IsIfritAttuned)
                {
                    switch (level)
                    {
                        case <= 29:
                            return SMN.RubyRuin1;
                        case <= 53:
                            return SMN.RubyRuin2;
                        case <= 71:
                            return SMN.RubyRuin3;
                        case >= 72:
                            return SMN.RubyRite;
                    }
                }

                if (gauge.IsTitanAttuned || HasEffect(SMN.Buffs.TitansFavor))
                {
                    if (HasEffect(SMN.Buffs.TitansFavor))
                    {
                        return SMN.MountainBuster;
                    }

                    switch (level)
                    {
                        case <= 29:
                            return SMN.TopazRuin1;
                        case <= 53:
                            return SMN.TopazRuin2;
                        case <= 71:
                            return SMN.TopazRuin3;
                        case >= 72:
                            return SMN.TopazRite;
                    }
                }

                if (gauge.IsGarudaAttuned)
                {
                    switch (level)
                    {
                        case <= 29:
                            return SMN.EmeralRuin1;
                        case <= 53:
                            return SMN.EmeralRuin2;
                        case <= 71:
                            return SMN.EmeralRuin3;
                        case >= 72:
                            return SMN.EmeraldRite;
                    }
                }
            }

            return actionID;
        }
    }
    #endregion

    #region AoECombo
    internal class TriDisasterCombo : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.TriDisasterCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Tridisaster)
            {
                var gauge = GetJobGauge<SMNGauge>();
                if (gauge.IsIfritAttuned)
                {
                    switch (level)
                    {
                        case <= 73:
                            return SMN.RubyOutburst;
                        case <= 81:
                            return SMN.RubyDisaster;
                        case >= 82:
                            return SMN.RubyCata;
                    }
                }

                if (gauge.IsTitanAttuned || HasEffect(SMN.Buffs.TitansFavor))
                {
                    if (HasEffect(SMN.Buffs.TitansFavor))
                    {
                        return SMN.MountainBuster;
                    }

                    switch (level)
                    {
                        case <= 73:
                            return SMN.TopazOutburst;
                        case <= 81:
                            return SMN.TopazDisaster;
                        case >= 82:
                            return SMN.TopazCata;
                    }
                }

                if (gauge.IsGarudaAttuned)
                {
                    switch (level)
                    {
                        case <= 73:
                            return SMN.EmeraldOutburst;
                        case <= 81:
                            return SMN.EmeraldDisaster;
                        case >= 82:
                            return SMN.EmeraldCata;
                    }
                }
            }

            return actionID;
        }
    }
    #endregion

    
}
