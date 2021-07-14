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
        private readonly Container container;
        private readonly ShaderContainer shader;

        public TestSceneShader()
        {
            Add(container = new Container
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
                    shader = new ShaderContainer
                    {
                        RelativeSizeAxes = Axes.Both
                    }
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddStep("Load test shader", () => shader.ShaderName.Value = "test");
            AddStep("Load fractal shader", () => shader.ShaderName.Value = "fractal");
            AddStep("Load empty shader", () => shader.ShaderName.Value = string.Empty);

            AddStep("Spin", () => container.Spin(10000, RotationDirection.Clockwise));
            AddStep("Stop spin", () =>
            {
                container.ClearTransforms();
                container.Rotation = 0;
            });
        }
    }
}
