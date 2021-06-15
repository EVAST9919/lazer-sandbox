using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.MusicHelpers;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Visualizers;
using osuTK.Graphics;

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
        private OsuSpriteText text;
        private Box progress;

        [BackgroundDependencyLoader]
        private void load(SandboxRulesetConfigManager config)
        {
            Anchor = Anchor.BottomLeft;
            Origin = Anchor.BottomLeft;
            Width = 500;
            Height = 60;
            Margin = new MarginPadding(50);
            Children = new Drawable[]
            {
                text = new OsuSpriteText
                {
                    Margin = new MarginPadding { Top = 10 },
                    Font = OsuFont.GetFont(size: 30)
                },
                new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 7,
                    Colour = Color4.White,
                    Alpha = 0.3f
                },
                progress = new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 7,
                    Width = 0,
                    Colour = Color4.White
                },
                visualizer = new LinearMusicVisualizerDrawable
                {
                    Origin = Anchor.BottomLeft,
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

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Beatmap.BindValueChanged(b =>
            {
                text.Text = $"{b.NewValue.Metadata.Artist} - {b.NewValue.Metadata.Title}";
            }, true);
        }

        protected override void Update()
        {
            base.Update();

            var track = Beatmap.Value?.Track;
            progress.Width = (float)((track == null || track.Length == 0) ? 0 : (track.CurrentTime / track.Length));
        }

        protected override void OnAmplitudesUpdate(float[] amplitudes)
        {
            visualizer.SetAmplitudes(amplitudes);
        }
    }
}
