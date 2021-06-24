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
            AutoSizeAxes = Axes.Both;
            AddInternal(new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(10, 0),
                Children = new Drawable[]
                {
                    new LocalBox
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Current = { BindTarget = Current }
                    },
                    new SpriteText
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Colour = Color4.Black,
                        Font = OsuFont.GetFont(size: 20),
                        Text = label
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

            private readonly Container fill;

            public LocalBox()
            {
                Size = new Vector2(20);
                Masking = true;
                BorderColour = Color4.Black;
                BorderThickness = 3;
                CornerRadius = 3;
                InternalChildren = new Drawable[]
                {
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Masking = true,
                        BorderColour = Color4.Black,
                        BorderThickness = 3,
                        CornerRadius = 3,
                        Child = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0,
                            AlwaysPresent = true
                        }
                    },
                    fill = new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Masking = true,
                        CornerRadius = 3,
                        Child = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Black
                        }
                    }
                };
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                Current.BindValueChanged(enabled =>
                {
                    fill.ScaleTo(enabled.NewValue ? 0.5f : 0, 250, Easing.OutQuint);
                }, true);

                fill.FinishTransforms();
            }
        }
    }
}
