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
                    }
                });
            }
        }

        private class TypeBSubsection : Subsection
        {

        }
    }
}
