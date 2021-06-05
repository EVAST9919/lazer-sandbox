using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Sandbox.Screens.Numbers;
using osuTK;

namespace osu.Game.Rulesets.Sandbox.Screens
{
    public class MainRulesetScreen : SandboxScreen
    {
        public MainRulesetScreen()
        {
            AddInternal(new OsuScrollContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Width = 0.4f,
                Child = new FillFlowContainer<TriangleButton>
                {
                    Margin = new MarginPadding { Vertical = 10 },
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 10),
                    Children = new[]
                    {
                        new TriangleButton
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 50,
                            Text = "2048 Game",
                            Action = () => this.Push(new NumbersScreen())
                        }
                    }
                }
            });
        }
    }
}
