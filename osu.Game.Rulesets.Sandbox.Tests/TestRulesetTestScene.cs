using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.MusicHelpers;

namespace osu.Game.Rulesets.Sandbox.Tests
{
    public abstract class TestRulesetTestScene : RulesetTestScene
    {
        private readonly NowPlayingOverlay nowPlayingOverlay;

        public TestRulesetTestScene()
        {
            AddRange(new Drawable[]
            {
                CreateTestPlayfield(),
                nowPlayingOverlay = new NowPlayingOverlay
                {
                    Origin = Anchor.TopRight,
                    Anchor = Anchor.TopRight,
                    State = { Value = Visibility.Visible },
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddStep("Snow music controller", () => nowPlayingOverlay.State.Value = Visibility.Visible);
        }

        protected abstract TestPlayfield CreateTestPlayfield();

        protected abstract class TestPlayfield : CurrentBeatmapProvider
        {
            public TestPlayfield()
            {
                RelativeSizeAxes = Axes.Both;
            }
        }
    }
}
