using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.MusicHelpers;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Visualizers;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Visualizers.Linear;
using osu.Game.Rulesets.Sandbox.UI;

namespace osu.Game.Rulesets.Sandbox.Tests.Visualizer
{
    public class TestSceneLinearRounded : RulesetTestScene
    {
        private readonly Visualizer visualizer;
        private RoundedLinearMusicVisualizerDrawable drawable => visualizer.Drawable;

        public TestSceneLinearRounded()
        {
            AddRange(new Drawable[]
            {
                visualizer = new Visualizer(),
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
            AddSliderStep("Anchor", 0, 2, 0, value => drawable.BarAnchorBindable.Value = (BarAnchor)value);
            AddSliderStep("Bar count", 0, 3000, 30, value => drawable.BarCount.Value = value);
            AddSliderStep("Bar width", 1f, 20f, 15, value => drawable.BarWidth.Value = value);
        }

        private class Visualizer : MusicAmplitudesProvider
        {
            public readonly RoundedLinearMusicVisualizerDrawable Drawable;

            public Visualizer()
            {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                RelativeSizeAxes = Axes.X;
                Height = 200;
                Child = Drawable = new RoundedLinearMusicVisualizerDrawable();
            }

            protected override void OnAmplitudesUpdate(float[] amplitudes)
            {
                Drawable.SetAmplitudes(amplitudes);
            }
        }
    }
}
