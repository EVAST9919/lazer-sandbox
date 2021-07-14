using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Shaders;
using System;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Textures;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Sandbox.Graphics
{
    public class ShaderContainer : Drawable
    {
        public readonly Bindable<string> ShaderName = new Bindable<string>();

        [Resolved]
        private ShaderManager shaders { get; set; }

        private IShader shader;

        protected override void LoadComplete()
        {
            base.LoadComplete();
            ShaderName.BindValueChanged(sh => updateShader(sh.NewValue), true);
        }

        private void updateShader(string shaderName)
        {
            shader = string.IsNullOrEmpty(shaderName) ? null : shaders.Load(VertexShaderDescriptor.TEXTURE_2, shaderName);
            shaderLoadTime = (float)Clock.CurrentTime;
            Invalidate(Invalidation.DrawNode);
        }

        private float currentTime;
        private float shaderLoadTime;

        protected override void Update()
        {
            base.Update();

            currentTime = (float)(Clock.CurrentTime - shaderLoadTime) / 1000;
            Invalidate(Invalidation.DrawNode);
        }

        protected override DrawNode CreateDrawNode() => new ShaderDrawNode(this);

        private class ShaderDrawNode : DrawNode
        {
            private ShaderContainer source => (ShaderContainer)Source;

            private IShader shader;
            private Quad screenSpaceDrawQuad;

            public ShaderDrawNode(ShaderContainer source)
                : base(source)
            {
            }

            private float time;

            public override void ApplyState()
            {
                base.ApplyState();

                shader = source.shader;
                screenSpaceDrawQuad = source.ScreenSpaceDrawQuad;
                time = source.currentTime;
            }

            public override void Draw(Action<TexturedVertex2D> vertexAction)
            {
                base.Draw(vertexAction);

                if (shader == null)
                    return;

                shader.Bind();

                shader.GetUniform<float>("time").UpdateValue(ref time);

                DrawQuad(Texture.WhitePixel, screenSpaceDrawQuad, DrawColourInfo.Colour);

                shader.Unbind();
            }
        }
    }
}
