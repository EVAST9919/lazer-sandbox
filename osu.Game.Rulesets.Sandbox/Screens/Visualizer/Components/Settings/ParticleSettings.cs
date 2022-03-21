using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Framework.Graphics;
using osuTK;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Sandbox.UI.Settings;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Settings
{
    public class ParticleSettings : FillFlowContainer
    {
        private readonly BindableBool showParticles = new BindableBool();

        [BackgroundDependencyLoader]
        private void load(SandboxRulesetConfigManager rulesetConfig)
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Direction = FillDirection.Vertical;
            Spacing = new Vector2(0, 5);
            Children = new Drawable[]
            {
                new SettingsEnumDropdown<ParticlesDirection>
                {
                    LabelText = "Particles direction",
                    Current = rulesetConfig.GetBindable<ParticlesDirection>(SandboxRulesetSetting.ParticlesDirection)
                },
                new ColourPickerDropdown("Particles colour", SandboxRulesetSetting.ParticlesColour),
                new SettingsSlider<int>
                {
                    LabelText = "Particle count",
                    Current = rulesetConfig.GetBindable<int>(SandboxRulesetSetting.ParticleCount),
                    KeyboardStep = 1
                }
            };

            rulesetConfig.BindWith(SandboxRulesetSetting.ShowParticles, showParticles);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            showParticles.BindValueChanged(s => Alpha = s.NewValue ? 1 : 0, true);
        }
    }
}
