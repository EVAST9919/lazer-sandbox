using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;

namespace osu.Game.Rulesets.Sandbox.Graphics
{
    public class SandboxProgressRing : Sprite
    {
        private float progress;

        public float Progress
        {
            get => progress;
            set
            {
                progress = value;
                Invalidate(Invalidation.DrawNode);
            }
        }

        private float innerRadius = 1;

        /// <summary>
        /// The inner fill radius, relative to the <see cref="Drawable.DrawSize"/> of the <see cref="CircularProgress"/>.
        /// The value range is 0 to 1 where 0 is invisible and 1 is completely filled.
        /// The entire texture still fills the disk without cropping it.
        /// </summary>
        public float InnerRadius
        {
            get => innerRadius;
            set
            {
                innerRadius = Math.Clamp(value, 0, 1);
                Invalidate(Invalidation.DrawNode);
            }
        }

        public SandboxProgressRing()
        {
            Texture = Texture.WhitePixel;
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders)
        {
            TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "ring");
            RoundedTextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "ring");
        }

        protected override DrawNode CreateDrawNode() => new CircularProgressDrawNode(this);

        private class CircularProgressDrawNode : SpriteDrawNode
        {
            public new SandboxProgressRing Source => (SandboxProgressRing)base.Source;

            public CircularProgressDrawNode(SandboxProgressRing source)
                : base(source)
            {
            }

            private float innerRadius;
            private float progress;
            private float texel;

            public override void ApplyState()
            {
                base.ApplyState();

                innerRadius = Source.innerRadius;
                progress = Math.Abs(Source.progress);
                texel = 1f / Source.ScreenSpaceDrawQuad.Size.X;
            }

            protected override void Blit(Action<TexturedVertex2D> vertexAction)
            {
                Shader.GetUniform<float>("innerRadius").UpdateValue(ref innerRadius);
                Shader.GetUniform<float>("progress").UpdateValue(ref progress);
                Shader.GetUniform<float>("texel").UpdateValue(ref texel);

                base.Blit(vertexAction);
            }
        }
    }
}
