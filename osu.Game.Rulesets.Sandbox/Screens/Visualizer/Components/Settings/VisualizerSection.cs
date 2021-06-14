using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Rulesets.Sandbox.UI.Settings;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Settings
{
    public class VisualizerSection : SandboxSettingsSection
    {
        protected override string HeaderName => "Visualizer";

        [BackgroundDependencyLoader]
        private void load(SandboxRulesetConfigManager config)
        {
            AddRange(new Drawable[]
            {
                new SettingsEnumDropdown<VisualizerLayout>
                {
                    LabelText = "Layout type",
                    Current = config.GetBindable<VisualizerLayout>(SandboxRulesetSetting.VisualizerLayout)
                },
                new LayoutSettingsSubsection()
            });
        }
    }
}
