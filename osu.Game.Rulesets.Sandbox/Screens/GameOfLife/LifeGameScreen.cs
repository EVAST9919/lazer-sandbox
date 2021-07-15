using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Sandbox.Screens.GameOfLife.Components;

namespace osu.Game.Rulesets.Sandbox.Screens.GameOfLife
{
    public class LifeGameScreen : SandboxScreen
    {
        public LifeGameScreen()
        {
            AddInternal(new ContentAdjustmentContainer
            {
                Child = new LifeGamePlayfield(10, 10)
            });
        }

        private class ContentAdjustmentContainer : Container
        {
            protected override Container<Drawable> Content => content;

            private readonly Container content;

            public ContentAdjustmentContainer()
            {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                RelativeSizeAxes = Axes.Both;
                InternalChild = content = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fit,
                };
            }
        }
    }
}
