using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Sandbox.Screens.Numbers.Components;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.Numbers
{
    public class NumbersScreen : SandboxScreen
    {
        private readonly NumbersPlayfield playfield;
        private readonly OsuSpriteText scoreText;

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
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.BottomCentre,
                    AutoSizeAxes = Axes.Y,
                    Width = 150,
                    CornerRadius = 6,
                    Masking = true,
                    Margin = new MarginPadding { Bottom = 240 },
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Color4(187, 173, 160, 255)
                        },
                        scoreText = new OsuSpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Font = OsuFont.GetFont(size: 40, weight: FontWeight.Bold),
                            Text = "0",
                            Colour = new Color4(119, 110, 101, 255),
                            Shadow = false,
                            Margin = new MarginPadding { Horizontal = 10, Vertical = 10 },
                        }
                    }
                },
                playfield = new NumbersPlayfield
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            playfield.Score.BindValueChanged(score => scoreText.Text = score.NewValue.ToString(), true);
        }
    }
}
