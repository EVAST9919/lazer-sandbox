using osu.Game.Rulesets.Sandbox.Screens.Numbers.Components;
using osu.Game.Tests.Visual;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Sandbox.Tests.NumbersGame
{
    public class TestSceneNumbersPlayfield : OsuTestScene
    {
        private readonly NumbersPlayfield playfield;

        public TestSceneNumbersPlayfield()
        {
            Add(playfield = new NumbersPlayfield
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            AddStep("Restart", playfield.Restart);
        }
    }
}
