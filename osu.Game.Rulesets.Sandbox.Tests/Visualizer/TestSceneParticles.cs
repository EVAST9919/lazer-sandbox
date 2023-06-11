using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.MusicHelpers;
using osu.Game.Rulesets.Sandbox.UI;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Tests.Visualizer
{
    public partial class TestSceneParticles : RulesetTestScene
    {
        private readonly ParticlesDrawable particles;

        public TestSceneParticles()
        {
            AddRange(new Drawable[]
            {
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(500),
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Black
                        },
                        new CurrentRateContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = particles = new ParticlesDrawable()
                        }
                    }
                },
                new Container
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    AutoSizeAxes = Axes.Y,
                    Width = 400,
                    Child = new TrackController()
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            AddSliderStep("Restart", 1, 30000, 1000, c => particles.TargetCount = c);
            AddSliderStep("Direction", 0, 6, 0, d => particles.Direction.Value = (ParticlesDirection)d);
        }
    }
}
