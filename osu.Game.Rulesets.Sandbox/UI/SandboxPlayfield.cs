using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Sandbox.UI
{
    public class SandboxPlayfield : Playfield
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                HitObjectContainer
            };
        }
    }
}
