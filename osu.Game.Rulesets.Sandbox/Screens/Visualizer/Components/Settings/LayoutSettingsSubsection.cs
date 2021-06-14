using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Framework.Bindables;
using osuTK;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Settings
{
    public class LayoutSettingsSubsection : CompositeDrawable
    {
        private readonly Bindable<VisualizerLayout> layoutBindable = new Bindable<VisualizerLayout>();

        [BackgroundDependencyLoader]
        private void load(SandboxRulesetConfigManager config)
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            config.BindWith(SandboxRulesetSetting.VisualizerLayout, layoutBindable);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            layoutBindable.BindValueChanged(updateSubsection, true);
        }

        private void updateSubsection(ValueChangedEvent<VisualizerLayout> layout)
        {
            switch (layout.NewValue)
            {
                default:
                case VisualizerLayout.TypeA:
                    loadSubsection(new TypeASubsection());
                    break;

                case VisualizerLayout.TypeB:
                    loadSubsection(new TypeBSubsection());
                    break;
            }
        }

        private void loadSubsection(Subsection s)
        {
            InternalChild = s;
        }

        private abstract class Subsection : FillFlowContainer
        {
            public Subsection()
            {
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;
                Direction = FillDirection.Vertical;
                Spacing = new Vector2(0, 10);
            }
        }

        private class TypeASubsection : Subsection
        {
            [BackgroundDependencyLoader]
            private void load(SandboxRulesetConfigManager config)
            {
                AddRange(new Drawable[]
                {
                    new SettingsSlider<int>
                    {
                        LabelText = "Radius",
                        KeyboardStep = 1,
                        Current = config.GetBindable<int>(SandboxRulesetSetting.Radius)
                    },
                    new SettingsEnumDropdown<CircularBarType>
                    {
                        LabelText = "Bar type",
                        Current = config.GetBindable<CircularBarType>(SandboxRulesetSetting.CircularBarType)
                    },
                    new SettingsCheckbox
                    {
                        LabelText = "Symmetry",
                        Current = config.GetBindable<bool>(SandboxRulesetSetting.Symmetry)
                    },
                    new SettingsSlider<int>
                    {
                        LabelText = "Visualizer count",
                        Current = config.GetBindable<int>(SandboxRulesetSetting.VisualizerAmount),
                        KeyboardStep = 1,
                        TransferValueOnCommit = true
                    },
                    new SettingsSlider<double>
                    {
                        LabelText = "Bar width",
                        Current = config.GetBindable<double>(SandboxRulesetSetting.BarWidthA),
                        KeyboardStep = 0.1f
                    },
                    new SettingsSlider<int>
                    {
                        LabelText = "Total bar count",
                        Current = config.GetBindable<int>(SandboxRulesetSetting.BarsPerVisual),
                        KeyboardStep = 1
                    },
                    new SettingsSlider<int>
                    {
                        LabelText = "Decay",
                        Current = config.GetBindable<int>(SandboxRulesetSetting.DecayA),
                        KeyboardStep = 1
                    },
                    new SettingsSlider<int>
                    {
                        LabelText = "Height Multiplier",
                        Current = config.GetBindable<int>(SandboxRulesetSetting.MultiplierA),
                        KeyboardStep = 1
                    },
                    new SettingsSlider<int>
                    {
                        LabelText = "Smoothness",
                        Current = config.GetBindable<int>(SandboxRulesetSetting.SmoothnessA),
                        KeyboardStep = 1
                    },
                    new SettingsSlider<int>
                    {
                        LabelText = "Rotation",
                        KeyboardStep = 1,
                        Current = config.GetBindable<int>(SandboxRulesetSetting.Rotation)
                    }
                });
            }
        }

        private class TypeBSubsection : Subsection
        {

        }
    }
}
