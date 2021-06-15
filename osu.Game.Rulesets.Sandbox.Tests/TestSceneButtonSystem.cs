using osu.Game.Rulesets.Sandbox.Screens.Main.Components;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Sandbox.Tests
{
    public class TestSceneButtonSystem : OsuTestScene
    {
        private const int button_count = 10;

        public TestSceneButtonSystem()
        {
            var buttons = new SandboxPanel[button_count];

            for (int i = 0; i < button_count; i++)
                buttons[i] = new SandboxPanel((i + 1).ToString());

            Add(new SandboxButtonSystem { Buttons = buttons });
        }
    }
}
