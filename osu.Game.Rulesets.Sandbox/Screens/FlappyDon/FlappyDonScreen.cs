using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Sandbox.Screens.FlappyDon.Components;

namespace osu.Game.Rulesets.Sandbox.Screens.FlappyDon
{
    public class FlappyDonScreen : SandboxScreen
    {
        private Backdrop background;
        private Backdrop ground;

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal(new Drawable[]
            {
                background = new Backdrop(() => new BackgroundSprite(), 20000),
                ground = new Backdrop(() => new GroundSprite(), 2250)
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            background.Start();
            ground.Start();
        }
    }
}
