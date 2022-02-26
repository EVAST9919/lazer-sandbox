using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Sandbox.UI;
using osu.Game.Tests.Visual;
using osuTK;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Sandbox.Tests.UI
{
    public class TestSceneInteractiveContainer : OsuTestScene
    {
        public TestSceneInteractiveContainer()
        {
            Add(new InteractiveContainer
            {
                Child = new Box
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(100)
                }
            });
        }
    }
}
