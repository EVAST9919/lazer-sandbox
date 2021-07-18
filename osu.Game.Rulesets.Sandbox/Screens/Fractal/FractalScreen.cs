using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Rulesets.Sandbox.Screens.Fractal.Components;
using osu.Game.Rulesets.Sandbox.UI.Overlays;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.Fractal
{
    public class FractalScreen : SandboxScreen
    {
        private readonly Tip tip;

        public FractalScreen()
        {
            AddRangeInternal(new Drawable[]
            {
                new InteractiveFractalDrawable(),
                tip = new Tip()
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            tip.Show();
        }

        private class Tip : SandboxOverlay
        {
            protected override SandboxOverlayButton[] CreateButtons() => new[]
            {
                new SandboxOverlayButton("Got it")
                {
                    ClickAction = onClick
                }
            };

            protected override Drawable CreateContent() => new TextFlowContainer(f =>
            {
                f.Colour = Color4.Black;
                f.Font = OsuFont.GetFont(size: 30, weight: FontWeight.Regular);
            })
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                TextAnchor = Anchor.TopCentre,
                Text = "Use mouse to scale/zoom the fractal."
            };

            private void onClick() => Hide();
        }
    }
}
