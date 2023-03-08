using osu.Framework.Allocation;
using osu.Framework.Screens;
using osu.Game.Rulesets.Sandbox.Extensions;
using osu.Game.Rulesets.Sandbox.Screens.FlappyDon;
using osu.Game.Rulesets.Sandbox.Screens.Main.Components;
using osu.Game.Rulesets.Sandbox.Screens.Numbers;
using osu.Game.Rulesets.Sandbox.Screens.Rulesets;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer;
using osu.Game.Rulesets.UI;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Sandbox.Screens.Main
{
    public partial class MainRulesetScreen : SandboxScreen
    {
        public MainRulesetScreen()
        {
            InternalChild = new SandboxButtonSystem
            {
                Buttons = new[]
                {
                    new SandboxPanel("Visualizer") { Action = () => this.Push(new VisualizerScreen()) },
                    new SandboxPanel("2048") { Action = () => this.Push(new NumbersScreen()) },
                    new SandboxPanel("FlappyDon", new Creator { Name = "Tim Oliver", URL = "https://github.com/TimOliver"}) { Action = () => this.Push(new FlappyDonScreen()) },
                    new SandboxPanel("Rulesets") { Action = () => this.Push(new RulesetsScreen()) }
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
