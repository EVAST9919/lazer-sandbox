using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using osu.Game.Overlays;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Sandbox.Extensions
{
    public static class OsuGameExtensions
    {
        public static SandboxRuleset GetRuleset(this DependencyContainer dependencies)
        {
            var rulesets = dependencies.Get<RulesetStore>().AvailableRulesets.Select(info => info.CreateInstance());
            return (SandboxRuleset)rulesets.FirstOrDefault(r => r is SandboxRuleset);
        }

        public static SandboxRuleset GetThisRuleset(this RulesetStore rulesetStore)
        {
            var rulesets = rulesetStore.AvailableRulesets.Select(info => info.CreateInstance());
            return (SandboxRuleset)rulesets.FirstOrDefault(r => r is SandboxRuleset);
        }

        public static OsuScreenStack GetScreenStack(this OsuGame game) => game.ChildrenOfType<OsuScreenStack>().FirstOrDefault();

        public static SettingsOverlay GetSettingsOverlay(this OsuGame game) => game.ChildrenOfType<SettingsOverlay>().FirstOrDefault();
    }
}
