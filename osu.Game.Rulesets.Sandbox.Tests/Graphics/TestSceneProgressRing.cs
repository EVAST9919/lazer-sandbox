using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Sandbox.Graphics;
using osu.Game.Rulesets.Sandbox.UI;
using osu.Game.Screens.Ranking.Expanded.Accuracy;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Tests.Graphics
{
    public class TestSceneProgressRing : RulesetTestScene
    {
        private readonly SandboxProgressRing yosoRing;
        private readonly SmoothCircularProgress smooth;
        private readonly CircularProgress basic;

        public TestSceneProgressRing()
        {
            AddRange(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Gray
                },
                new InteractiveContainer
                {
                    Child = new GridContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        RowDimensions = new[]
                        {
                            new Dimension()
                        },
                        ColumnDimensions = new[]
                        {
                            new Dimension(GridSizeMode.Relative, size: 1f / 3),
                            new Dimension(GridSizeMode.Relative, size: 1f / 3),
                            new Dimension(GridSizeMode.Distributed)
                        },
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new Vector2(0, 30),
                                    Children = new Drawable[]
                                    {
                                        new SpriteText
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Text = "CircularProgress"
                                        },
                                        basic = new CircularProgress
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Width = 200
                                        }
                                    }
                                },
                                new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new Vector2(0, 30),
                                    Children = new Drawable[]
                                    {
                                        new SpriteText
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Text = "SmoothCircularProgress"
                                        },
                                        smooth = new SmoothCircularProgress
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Width = 200
                                        }
                                    }
                                },
                                new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new Vector2(0, 30),
                                    Children = new Drawable[]
                                    {
                                        new SpriteText
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Text = "Shader"
                                        },
                                        yosoRing = new SandboxProgressRing
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Width = 200
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddSliderStep("Progress", -2f, 2.0f, 0.5f, v =>
            {
                yosoRing.Progress = v;
                smooth.Current.Value = v;
                basic.Current.Value = v;
            });
            AddSliderStep("Thickness", 0f, 1f, 0.3f, v => yosoRing.InnerRadius = smooth.InnerRadius = basic.InnerRadius = v);
            AddSliderStep("Height", 0f, 400f, 200f, v => yosoRing.Height = smooth.Height = basic.Height = v);
            AddSliderStep("Alpha", 0f, 1f, 1f, v => yosoRing.Alpha = smooth.Alpha = basic.Alpha = v);
            AddSliderStep("Color Alpha", 0f, 1f, 1f, v => yosoRing.Colour = smooth.Colour = basic.Colour = ColourInfo.GradientVertical(Color4.White.Opacity(v), Color4.Pink));
        }
    }
}
