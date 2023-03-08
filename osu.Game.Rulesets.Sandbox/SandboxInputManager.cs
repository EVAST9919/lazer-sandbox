using System.ComponentModel;
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
        [Description("2048 Up")]
        NumbersUp,

        [Description("2048 Down")]
        NumbersDown,

        [Description("2048 Left")]
        NumbersLeft,

        [Description("2048 Right")]
        NumbersRight,

        [Description("FlappyDon Jump")]
        FlappyJump
    }
}
