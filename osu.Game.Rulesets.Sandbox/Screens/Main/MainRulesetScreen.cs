using osu.Framework.Allocation;
using osu.Framework.Screens;
using osu.Game.Rulesets.Sandbox.Extensions;
using osu.Game.Rulesets.Sandbox.Screens.FlappyDon;
using osu.Game.Rulesets.Sandbox.Screens.Main.Components;
using osu.Game.Rulesets.Sandbox.Screens.Numbers;
using osu.Game.Rulesets.UI;
using osu.Game.Screens;

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
                    new SandboxSelectionButton("2048") { Action = () => this.Push(new NumbersScreen()) },
                    new SandboxSelectionButton("FlappyDon") { Action = () => this.Push(new FlappyDonScreen()) } // by https://github.com/TimOliver
                }
            };
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var baseDependencies = new DependencyContainer(base.CreateChildDependencies(parent));

            return new OsuScreenDependencies(false, new DrawableRulesetDependencies(baseDependencies.GetRuleset(), baseDependencies));
        }
    }
}
