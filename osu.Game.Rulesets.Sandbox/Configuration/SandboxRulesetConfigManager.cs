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
            SetDefault(SandboxRulesetSetting.NumbersGameBestScore, 0);
        }
    }

    public enum SandboxRulesetSetting
    {
        NumbersGameBestScore
    }
}
