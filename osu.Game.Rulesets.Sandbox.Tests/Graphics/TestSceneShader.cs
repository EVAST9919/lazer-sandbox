using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Sandbox.Graphics;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Tests.Graphics
{
    public class TestSceneShader : RulesetTestScene
    {
        private readonly Container rotatingContainer;

        public TestSceneShader()
        {
            Add(rotatingContainer = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(500),
                Masking = true,
                BorderThickness = 5,
                BorderColour = Color4.White,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                        AlwaysPresent = true
                    },
                    new TimedDrawableShader("test")
                    {
                        RelativeSizeAxes = Axes.Both
                    }
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddStep("Spin", () => rotatingContainer.Spin(10000, RotationDirection.Clockwise));
            AddStep("Stop spin", () =>
            {
                rotatingContainer.ClearTransforms();
                rotatingContainer.Rotation = 0;
            });
        }
    }
}
