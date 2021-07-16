using osu.Game.Rulesets.Sandbox.Screens.GameOfLife.Components;
using osu.Game.Rulesets.Sandbox.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.GameOfLife
{
    public class LifeGameScreen : SandboxScreen
    {
        public LifeGameScreen()
        {
            AddInternal(new ContentFitContainer
            {
                Child = new LifeGamePlayfield(10, 10)
            });
        }
    }
}
