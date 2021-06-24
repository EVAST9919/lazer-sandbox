using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Sandbox.Tests.Visualizer
{
    public class TestSceneSettingsTip : OsuTestScene
    {
        private readonly VisualizerSettingsTip tip;

        public TestSceneSettingsTip()
        {
            Add(tip = new VisualizerSettingsTip());
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddStep("Toggle visibility", tip.ToggleVisibility);
        }
    }
}
