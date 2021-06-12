using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Sandbox.UI.Settings.Sections;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer
{
    public class VisualizerScreen : SandboxScreenWithSettings
    {
        protected override Drawable CreateBackground() => Empty();

        protected override Drawable CreateMovingContent() => Empty();

        protected override SandboxSettingsSection[] CreateSettingsSections() => Array.Empty<SandboxSettingsSection>();
    }
}
