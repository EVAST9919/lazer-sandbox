using System.ComponentModel;
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
        }
    }

    public enum SandboxRulesetSetting
    {
        NumbersGameBestScore,
        FlappyDonGameBestScore,
        ShowParticles,
        ParticleCount,
        ShowStoryboard,
        VisualizerLayout
    }

    public enum VisualizerLayout
    {
        [Description("Type A")]
        TypeA,

        [Description("Type B")]
        TypeB
    }
}
