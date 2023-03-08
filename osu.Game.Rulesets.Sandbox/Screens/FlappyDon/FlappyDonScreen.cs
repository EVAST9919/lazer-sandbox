using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Sandbox.Extensions;
using osu.Game.Rulesets.Sandbox.Screens.FlappyDon.Components;

namespace osu.Game.Rulesets.Sandbox.Screens.FlappyDon
{
    public partial class FlappyDonScreen : SandboxScreen
    {
        [BackgroundDependencyLoader]
        private void load(RulesetStore rulesetStore)
        {
            AddInternal(new SandboxInputManager(rulesetStore.GetThisRuleset().RulesetInfo)
            {
                RelativeSizeAxes = Axes.Both,
                Child = new FlappyDonGame()
            });
        }
    }
}
