using System;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
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
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.Main.Components
{
    public partial class SandboxPanel : CompositeDrawable
    {
        public static readonly float WIDTH = 300;
        private const float reflection_idle = -200;
        private const float reflection_active = -100;

        public Action Action;

        [Resolved]
        private OsuColour colours { get; set; }

        private readonly string name;
        private readonly Creator? creator;
        private Box reflection;

        public SandboxPanel(string name, Creator? creator = null)
        {
            this.name = name;
            this.creator = creator;
        }

        private Sprite texture;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Y;
            Width = WIDTH;
            Masking = true;
            CornerRadius = 3;
            EdgeEffect = idle_edge_effect;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f
                },
                new Container
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
                },
                reflection = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Rotation = 80,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.TopCentre,
                    Y = reflection_idle,
                    Alpha = 0.2f,
                    EdgeSmoothness = Vector2.One
                }
            };

            if (!creator.HasValue)
                return;

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

        protected override bool OnHover(HoverEvent e)
        {
            texture?.FadeColour(Color4.DarkGray, 250, Easing.OutQuint);
            reflection.MoveToY(reflection_active, 250, Easing.OutQuint);
            TweenEdgeEffectTo(hover_edge_effect, 250, Easing.OutQuint);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            texture?.FadeColour(Color4.Gray, 250, Easing.OutQuint);
            reflection.MoveToY(reflection_idle, 250, Easing.OutQuint);
            TweenEdgeEffectTo(idle_edge_effect, 250, Easing.OutQuint);
        }

        protected override bool OnClick(ClickEvent e)
        {
            Action?.Invoke();
            return true;
        }

        private static readonly EdgeEffectParameters idle_edge_effect = new EdgeEffectParameters
        {
            Radius = 1,
            Colour = Color4.Black.Opacity(0.5f),
            Type = EdgeEffectType.Shadow,
            Offset = Vector2.Zero
        };

        private static readonly EdgeEffectParameters hover_edge_effect = new EdgeEffectParameters
        {
            Radius = 20,
            Colour = Color4.Black.Opacity(0.5f),
            Type = EdgeEffectType.Shadow,
            Offset = new Vector2(0, 5)
        };
    }

    public struct Creator
    {
        public string URL { get; set; }

        public string Name { get; set; }
    }
}
