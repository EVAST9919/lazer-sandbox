using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Overlays;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.MusicHelpers;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Tests.Visualizer
{
    public class TestSceneParticles : RulesetTestScene
    {
        private readonly ParticlesDrawable particles;
        private readonly NowPlayingOverlay nowPlayingOverlay;

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
                nowPlayingOverlay = new NowPlayingOverlay
                {
                    Origin = Anchor.TopRight,
                    Anchor = Anchor.TopRight,
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            AddStep("Toggle visibility", nowPlayingOverlay.ToggleVisibility);
            AddSliderStep("Restart", 1, 30000, 1000, v => particles.Restart(v));
            AddStep("Toggle direction", particles.SetRandomDirection);
        }
    }
}
