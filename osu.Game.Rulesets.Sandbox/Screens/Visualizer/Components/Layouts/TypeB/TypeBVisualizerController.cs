using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.MusicHelpers;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Visualizers;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Visualizers.Linear;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Layouts.TypeB
{
    public partial class TypeBVisualizerController : MusicAmplitudesProvider
    {
        private readonly Bindable<double> barWidth = new Bindable<double>();
        private readonly Bindable<int> barCount = new Bindable<int>();
        private readonly Bindable<int> multiplier = new Bindable<int>();
        private readonly Bindable<int> decay = new Bindable<int>();
        private readonly Bindable<int> smoothness = new Bindable<int>();
        private readonly Bindable<LinearBarType> type = new Bindable<LinearBarType>();
        private readonly Bindable<string> colour = new Bindable<string>("#ffffff");
        private readonly Bindable<string> progressColour = new Bindable<string>("#ffffff");
        private readonly Bindable<string> textColour = new Bindable<string>("#ffffff");

        private Container textContainer;
        private Box progress;
        private Container<LinearMusicVisualizerDrawable> visualizerContainer;

        [BackgroundDependencyLoader]
        private void load(SandboxRulesetConfigManager config)
        {
            Anchor = Anchor.BottomLeft;
            Origin = Anchor.BottomLeft;
            AutoSizeAxes = Axes.Both;
            Margin = new MarginPadding(50);
            Child = new FillFlowContainer
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Width = 500,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    visualizerContainer = new Container<LinearMusicVisualizerDrawable>
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 200
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 5,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Color4.White,
                                Alpha = 0.3f
                            },
                            progress = new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Width = 0
                            }
                        }
                    },
                    textContainer = new Container
                    {
                        AutoSizeAxes = Axes.Both
                    }
                }
            };

            config.BindWith(SandboxRulesetSetting.BarCountB, barCount);
            config.BindWith(SandboxRulesetSetting.BarWidthB, barWidth);
            config.BindWith(SandboxRulesetSetting.MultiplierB, multiplier);
            config.BindWith(SandboxRulesetSetting.DecayB, decay);
            config.BindWith(SandboxRulesetSetting.SmoothnessB, smoothness);
            config.BindWith(SandboxRulesetSetting.LinearBarType, type);

            config?.BindWith(SandboxRulesetSetting.TypeBColour, colour);
            config?.BindWith(SandboxRulesetSetting.TypeBProgressColour, progressColour);
            config?.BindWith(SandboxRulesetSetting.TypeBTextColour, textColour);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Beatmap.BindValueChanged(b =>
            {
                textContainer.Child = new LinearBeatmapName(b.NewValue.Metadata);
            }, true);

            type.BindValueChanged(t =>
            {
                LinearMusicVisualizerDrawable drawable;

                switch (t.NewValue)
                {
                    default:
                    case LinearBarType.Basic:
                        drawable = new BasicLinearMusicVisualizerDrawable();
                        break;

                    case LinearBarType.Rounded:
                        drawable = new RoundedLinearMusicVisualizerDrawable();
                        break;
                }

                visualizerContainer.Child = drawable.With(d =>
                {
                    d.BarAnchorBindable.Value = BarAnchor.Bottom;
                    d.BarWidth.BindTo(barWidth);
                    d.BarCount.BindTo(barCount);
                    d.HeightMultiplier.BindTo(multiplier);
                    d.Decay.BindTo(decay);
                    d.Smoothness.BindTo(smoothness);
                });
            }, true);

            colour.BindValueChanged(c => visualizerContainer.Colour = Colour4.FromHex(c.NewValue), true);
            progressColour.BindValueChanged(c => progress.Colour = Colour4.FromHex(c.NewValue), true);
            textColour.BindValueChanged(c => textContainer.Colour = Colour4.FromHex(c.NewValue), true);
        }

        protected override void Update()
        {
            base.Update();

            var track = Beatmap.Value?.Track;
            progress.Width = (float)((track == null || track.Length == 0) ? 0 : (track.CurrentTime / track.Length));
        }

        protected override void OnAmplitudesUpdate(float[] amplitudes)
        {
            visualizerContainer.Child?.SetAmplitudes(amplitudes);
        }

        private partial class LinearBeatmapName : OsuSpriteText
        {
            private readonly BeatmapMetadata metadata;

            private ILocalisedBindableString titleBinding;
            private ILocalisedBindableString artistBinding;

            public LinearBeatmapName(BeatmapMetadata metadata)
            {
                this.metadata = metadata;

                Font = OsuFont.GetFont(size: 30);
            }

            [BackgroundDependencyLoader]
            private void load(LocalisationManager localisation)
            {
                titleBinding = localisation.GetLocalisedBindableString(new RomanisableString(metadata.TitleUnicode, metadata.Title));
                artistBinding = localisation.GetLocalisedBindableString(new RomanisableString(metadata.ArtistUnicode, metadata.Artist));
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                titleBinding.BindValueChanged(_ => updateText());
                artistBinding.BindValueChanged(_ => updateText(), true);
            }

            private void updateText() => Text = $"{artistBinding.Value} - {titleBinding.Value}";
        }
    }
}
