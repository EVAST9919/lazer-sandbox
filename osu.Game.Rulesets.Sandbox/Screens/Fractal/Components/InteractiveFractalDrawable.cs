using osu.Game.Rulesets.Sandbox.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Shaders;

namespace osu.Game.Rulesets.Sandbox.Screens.Fractal.Components
{
    public class InteractiveFractalDrawable : ShaderContainer
    {
        public InteractiveFractalDrawable()
            : base("fractalInteractive")
        {
            RelativeSizeAxes = Axes.Both;
        }

        private float scale = 1f;

        protected override bool OnScroll(ScrollEvent e)
        {
            base.OnScroll(e);

            scale += (e.ScrollDelta.Y > 0 ? 1 : -1) * scale * 0.01f;

            if (scale < 1f)
                scale = 1f;

            return true;
        }

        protected override ShaderDrawNode CreateShaderDrawNode() => new FractalDrawNode(this);

        private class FractalDrawNode : ShaderDrawNode
        {
            public new InteractiveFractalDrawable Source => (InteractiveFractalDrawable)base.Source;

            public FractalDrawNode(InteractiveFractalDrawable source)
                : base(source)
            {
            }

            private float scale = 0;

            public override void ApplyState()
            {
                base.ApplyState();

                scale = Source.scale - 1;
            }

            protected override void UpdateUniforms(IShader shader)
            {
                shader.GetUniform<float>("scale").UpdateValue(ref scale);
            }
        }
    }
}
