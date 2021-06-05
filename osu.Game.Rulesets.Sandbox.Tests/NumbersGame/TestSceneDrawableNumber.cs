using osu.Framework.Graphics;
using osu.Game.Rulesets.Sandbox.Screens.Numbers.Components;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Sandbox.Tests.NumbersGame
{
    public class TestSceneDrawableNumber : OsuTestScene
    {
        private readonly DrawableNumber drawableNumber;

        public TestSceneDrawableNumber()
        {
            Add(drawableNumber = new DrawableNumber(0, 0)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            AddStep("Increment", () => drawableNumber.Increment());
        }
    }
}
