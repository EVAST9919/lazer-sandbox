using System.ComponentModel;
using osu.Framework.Graphics;
using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;

namespace osu.Game.Rulesets.Sandbox.Configuration
{
    public class SandboxRulesetConfigManager : RulesetConfigManager<SandboxRulesetSetting>
    {
        public SandboxRulesetConfigManager(SettingsStore settings, RulesetInfo ruleset, int? variant = null)
            : base(settings, ruleset, variant)
        {
        }

        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();

            // Best scores
            SetDefault(SandboxRulesetSetting.NumbersGameBestScore, 0);
            SetDefault(SandboxRulesetSetting.FlappyDonGameBestScore, 0);

            // Visualizer
            SetDefault(SandboxRulesetSetting.ShowParticles, true);
            SetDefault(SandboxRulesetSetting.ParticleCount, 500, 50, 1000);
            SetDefault(SandboxRulesetSetting.ShowStoryboard, false);
            SetDefault(SandboxRulesetSetting.VisualizerLayout, VisualizerLayout.TypeA);
            SetDefault(SandboxRulesetSetting.ShowSettingsTip, true);
            SetDefault(SandboxRulesetSetting.ParticlesColour, "#ffffff");

            // TypeA settings
            SetDefault(SandboxRulesetSetting.Radius, 350, 200, 500);
            SetDefault(SandboxRulesetSetting.CircularBarType, CircularBarType.Basic);
            SetDefault(SandboxRulesetSetting.Rotation, 0, 0, 360);
            SetDefault(SandboxRulesetSetting.DecayA, 200, 100, 500);
            SetDefault(SandboxRulesetSetting.MultiplierA, 400, 200, 500);
            SetDefault(SandboxRulesetSetting.Symmetry, false);
            SetDefault(SandboxRulesetSetting.SmoothnessA, 1, 0, 50);
            SetDefault(SandboxRulesetSetting.BarWidthA, 3.0, 1, 20);
            SetDefault(SandboxRulesetSetting.BarsPerVisual, 120, 10, 3500);
            SetDefault(SandboxRulesetSetting.VisualizerAmount, 3, 1, 10);

            // TypeB settings
            SetDefault(SandboxRulesetSetting.DecayB, 200, 100, 500);
            SetDefault(SandboxRulesetSetting.MultiplierB, 400, 200, 500);
            SetDefault(SandboxRulesetSetting.SmoothnessB, 1, 0, 50);
            SetDefault(SandboxRulesetSetting.BarWidthB, 3.0, 1, 20);
            SetDefault(SandboxRulesetSetting.BarCountB, 120, 10, 3500);
            SetDefault(SandboxRulesetSetting.LinearBarType, LinearBarType.Basic);
        }
    }

    public enum SandboxRulesetSetting
    {
        NumbersGameBestScore,
        FlappyDonGameBestScore,

        // Visualizer
        ShowParticles,
        ParticleCount,
        ShowStoryboard,
        VisualizerLayout,
        ShowSettingsTip,
        ParticlesColour,

        // TypeA settings
        Radius,
        CircularBarType,
        Rotation,
        DecayA,
        MultiplierA,
        Symmetry,
        SmoothnessA,
        BarWidthA,
        BarsPerVisual,
        VisualizerAmount,

        // TypeB settings
        DecayB,
        MultiplierB,
        SmoothnessB,
        BarWidthB,
        BarCountB,
        LinearBarType
    }

    public enum VisualizerLayout
    {
        [Description("Type A")]
        TypeA,

        [Description("Type B")]
        TypeB,
        Empty
    }

    public enum CircularBarType
    {
        Basic,
        Rounded,
        Fall,
        Dots
    }

    public enum LinearBarType
    {
        Basic,
        Rounded
    }
}
