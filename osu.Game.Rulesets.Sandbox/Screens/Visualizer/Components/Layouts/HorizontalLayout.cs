using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Layouts.Horizontal;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Layouts
{
    public class HorizontalLayout : DrawableVisualizerLayout
    {
        public HorizontalLayout()
        {
            AddInternal(new HorizontalVisualizerController());
        }
    }
}
