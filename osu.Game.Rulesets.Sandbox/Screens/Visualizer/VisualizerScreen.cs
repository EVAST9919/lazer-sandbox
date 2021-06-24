using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Screens;
using osu.Game.Graphics;
using osu.Game.Input.Bindings;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Settings;
using osu.Game.Rulesets.Sandbox.UI;
using osu.Game.Rulesets.Sandbox.UI.Overlays;
using osu.Game.Rulesets.Sandbox.UI.Settings;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer
{
    public class VisualizerScreen : SandboxScreenWithSettings, IKeyBindingHandler<GlobalAction>
    {
        public override bool AllowBackButton => false;

        public override bool HideOverlaysOnEnter => true;

        private readonly BindableBool showTip = new BindableBool();

        [BackgroundDependencyLoader]
        private void load(SandboxRulesetConfigManager config)
        {
            config.BindWith(SandboxRulesetSetting.ShowSettingsTip, showTip);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (showTip.Value)
            {
                var tip = new SettingsTip();
                AddInternal(tip);
                tip.Show();
            }
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

        private class SettingsTip : SandboxOverlay
        {
            private readonly BindableBool showAgain = new BindableBool();

            protected override SandboxOverlayButton[] CreateButtons() => new[]
            {
                new SandboxOverlayButton("Got it")
                {
                    ClickAction = onClick
                }
            };

            private SandboxCheckbox checkbox;

            protected override Drawable CreateContent() => new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Children = new Drawable[]
                {
                    new TextFlowContainer(f =>
                    {
                        f.Colour = Color4.Black;
                        f.Font = OsuFont.GetFont(size: 30, weight: FontWeight.Regular);
                    })
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        TextAnchor = Anchor.TopCentre,
                        Text = "To open visualizer settings move your cursor to the right side of the screen."
                    },
                    new Container
                    {
                        BypassAutoSizeAxes = Axes.Y,
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.TopCentre,
                        Y = 30,
                        AutoSizeAxes = Axes.Y,
                        Width = 200,
                        Child = checkbox = new SandboxCheckbox("Don't show again")
                    }
                }
            };

            [BackgroundDependencyLoader]
            private void load(SandboxRulesetConfigManager config)
            {
                config.BindWith(SandboxRulesetSetting.ShowSettingsTip, showAgain);
            }

            private void onClick()
            {
                if (checkbox.Current.Value)
                    showAgain.Value = false;

                Hide();
            }
        }
    }
}
