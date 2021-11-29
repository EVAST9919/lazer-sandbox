using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Game.Graphics.Cursor;
using osu.Game.Input;
using osu.Game.Input.Bindings;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Settings;
using osu.Game.Rulesets.Sandbox.UI;
using osu.Game.Rulesets.Sandbox.UI.Settings;
using osuTK;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer
{
    public class VisualizerScreen : SandboxScreenWithSettings, IKeyBindingHandler<GlobalAction>
    {
        public override bool AllowBackButton => false;

        public override bool HideOverlaysOnEnter => true;

        private readonly IBindable<bool> isIdle = new BindableBool();
        private readonly BindableBool showTip = new BindableBool();
        private CursorHider cursorHider;

        [BackgroundDependencyLoader]
        private void load(SandboxRulesetConfigManager config, IdleTracker idleTracker)
        {
            config.BindWith(SandboxRulesetSetting.ShowSettingsTip, showTip);
            isIdle.BindTo(idleTracker.IsIdle);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (showTip.Value)
            {
                var tip = new VisualizerSettingsTip();
                AddInternal(tip);
                tip.Show();
            }

            AddInternal(cursorHider = new CursorHider());

            isIdle.BindValueChanged(idle =>
            {
                if (idle.NewValue)
                {
                    SettingsVisible.Value = false;
                    cursorHider.Size = Vector2.One;
                }
                else
                {
                    cursorHider.Size = Vector2.Zero;
                }
            });
        }

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

        public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
        {
            switch (e.Action)
            {
                case GlobalAction.Back:
                    this.Exit();
                    return true;
            }

            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
        {
        }

        private class CursorHider : CompositeDrawable, IProvideCursor
        {
            public CursorHider()
            {
                RelativeSizeAxes = Axes.Both;
                Size = Vector2.Zero;
            }

            public CursorContainer Cursor => new EmptyCursor();

            public bool ProvidingUserCursor => true;

            protected override bool OnHover(HoverEvent e)
            {
                base.OnHover(e);
                return true;
            }

            private class EmptyCursor : CursorContainer
            {
                protected override Drawable CreateCursor() => Empty();
            }
        }
    }
}
