using osu.Game.Rulesets.Sandbox.Screens.Fractal.Components;

namespace osu.Game.Rulesets.Sandbox.Tests.Fractals
{
    public class TestSceneMandelbrot : RulesetTestScene
    {
        public TestSceneMandelbrot()
        {
            Add(new InteractiveFractalDrawable());
        }
    }
}
