using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Sandbox.Screens
{
    public abstract class SandboxScreen : ScreenWithBeatmapBackground
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();

            Beatmap.BindValueChanged(b => updateComponentFromBeatmap(b.NewValue));
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);
            this.FadeInFromZero(250, Easing.OutQuint);
            updateComponentFromBeatmap(Beatmap.Value);
        }

        public override bool OnExiting(IScreen next)
        {
            this.FadeOut(250, Easing.OutQuint);
            return base.OnExiting(next);
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);
            this.FadeIn(250, Easing.OutQuint);
        }

        public override void OnSuspending(IScreen next)
        {
            base.OnSuspending(next);
            this.FadeOut(250, Easing.OutQuint);
        }

        private void updateComponentFromBeatmap(WorkingBeatmap beatmap)
        {
            ApplyToBackground(b =>
            {
                b.IgnoreUserSettings.Value = false;
                b.Beatmap = beatmap;
            });
        }
    }
}
