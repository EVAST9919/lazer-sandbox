using osu.Game.Rulesets.Sandbox.Screens.FlappyDon.Components;

namespace osu.Game.Rulesets.Sandbox.Screens.FlappyDon
{
    public class FlappyDonScreen : SandboxScreen
    {
        public FlappyDonScreen()
        {
            AddInternal(new FlappyDonGame());
        }
    }
}
