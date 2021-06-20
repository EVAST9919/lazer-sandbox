using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Screens;
using osu.Game.Input.Bindings;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Settings;
using osu.Game.Rulesets.Sandbox.UI;
using osu.Game.Rulesets.Sandbox.UI.Settings;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer
{
    public class VisualizerScreen : SandboxScreenWithSettings, IKeyBindingHandler<GlobalAction>
    {
        public override bool AllowBackButton => false;

        public override bool HideOverlaysOnEnter => true;

        protected override Drawable CreateBackground() => new Container
        {
            RelativeSizeAxes = Axes.Both,
            Children = new Drawable[]
            {
                new StoryboardContainer(),
                new Particles()
            }
        };

        protected override Drawable CreateContent() => new LayoutController();

        protected override SandboxSettingsSection[] CreateSettingsSections() => new SandboxSettingsSection[]
        {
            new TrackSection(),
            new BackgroundSection(),
            new VisualizerSection()
        };

        public bool OnPressed(GlobalAction action)
        {
            switch (action)
            {
                case GlobalAction.Back:
                    this.Exit();
                    return true;
            }

            return false;
        }

        public void OnReleased(GlobalAction action)
        {
        }
    }
}
