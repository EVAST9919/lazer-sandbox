using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Layouts;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components
{
    public class LayoutController : CompositeDrawable
    {
        private readonly Bindable<VisualizerLayout> layoutBinable = new Bindable<VisualizerLayout>();

        [BackgroundDependencyLoader]
        private void load(SandboxRulesetConfigManager config)
        {
            RelativeSizeAxes = Axes.Both;
            config?.BindWith(SandboxRulesetSetting.VisualizerLayout, layoutBinable);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            layoutBinable.BindValueChanged(_ => updateLayout(), true);
        }

        private void updateLayout()
        {
            switch(layoutBinable.Value)
            {
                default:
                case VisualizerLayout.TypeA:
                    loadLayout(new CircularLayout());
                    break;

                case VisualizerLayout.TypeB:
                    loadLayout(new HorizontalLayout());
                    break;
            }
        }

        private void loadLayout(DrawableVisualizerLayout layout)
        {
            InternalChild = layout;
        }
    }
}
