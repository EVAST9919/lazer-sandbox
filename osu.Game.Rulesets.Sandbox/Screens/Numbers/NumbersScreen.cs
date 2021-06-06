using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Sandbox.Screens.Numbers.Components;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.Numbers
{
    public class NumbersScreen : SandboxScreen
    {
        private readonly NumbersPlayfield playfield;
        private readonly ScoreContainer currentScore;

        public NumbersScreen()
        {
            AddRangeInternal(new Drawable[]
            {
                new OsuClickableContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.TopCentre,
                    AutoSizeAxes = Axes.Both,
                    CornerRadius = 6,
                    Masking = true,
                    Margin = new MarginPadding { Top = 240 },
                    Action = () => playfield?.Restart(),
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Color4(187, 173, 160, 255)
                        },
                        new OsuSpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Text = @"Restart".ToUpper(),
                            Font = OsuFont.GetFont(size: 25, weight: FontWeight.Bold),
                            Colour = new Color4(119, 110, 101, 255),
                            Shadow = false,
                            Margin = new MarginPadding { Horizontal = 10, Vertical = 10 },
                        }
                    }
                },
                currentScore = new ScoreContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.BottomCentre,
                    Margin = new MarginPadding { Bottom = 240 },
                },
                playfield = new NumbersPlayfield
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                }
            });

            currentScore.Score.BindTo(playfield.Score);
        }

        private class ScoreContainer : CompositeDrawable
        {
            public readonly BindableInt Score = new BindableInt();

            private readonly OsuSpriteText spriteText;

            public ScoreContainer()
            {
                Size = new Vector2(200, 60);
                CornerRadius = 6;
                Masking = true;
                InternalChildren = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = new Color4(187, 173, 160, 255)
                    },
                    spriteText = new OsuSpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = OsuFont.GetFont(size: 40, weight: FontWeight.Bold),
                        Colour = new Color4(119, 110, 101, 255),
                        Shadow = false
                    }
                };
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();
                Score.BindValueChanged(s => spriteText.Text = s.NewValue.ToString(), true);
            }
        }
    }
}
