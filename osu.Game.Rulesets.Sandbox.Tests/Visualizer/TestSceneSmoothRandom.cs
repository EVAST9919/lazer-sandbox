using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Sandbox.Extensions;
using osu.Game.Tests.Visual;
using osu.Framework.Graphics;
using osuTK;
using System;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Sandbox.Tests.Visualizer
{
    public class TestSceneSmoothRandom : OsuTestScene
    {
        private readonly OsuSpriteText valueText;
        private readonly OsuSpriteText minText;
        private readonly OsuSpriteText maxText;
        private readonly OpenSimplexNoise noise;
        private readonly Circle circle;

        public TestSceneSmoothRandom()
        {
            noise = new OpenSimplexNoise();

            Children = new Drawable[]
            {
                new FillFlowContainer
                {
                    Margin = new MarginPadding(10),
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 10),
                    Children = new Drawable[]
                    {
                        valueText = new OsuSpriteText(),
                        minText = new OsuSpriteText(),
                        maxText = new OsuSpriteText()
                    }
                },
                circle = new Circle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(20)
                }
            };
        }

        private double min, max;

        protected override void Update()
        {
            base.Update();

            var value = noise.Evaluate(Time.Current / 1000, 0.0);

            min = Math.Min(value, min);
            max = Math.Max(value, max);

            valueText.Text = $"Current: {value}";
            minText.Text = $"Min: {min}";
            maxText.Text = $"Max: {max}";

            circle.Y = (float)value * 100;
        }
    }
}
