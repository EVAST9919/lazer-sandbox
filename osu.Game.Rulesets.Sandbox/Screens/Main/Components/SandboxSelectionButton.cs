using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.Main.Components
{
    public class SandboxSelectionButton : CompositeDrawable
    {
        public static readonly float WIDTH = 300;

        public Action Action;

        [Resolved]
        private OsuColour colours { get; set; }

        private readonly string name;
        private readonly Creator creator;

        public SandboxSelectionButton(string name, Creator creator = null)
        {
            this.name = name;
            this.creator = creator;
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

            if (creator != null)
            {
                var linkFlow = new LinkFlowContainer(t =>
                {
                    t.Font = OsuFont.GetFont(size: 20);
                })
                {
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Margin = new MarginPadding { Bottom = 50 }
                };

                linkFlow.AddText("Created by ");
                linkFlow.AddLink(creator.Name, creator.URL);

                AddInternal(linkFlow);
            }
        }

        protected override bool OnHover(HoverEvent e)
        {
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

    public class Creator
    {
        public string URL { get; private set; }

        public string Name { get; private set; }

        public Creator(string url, string name)
        {
            URL = url;
            Name = name;
        }
    }
}
