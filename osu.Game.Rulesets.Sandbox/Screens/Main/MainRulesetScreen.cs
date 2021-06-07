using osu.Framework.Screens;
using osu.Game.Rulesets.Sandbox.Screens.Main.Components;
using osu.Game.Rulesets.Sandbox.Screens.Numbers;

namespace osu.Game.Rulesets.Sandbox.Screens.Main
{
    public class MainRulesetScreen : SandboxScreen
    {
        public MainRulesetScreen()
        {
            InternalChild = new SandboxButtonSystem
            {
                Buttons = new[]
                {
                    new SandboxSelectionButton("2048")
                    {
                        Action = () => this.Push(new NumbersScreen())
                    }
                }
            };
        }
    }
}
