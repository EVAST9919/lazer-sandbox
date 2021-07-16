using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Sandbox.Graphics
{
    public class ContentFitContainer : Container
    {
        protected override Container<Drawable> Content => content;

        private readonly Container content;

        public ContentFitContainer()
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
