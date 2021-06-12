using osu.Framework.Allocation;
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

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Settings
{
    public class StoryboardContainer : CurrentBeatmapProvider
    {
        private readonly BindableDouble dim = new BindableDouble();
        private readonly BindableBool showStoryboard = new BindableBool();

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager osuConfig, SandboxRulesetConfigManager rulesetConfig)
        {
            RelativeSizeAxes = Axes.Both;

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
        private AudioContainer storyboard;

        private void updateStoryboard(WorkingBeatmap beatmap)
        {
            cancellationToken?.Cancel();
            storyboard?.FadeOut(250, Easing.OutQuint).Expire();
            storyboard = null;

            if (!showStoryboard.Value)
                return;

            if (!beatmap.Storyboard.HasDrawable)
                return;

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

            LoadComponentAsync(new AudioContainer
            {
                RelativeSizeAxes = Axes.Both,
                Volume = { Value = 0 },
                Alpha = 0,
                Children = new Drawable[]
                {
                    layer,
                    new DrawableStoryboard(beatmap.Storyboard) { Clock = new InterpolatingFramedClock(beatmap.Track) }
                }
            }, loaded =>
            {
                Add(storyboard = loaded);
                loaded.FadeIn(250, Easing.OutQuint);
            }, (cancellationToken = new CancellationTokenSource()).Token);
        }

        private void updateStoryboardDim(float newDim) => Colour = new Color4(1 - newDim, 1 - newDim, 1 - newDim, 1);
    }
}
