using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Sandbox.Screens.Fractal.Components;
using osu.Game.Rulesets.Sandbox.UI.Overlays;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.Fractal
{
    public class FractalScreen : SandboxScreen
    {
        private readonly Tip tip;
        private readonly OsuSpriteText text;
        private readonly InteractiveFractalDrawable fractal;

        private readonly Bindable<float> zoom = new Bindable<float>();

        public FractalScreen()
        {
            AddRangeInternal(new Drawable[]
            {
                fractal = new InteractiveFractalDrawable(),
                new Container
                {
                    Margin = new MarginPadding(20),
                    Masking = true,
                    CornerRadius = 5,
                    AutoSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Black,
                            Alpha = 0.5f
                        },
                        text = new OsuSpriteText
                        {
                            Margin = new MarginPadding(10),
                            Font = OsuFont.GetFont(size: 30, weight: FontWeight.Light)
                        }
                    }
                },
                tip = new Tip()
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            zoom.BindTo(fractal.ZoomLevel);
            zoom.BindValueChanged(z => text.Text = $"Zoom: {z.NewValue:0.00}x", true);

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
