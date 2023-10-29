using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Sandbox.Tests
{
    public abstract partial class RulesetTestScene : OsuTestScene
    {
        protected override Ruleset CreateRuleset() => new SandboxRuleset();
    }
}
