﻿using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.Shooter
{
    public class ShooterPlayer : CompositeDrawable
    {
        public ShooterPlayer()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Size = new Vector2(30);
            InternalChildren = new Drawable[]
            {
                new Circle
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.White
                },
                new Circle
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Size = new Vector2(10),
                    Colour = Color4.Black
                }
            };
        }
    }
}
