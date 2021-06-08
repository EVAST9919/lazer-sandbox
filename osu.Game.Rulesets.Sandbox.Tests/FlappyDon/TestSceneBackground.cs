using osu.Game.Rulesets.Sandbox.Screens.FlappyDon.Components;

namespace osu.Game.Rulesets.Sandbox.Tests.FlappyDon
{
    public class TestSceneBackground : RulesetTestScene
    {
        public TestSceneBackground()
        {
            Backdrop b;

            Add(b = new Backdrop(() => new BackgroundSprite(), 20000));
            b.Start();
        }
    }
}
