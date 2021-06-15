using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Layouts.TypeA;
using osuTK;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Layouts
{
    public class TypeALayout : DrawableVisualizerLayout
    {
        private readonly Bindable<int> radius = new Bindable<int>(350);

        private TypeAVisualizerController visualizerController;

        [BackgroundDependencyLoader]
        private void load(SandboxRulesetConfigManager config)
        {
            InternalChildren = new Drawable[]
            {
                visualizerController = new TypeAVisualizerController
                {
                    Position = new Vector2(0.5f),
                },
                new CircularBeatmapLogo
                {
                    Position = new Vector2(0.5f),
                    Size = { BindTarget = radius }
                }
            };

            config?.BindWith(SandboxRulesetSetting.Radius, radius);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            radius.BindValueChanged(r =>
            {
                visualizerController.Size = new Vector2(r.NewValue - 2);
            }, true);
        }
    }
}
