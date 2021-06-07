using osu.Framework.Allocation;
using osu.Framework.Screens;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Rulesets.Sandbox.Extensions;
using osu.Game.Rulesets.Sandbox.Screens.Main.Components;
using osu.Game.Rulesets.Sandbox.Screens.Numbers;

namespace osu.Game.Rulesets.Sandbox.Screens.Main
{
    public class MainRulesetScreen : SandboxScreen
    {
        public MainRulesetScreen()
        {
            InternalChild = new SandboxButtonSystem
            {
                Buttons = new[]
                {
                    new SandboxSelectionButton("2048") { Action = () => this.Push(new NumbersScreen()) }
                }
            };
        }

        private DependencyContainer dependencies;
        private SandboxRulesetConfigManager config;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            var ruleset = dependencies.GetRuleset();

            // Cache ruleset's config
            config = dependencies.Get<RulesetConfigCache>().GetConfigFor(ruleset) as SandboxRulesetConfigManager;
            if (config != null)
                dependencies.Cache(config);

            return dependencies;
        }
    }
}
