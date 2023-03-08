using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Sandbox
{
    public partial class SandboxInputManager : RulesetInputManager<SandboxAction>
    {
        public SandboxInputManager(RulesetInfo ruleset)
            : base(ruleset, 0, SimultaneousBindingMode.Unique)
        {
        }
    }

    public enum SandboxAction
    {
        NumbersUp,
        NumbersDown,
        NumbersLeft,
        NumbersRight
    }
}
