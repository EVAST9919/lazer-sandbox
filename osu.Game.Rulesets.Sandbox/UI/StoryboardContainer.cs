﻿using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.MusicHelpers;
using osuTK.Graphics;
using System.Threading;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Storyboards.Drawables;
using osu.Framework.Timing;
using osu.Game.Storyboards;
using osuTK;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Sandbox.UI
{
    public partial class StoryboardContainer : CurrentBeatmapProvider
    {
        private readonly BindableDouble dim = new BindableDouble();
        private readonly BindableBool showStoryboard = new BindableBool();

        private Container<StoryboardLayer> storyboardContainer;
        private LoadingSpinner loading;

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager osuConfig, SandboxRulesetConfigManager rulesetConfig)
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Margin = new MarginPadding(20),
                    Child = loading = new LoadingSpinner(true)
                    {
                        Scale = new Vector2(1.5f)
                    },
                },
                storyboardContainer = new Container<StoryboardLayer>
                {
                    RelativeSizeAxes = Axes.Both
                }
            };

            osuConfig?.BindWith(OsuSetting.DimLevel, dim);
            rulesetConfig?.BindWith(SandboxRulesetSetting.ShowStoryboard, showStoryboard);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            dim.BindValueChanged(d => updateStoryboardDim((float)d.NewValue), true);
            showStoryboard.BindValueChanged(_ => updateStoryboard(Beatmap.Value));
        }

        protected override void OnBeatmapChanged(ValueChangedEvent<WorkingBeatmap> beatmap)
        {
            updateStoryboard(beatmap.NewValue);
        }

        private CancellationTokenSource cancellationToken;
        private StoryboardLayer storyboard;

        private void updateStoryboard(WorkingBeatmap beatmap)
        {
            cancellationToken?.Cancel();
            storyboard?.FadeOut(250, Easing.OutQuint).Expire();
            storyboard = null;

            if (!(showStoryboard.Value && beatmap.Storyboard.HasDrawable))
            {
                loading.Hide();
                return;
            }

            loading.Show();

            LoadComponentAsync(new StoryboardLayer(beatmap), loaded =>
            {
                storyboardContainer.Add(storyboard = loaded);
                loaded.FadeIn(250, Easing.OutQuint);
                loading.Hide();
            }, (cancellationToken = new CancellationTokenSource()).Token);
        }

        private void updateStoryboardDim(float newDim) => Colour = new Color4(1 - newDim, 1 - newDim, 1 - newDim, 1);

        private partial class StoryboardLayer : AudioContainer
        {
            private readonly WorkingBeatmap beatmap;
            private readonly InterpolatingFramedClock storyboardClock;

            public StoryboardLayer(WorkingBeatmap beatmap)
            {
                this.beatmap = beatmap;

                storyboardClock = new InterpolatingFramedClock();
                storyboardClock.ChangeSource(beatmap.Track);
                storyboardClock.ProcessFrame();
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                RelativeSizeAxes = Axes.Both;
                Volume.Value = 0;
                Alpha = 0;

                Drawable layer;

                if (beatmap.Storyboard.ReplacesBackground)
                {
                    layer = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black
                    };
                }
                else
                {
                    layer = new BeatmapBackground(beatmap);
                }

                Children = new Drawable[]
                {
                    layer,
                    new FillStoryboard(beatmap.Storyboard) { Clock = new InterpolatingFramedClock(storyboardClock) }
                };
            }

            private partial class FillStoryboard : DrawableStoryboard
            {
                protected override Vector2 DrawScale => Scale;

                public FillStoryboard(Storyboard storyboard)
                    : base(storyboard)
                {
                }

                protected override void Update()
                {
                    base.Update();
                    Scale = DrawWidth / DrawHeight > Parent.DrawWidth / Parent.DrawHeight ? new Vector2(Parent.DrawHeight / Height) : new Vector2(Parent.DrawWidth / Width);
                }
            }
        }
    }
}
