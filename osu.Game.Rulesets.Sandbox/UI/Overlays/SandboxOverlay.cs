using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using osu.Game.Graphics;
using System;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Extensions.Color4Extensions;

namespace osu.Game.Rulesets.Sandbox.UI.Overlays
{
    public abstract class SandboxOverlay : OverlayContainer
    {
        public SandboxOverlay()
        {
            RelativeSizeAxes = Axes.Both;
            AddRange(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f
                },
                new Container
                {
                    Size = new Vector2(500, 300),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Masking = true,
                            CornerRadius = 3,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Color4.White,
                                    EdgeSmoothness = Vector2.One
                                },
                                new Container
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Padding = new MarginPadding { Top = 15, Horizontal = 15, Bottom = 30 },
                                    Child = CreateContent()
                                }
                            }
                        },
                        new FillFlowContainer<SandboxButton>
                        {
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.Centre,
                            Direction = FillDirection.Horizontal,
                            Spacing = new Vector2(40, 0),
                            Children = CreateButtons()
                        }
                    }
                }
            });
        }

        protected abstract Drawable CreateContent();

        protected abstract SandboxButton[] CreateButtons();

        protected override void PopIn() => this.FadeIn(250, Easing.OutQuint);

        protected override void PopOut() => this.FadeOut(250, Easing.OutQuint);

        protected class SandboxButton : CompositeDrawable
        {
            public Action ClickAction;

            private readonly Box bg;

            public SandboxButton(string text)
            {
                Size = new Vector2(100, 50);
                AddInternal(new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 5,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Colour = Color4.Black.Opacity(0.4f),
                        Offset = new Vector2(0, 5),
                        Radius = 10,
                        Type = EdgeEffectType.Shadow
                    },
                    Children = new Drawable[]
                    {
                        bg = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = baseColour,
                            EdgeSmoothness = Vector2.One
                        },
                        new SpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Colour = Color4.White,
                            Text = text,
                            Font = OsuFont.GetFont(size: 30, weight: FontWeight.SemiBold)
                        }
                    }
                });
            }

            private static Color4 baseColour => new Color4(20, 20, 20, 255);

            private static Color4 hoverColour => new Color4(40, 40, 40, 255);

            protected override bool OnClick(ClickEvent e)
            {
                ClickAction?.Invoke();
                return true;
            }

            protected override bool OnHover(HoverEvent e)
            {
                bg.FadeColour(hoverColour, 250, Easing.OutQuint);
                return true;
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                base.OnHoverLost(e);
                bg.FadeColour(baseColour, 250, Easing.OutQuint);
            }
        }
    }
}
