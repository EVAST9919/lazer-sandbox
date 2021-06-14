using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.MusicHelpers;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Visualizers;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Layouts.Horizontal
{
    public class HorizontalVisualizerController : MusicAmplitudesProvider
    {
        private readonly Bindable<double> barWidth = new Bindable<double>();
        private readonly Bindable<int> barCount = new Bindable<int>();
        private readonly Bindable<int> multiplier = new Bindable<int>();
        private readonly Bindable<int> decay = new Bindable<int>();
        private readonly Bindable<int> smoothness = new Bindable<int>();

        private LinearMusicVisualizerDrawable visualizer;

        [BackgroundDependencyLoader]
        private void load(SandboxRulesetConfigManager config)
        {
            Anchor = Anchor.BottomLeft;
            Origin = Anchor.BottomLeft;
            RelativeSizeAxes = Axes.X;
            Width = 0.5f;
            Height = 150;
            Margin = new MarginPadding(50);
            Children = new Drawable[]
            {
                visualizer = new LinearMusicVisualizerDrawable
                {
                    BarAnchorBindable = { Value = BarAnchor.Bottom },
                    BarWidth = { BindTarget = barWidth },
                    BarCount = { BindTarget = barCount },
                    HeightMultiplier = { BindTarget = multiplier },
                    Decay = { BindTarget = decay },
                    Smoothness = { BindTarget = smoothness }
                }
            };

            config.BindWith(SandboxRulesetSetting.BarCountB, barCount);
            config.BindWith(SandboxRulesetSetting.BarWidthB, barWidth);
            config.BindWith(SandboxRulesetSetting.MultiplierB, multiplier);
            config.BindWith(SandboxRulesetSetting.DecayB, decay);
            config.BindWith(SandboxRulesetSetting.SmoothnessB, smoothness);
        }

        protected override void OnAmplitudesUpdate(float[] amplitudes)
        {
            visualizer.SetAmplitudes(amplitudes);
        }
    }
}
