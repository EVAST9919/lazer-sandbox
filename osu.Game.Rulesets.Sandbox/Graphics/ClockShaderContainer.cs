using osu.Framework.Graphics.Shaders;

namespace osu.Game.Rulesets.Sandbox.Graphics
{
    public class ClockShaderContainer : ShaderContainer
    {
        private float currentTime;
        private float loadTime;

        public ClockShaderContainer(string shaderName)
            : base(shaderName)
        {
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            loadTime = (float)Clock.CurrentTime;
        }

        protected override void Update()
        {
            currentTime = (float)(Clock.CurrentTime - loadTime) / 1000;
            base.Update();
        }

        protected override ShaderDrawNode CreateShaderDrawNode() => new ClockShaderDrawNode(this);

        private class ClockShaderDrawNode : ShaderDrawNode
        {
            protected new ClockShaderContainer Source => (ClockShaderContainer)base.Source;

            public ClockShaderDrawNode(ClockShaderContainer source)
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
