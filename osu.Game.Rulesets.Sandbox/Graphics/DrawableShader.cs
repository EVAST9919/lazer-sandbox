using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Shaders;
using System;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Sandbox.Graphics
{
    public abstract class DrawableShader : Drawable
    {
        [Resolved]
        private Framework.Game game { get; set; }

        private readonly string shaderName;
        private IShader shader;

        protected DrawableShader(string shaderName)
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
            protected new DrawableShader Source => (DrawableShader)base.Source;

            private IShader shader;

            private Quad screenSpaceDrawQuad;
            private Quad gameQuad;

            public ShaderDrawNode(DrawableShader source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                shader = Source.shader;
                screenSpaceDrawQuad = Source.ScreenSpaceDrawQuad;
                gameQuad = Source.game.ScreenSpaceDrawQuad;
            }

            public override void Draw(Action<TexturedVertex2D> vertexAction)
            {
                base.Draw(vertexAction);
                Draw(screenSpaceDrawQuad, vertexAction);
            }

            protected virtual void Draw(Quad screenSpaceDrawQuad, Action<TexturedVertex2D> vertexAction)
            {
                if (shader == null)
                    return;

                if (!gameQuad.AABBFloat.IntersectsWith(screenSpaceDrawQuad.AABBFloat))
                    return;

                shader.Bind();

                UpdateUniforms(shader);

                DrawQuad(TextureToDraw, screenSpaceDrawQuad, DrawColourInfo.Colour, vertexAction: vertexAction);

                shader.Unbind();
            }

            protected virtual Texture TextureToDraw => Texture.WhitePixel;

            protected virtual void UpdateUniforms(IShader shader)
            {
            }
        }
    }
}
