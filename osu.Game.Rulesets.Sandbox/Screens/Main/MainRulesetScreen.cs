using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Game.Rulesets.Sandbox.Extensions;
using osu.Game.Rulesets.Sandbox.Screens.FlappyDon;
using osu.Game.Rulesets.Sandbox.Screens.Main.Components;
using osu.Game.Rulesets.Sandbox.Screens.Numbers;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer;
using osu.Game.Rulesets.Sandbox.UI;
using osu.Game.Rulesets.UI;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Sandbox.Screens.Main
{
    public class MainRulesetScreen : SandboxScreen
    {
        public MainRulesetScreen()
        {
            InternalChildren = new Drawable[]
            {
                new SandboxButtonSystem
                {
                    Buttons = new[]
                    {
                        new SandboxPanel("Visualizer", "Vis") { Action = () => this.Push(new VisualizerScreen()) },
                        new SandboxPanel("2048", "Numbers") { Action = () => this.Push(new NumbersScreen()) },
                        new SandboxPanel("FlappyDon", "Flappy", new Creator("https://github.com/TimOliver", "Tim Oliver")) { Action = () => this.Push(new FlappyDonScreen()) }
                    }
                },
                new Container
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    AutoSizeAxes = Axes.Y,
                    Width = 400,
                    Margin = new MarginPadding { Bottom = 30 },
                    Child = new CheckSandboxUpdatesButton
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    }
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
