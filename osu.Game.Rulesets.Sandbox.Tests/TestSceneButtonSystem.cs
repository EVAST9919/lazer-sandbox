using osu.Game.Rulesets.Sandbox.Screens.Main.Components;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Sandbox.Tests
{
    public class TestSceneButtonSystem : OsuTestScene
    {
        private const int button_count = 10;

        public TestSceneButtonSystem()
        {
            var buttons = new SandboxSelectionButton[button_count];

            for (int i = 0; i < button_count; i++)
                buttons[i] = new SandboxSelectionButton((i + 1).ToString());

            Add(new SandboxButtonSystem { Buttons = buttons });
        }
    }
}
