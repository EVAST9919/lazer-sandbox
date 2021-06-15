using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Layouts.TypeB;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Layouts
{
    public class TypeBLayout : DrawableVisualizerLayout
    {
        public TypeBLayout()
        {
            AddInternal(new TypeBVisualizerController());
        }
    }
}
