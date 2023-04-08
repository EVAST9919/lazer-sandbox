﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace osu.Game.Rulesets.Sandbox.Screens.FlappyDon.Components
{
    public partial class BackgroundSprite : Sprite
    {
        private Vector2 textureSize;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Texture = textures.Get("FlappyDon/bg", WrapMode.ClampToBorder, WrapMode.ClampToBorder);
            textureSize = Texture.Size;
            RelativeSizeAxes = Axes.Y;
            Height = 1.0f;
            EdgeSmoothness = Vector2.One;
        }

        protected override void Update()
        {
            base.Update();
            Width = DrawHeight * textureSize.X / textureSize.Y;
        }
    }
}
