using osu.Game.Rulesets.Sandbox.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Shaders;
using osuTK;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Sandbox.Screens.Fractal.Components
{
    public class InteractiveFractalDrawable : ShaderContainer
    {
        public readonly Bindable<float> ZoomLevel = new Bindable<float>(min_scale);

        private const float min_scale = 0.3f;

        public InteractiveFractalDrawable()
            : base("fractalInteractive")
        {
            RelativeSizeAxes = Axes.Both;
        }

        private Vector2 cameraPosition = Vector2.Zero;

        protected override bool OnScroll(ScrollEvent e)
        {
            base.OnScroll(e);

            ZoomLevel.Value += (e.ScrollDelta.Y > 0 ? 1 : -1) * ZoomLevel.Value * 0.1f;

            if (ZoomLevel.Value < min_scale)
                ZoomLevel.Value = min_scale;

            return true;
        }

        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
        {
            base.OnDrag(e);
            cameraPosition -= Vector2.Divide(e.Delta, DrawSize) / ZoomLevel.Value;
        }

        protected override ShaderDrawNode CreateShaderDrawNode() => new FractalDrawNode(this);

        private class FractalDrawNode : ShaderDrawNode
        {
            public new InteractiveFractalDrawable Source => (InteractiveFractalDrawable)base.Source;

            public FractalDrawNode(InteractiveFractalDrawable source)
                : base(source)
            {
            }

            private float scale;
            private Vector2 cameraPosition;
            private Vector2 drawSize;

            public override void ApplyState()
            {
                base.ApplyState();

                scale = Source.ZoomLevel.Value;
                cameraPosition = Source.cameraPosition;
                drawSize = Source.DrawSize;
            }

            protected override void UpdateUniforms(IShader shader)
            {
                shader.GetUniform<float>("scale").UpdateValue(ref scale);
                shader.GetUniform<Vector2>("cameraPosition").UpdateValue(ref cameraPosition);
                shader.GetUniform<Vector2>("drawSize").UpdateValue(ref drawSize);
            }
        }
    }
}
