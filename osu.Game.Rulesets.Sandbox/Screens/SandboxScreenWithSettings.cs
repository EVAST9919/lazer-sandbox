using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osu.Game.Rulesets.Sandbox.UI.Settings;

namespace osu.Game.Rulesets.Sandbox.Screens
{
    public abstract class SandboxScreenWithSettings : SandboxScreen
    {
        private readonly SandboxSettings settings;

        public SandboxScreenWithSettings()
        {
            AddRangeInternal(new Drawable[]
            {
                CreateBackground(),
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RowDimensions = new[]
                    {
                        new Dimension()
                    },
                    ColumnDimensions = new[]
                    {
                        new Dimension(),
                        new Dimension(GridSizeMode.AutoSize)
                    },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = CreateContent()
                            },
                            settings = new SandboxSettings
                            {
                                Sections = CreateSettingsSections()
                            }
                        }
                    }
                }
            });
        }

        protected abstract Drawable CreateContent();

        protected new abstract Drawable CreateBackground();

        protected abstract SandboxSettingsSection[] CreateSettingsSections();

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            base.OnMouseMove(e);

            if (settings.IsVisible.Value)
                return false;

            var cursorPosition = ToLocalSpace(e.CurrentState.Mouse.Position);

            if (Precision.AlmostEquals(cursorPosition.X, DrawWidth, 1))
            {
                settings.IsVisible.Value = true;
                return true;
            }

            return false;
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (!settings.IsVisible.Value)
                return false;

            settings.IsVisible.Value = false;
            return true;
        }
    }
}
