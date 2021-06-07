using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Sandbox.Screens.Numbers;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens
{
    public class MainRulesetScreen : SandboxScreen
    {
        private readonly FillFlowContainer<LocalButton> buttonsFlow;

        public MainRulesetScreen()
        {
            AddInternal(new OsuScrollContainer(Direction.Horizontal)
            {
                RelativeSizeAxes = Axes.Both,
                Height = 0.7f,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                ScrollbarVisible = false,
                Masking = false,
                Child = buttonsFlow = new FillFlowContainer<LocalButton>
                {
                    RelativeSizeAxes = Axes.Y,
                    AutoSizeAxes = Axes.X,
                    Direction = FillDirection.Horizontal,
                    Spacing = new Vector2(20, 0),
                    Children = new[]
                    {
                        new LocalButton("2048")
                        {
                            Action = () => this.Push(new NumbersScreen())
                        }
                    }
                }
            });
        }

        protected override void Update()
        {
            base.Update();
            buttonsFlow.Margin = new MarginPadding { Horizontal = DrawWidth / 2 - LocalButton.WIDTH / 2 };
        }

        private class LocalButton : CompositeDrawable
        {
            public static readonly float WIDTH = 300;

            public Action Action;

            [Resolved]
            private OsuColour colours { get; set; }

            private readonly string name;

            public LocalButton(string name)
            {
                this.name = name;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                RelativeSizeAxes = Axes.Y;
                Width = WIDTH;
                Masking = true;
                CornerRadius = 10;
                BorderThickness = 3;
                BorderColour = colours.Yellow;
                EdgeEffect = new EdgeEffectParameters
                {
                    Radius = 1,
                    Colour = colours.Yellow,
                    Hollow = true,
                    Type = EdgeEffectType.Glow
                };
                InternalChildren = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black,
                        Alpha = 0.5f
                    },
                    new OsuSpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Text = name,
                        Colour = colours.Yellow,
                        Font = OsuFont.GetFont(size: 40, weight: FontWeight.SemiBold)
                    }
                };
            }

            protected override bool OnHover(HoverEvent e)
            {
                TweenEdgeEffectTo(new EdgeEffectParameters
                {
                    Radius = 10,
                    Colour = colours.Yellow,
                    Hollow = true,
                    Type = EdgeEffectType.Glow
                }, 250, Easing.OutQuint);
                return true;
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                TweenEdgeEffectTo(new EdgeEffectParameters
                {
                    Radius = 1,
                    Colour = colours.Yellow,
                    Hollow = true,
                    Type = EdgeEffectType.Glow
                }, 250, Easing.OutQuint);
            }

            protected override bool OnClick(ClickEvent e)
            {
                Action?.Invoke();
                return true;
            }
        }
    }
}
