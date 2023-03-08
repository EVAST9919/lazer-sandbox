using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.Main.Components
{
    public partial class SandboxPanel : CompositeDrawable
    {
        public static readonly float WIDTH = 300;

        public Action Action;

        [Resolved]
        private OsuColour colours { get; set; }

        private readonly string name;
        private readonly Creator? creator;

        public SandboxPanel(string name, Creator? creator = null)
        {
            this.name = name;
            this.creator = creator;
        }

        private Sprite texture;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Container spriteHolder;

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
                spriteHolder = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = texture = new Sprite
                    {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        FillMode = FillMode.Fill,
                        Texture = textures.Get(name),
                        Colour = Color4.Gray
                    }
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

            if (creator.HasValue)
            {
                var linkFlow = new LinkFlowContainer(t =>
                {
                    t.Font = OsuFont.GetFont(size: 20);
                })
                {
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Margin = new MarginPadding { Bottom = 25 }
                };

                linkFlow.AddText("Created by ");
                linkFlow.AddLink(creator.Value.Name, creator.Value.URL);

                AddInternal(linkFlow);
            }
        }

        protected override bool OnHover(HoverEvent e)
        {
            texture?.FadeColour(Color4.DarkGray, 250, Easing.OutQuint);

            TweenEdgeEffectTo(new EdgeEffectParameters
            {
                Radius = 15,
                Colour = colours.Yellow,
                Hollow = true,
                Type = EdgeEffectType.Glow
            }, 250, Easing.OutQuint);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            texture?.FadeColour(Color4.Gray, 250, Easing.OutQuint);

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

    public struct Creator
    {
        public string URL { get; set; }

        public string Name { get; set; }
    }
}
