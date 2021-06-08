using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Sandbox.Screens.FlappyDon.Components;

namespace osu.Game.Rulesets.Sandbox.Screens.FlappyDon
{
    public class FlappyDonScreen : SandboxScreen
    {
        private Backdrop background;

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal(new Drawable[]
            {
                background = new Backdrop(() => new BackgroundSprite(), 20000.0f)
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            background.Start();
        }
    }
}
