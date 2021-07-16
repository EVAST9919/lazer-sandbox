using osu.Game.Rulesets.Sandbox.Graphics;
using osu.Game.Rulesets.Sandbox.Screens.Fractal.Components;

namespace osu.Game.Rulesets.Sandbox.Screens.Fractal
{
    public class FractalScreen : SandboxScreen
    {
        public FractalScreen()
        {
            AddInternal(new ContentFitContainer
            {
                Child = new InteractiveFractalDrawable()
            });
        }
    }
}
