using osu.Framework.Graphics.Shaders;

namespace osu.Game.Rulesets.Sandbox.Graphics
{
    public class TimedDrawableShader : DrawableShader
    {
        private float currentTime;

        public TimedDrawableShader(string shaderName)
            : base(shaderName)
        {
        }

        protected override void Update()
        {
            currentTime = (float)Clock.CurrentTime / 1000;
            base.Update();
        }

        protected override ShaderDrawNode CreateShaderDrawNode() => new ClockShaderDrawNode(this);

        protected class ClockShaderDrawNode : ShaderDrawNode
        {
            protected new TimedDrawableShader Source => (TimedDrawableShader)base.Source;

            public ClockShaderDrawNode(TimedDrawableShader source)
                : base(source)
            {
            }

            private float currentTime;

            public override void ApplyState()
            {
                base.ApplyState();
                currentTime = Source.currentTime;
            }

            protected override void UpdateUniforms(IShader shader)
            {
                shader.GetUniform<float>("time").UpdateValue(ref currentTime);
            }
        }
    }
}
