using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Sandbox.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Sandbox.Tests.Graphics
{
    public partial class TestSceneInnerShadowContainer : RulesetTestScene
    {
        private readonly TestContainer testContainer;

        public TestSceneInnerShadowContainer()
        {
            AddRange(new Drawable[]
            {
                testContainer = new TestContainer()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(400)
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddSliderStep("Corner radius", 0, 200, 10, c => testContainer.CornerRadius = c);
        }

        private partial class TestContainer : InnerShadowContainer
        {
            protected override Container<Drawable> CreateContent() => new Container();
        }
    }
}
