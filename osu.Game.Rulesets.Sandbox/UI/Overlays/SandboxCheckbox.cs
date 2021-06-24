using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.UI.Overlays
{
    public class SandboxCheckbox : CompositeDrawable
    {
        public readonly BindableBool Current = new BindableBool();

        public SandboxCheckbox(string label)
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            AddInternal(new GridContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.AutoSize)
                },
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.AutoSize),
                    new Dimension()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new LocalBox
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Current = { BindTarget = Current }
                        },
                        new SpriteText
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Colour = Color4.Black,
                            Font = OsuFont.GetFont(size: 20),
                            Text = label
                        }
                    }
                }
            });
        }

        protected override bool OnClick(ClickEvent e)
        {
            Current.Toggle();
            return true;
        }

        private class LocalBox : CompositeDrawable
        {
            public readonly BindableBool Current = new BindableBool();

            private readonly Box fill;

            public LocalBox()
            {
                Size = new Vector2(20);
                Masking = true;
                BorderColour = Color4.Black;
                BorderThickness = 3;
                InternalChild = fill = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    EdgeSmoothness = Vector2.One,
                    Colour = Color4.Black,
                    AlwaysPresent = true
                };
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                Current.BindValueChanged(enabled =>
                {
                    fill.Alpha = enabled.NewValue ? 1 : 0;
                }, true);
            }
        }
    }
}
