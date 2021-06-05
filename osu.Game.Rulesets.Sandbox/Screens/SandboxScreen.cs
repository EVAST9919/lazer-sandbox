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
            updateComponentFromBeatmap(Beatmap.Value);
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
