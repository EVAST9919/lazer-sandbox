using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Sandbox.Extensions;
using osu.Game.Tests.Visual;
using osu.Framework.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Sandbox.Tests.Visualizer
{
    public class TestSceneSmoothRandom : OsuTestScene
    {
        private readonly OsuSpriteText text;
        private readonly OpenSimplexNoise noise;
        private readonly Circle circle;

        public TestSceneSmoothRandom()
        {
            noise = new OpenSimplexNoise();

            Children = new Drawable[]
            {
                text = new OsuSpriteText(),
                circle = new Circle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(20)
                }
            };
        }

        protected override void Update()
        {
            base.Update();

            var value = noise.Evaluate(Time.Current / 1000, 0.0);

            text.Text = value.ToString();
            circle.Y = (float)value * 100;
        }
    }
}
