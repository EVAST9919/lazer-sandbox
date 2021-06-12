using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Rulesets.Sandbox.UI.Settings;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Settings
{
    public class VisualizerSection : SandboxSettingsSection
    {
        protected override string HeaderName => "Visualizer settings";

        [BackgroundDependencyLoader]
        private void load(SandboxRulesetConfigManager config)
        {
            AddRange(new Drawable[]
            {
                new SettingsCheckbox
                {
                    LabelText = "Show storyboard (if available)",
                    Current = config.GetBindable<bool>(SandboxRulesetSetting.ShowStoryboard)
                },
                new SettingsCheckbox
                {
                    LabelText = "Show particles",
                    Current = config.GetBindable<bool>(SandboxRulesetSetting.ShowParticles)
                },
                new SettingsSlider<int>
                {
                    LabelText = "Particle count",
                    Current = config.GetBindable<int>(SandboxRulesetSetting.ParticleCount),
                    KeyboardStep = 1,
                    TransferValueOnCommit = true
                }
            });
        }
    }
}
