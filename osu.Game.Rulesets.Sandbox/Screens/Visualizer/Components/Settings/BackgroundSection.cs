using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Configuration;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Rulesets.Sandbox.UI.Settings;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Settings
{
    public class BackgroundSection : SandboxSettingsSection
    {
        protected override string HeaderName => "Background";

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config, SandboxRulesetConfigManager rulesetConfig)
        {
            AddRange(new Drawable[]
            {
                new SettingsCheckbox
                {
                    LabelText = "Show storyboard (if available)",
                    Current = rulesetConfig.GetBindable<bool>(SandboxRulesetSetting.ShowStoryboard)
                },
                new SettingsCheckbox
                {
                    LabelText = "Show particles",
                    Current = rulesetConfig.GetBindable<bool>(SandboxRulesetSetting.ShowParticles)
                },
                new ParticlesColourDropdown(),
                new SettingsSlider<int>
                {
                    LabelText = "Particle count",
                    Current = rulesetConfig.GetBindable<int>(SandboxRulesetSetting.ParticleCount),
                    KeyboardStep = 1
                },
                new SettingsSlider<double>
                {
                    LabelText = "Background dim",
                    Current = config.GetBindable<double>(OsuSetting.DimLevel),
                    KeyboardStep = 0.01f,
                    DisplayAsPercentage = true
                },
                new SettingsSlider<double>
                {
                    LabelText = "Background blur",
                    Current = config.GetBindable<double>(OsuSetting.BlurLevel),
                    KeyboardStep = 0.01f,
                    DisplayAsPercentage = true
                }
            });
        }

        private class ParticlesColourDropdown : SettingsDropdownContainer
        {
            private readonly Bindable<string> hexColour = new Bindable<string>();

            private OsuColourPicker picker;

            public ParticlesColourDropdown()
                : base("Particles colour")
            {
            }

            protected override Drawable CreateContent() => picker = new OsuColourPicker();

            [BackgroundDependencyLoader]
            private void load(SandboxRulesetConfigManager rulesetConfig)
            {
                rulesetConfig.BindWith(SandboxRulesetSetting.ParticlesColour, hexColour);
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                picker.Current.Value = Colour4.FromHex(hexColour.Value);
                picker.Current.BindValueChanged(c =>
                {
                    hexColour.Value = c.NewValue.ToHex();
                });
            }
        }
    }
}
