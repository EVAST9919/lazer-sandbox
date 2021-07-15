using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Shaders;
using System;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Sandbox.Graphics
{
    public class ShaderContainer : Drawable
    {
        private IShader shader;

        private readonly string shaderName;

        public ShaderContainer(string shaderName)
        {
            this.shaderName = shaderName;
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders)
        {
            shader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, shaderName);
        }

        protected override void Update()
        {
            base.Update();
            Invalidate(Invalidation.DrawNode);
        }

        protected override DrawNode CreateDrawNode() => CreateShaderDrawNode();

        protected virtual ShaderDrawNode CreateShaderDrawNode() => new ShaderDrawNode(this);

        protected class ShaderDrawNode : DrawNode
        {
            protected new ShaderContainer Source => (ShaderContainer)base.Source;

            private IShader shader;

            private Quad screenSpaceDrawQuad;

            public ShaderDrawNode(ShaderContainer source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                shader = Source.shader;
                screenSpaceDrawQuad = Source.ScreenSpaceDrawQuad;
            }

            public override void Draw(Action<TexturedVertex2D> vertexAction)
            {
                base.Draw(vertexAction);

                if (shader == null)
                    return;

                shader.Bind();

                UpdateUniforms(shader);

                DrawQuad(Texture.WhitePixel, screenSpaceDrawQuad, DrawColourInfo.Colour);

                shader.Unbind();
            }

            protected virtual void UpdateUniforms(IShader shader)
            {
            }
        }
    }
}
