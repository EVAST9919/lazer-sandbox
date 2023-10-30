using osu.Framework.Allocation;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osuTK;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Visualizers.Linear
{
    public partial class RoundedLinearMusicVisualizerDrawable : LinearMusicVisualizerDrawable
    {
        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders)
        {
            Shader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "RoundedBar");
        }

        protected override LinearVisualizerDrawNode CreateLinearVisualizerDrawNode() => new RoundedDrawNode(this);

        private class RoundedDrawNode : LinearVisualizerDrawNode
        {
            public RoundedDrawNode(LinearMusicVisualizerDrawable source)
                : base(source)
            {
            }

            protected override void DrawBar(Vector2 barPosition, Vector2 barSize, IRenderer renderer)
            {
                var adjustedSize = barSize + new Vector2(0, barSize.X);

                Quad drawQuad = GetDrawQuad(barPosition, adjustedSize);

                renderer.DrawQuad(
                    Texture,
                    drawQuad,
                    DrawColourInfo.Colour,
                    new RectangleF(Vector2.Zero, drawQuad.Size),
                    VertexBatch.AddAction
                );
            }
        }
    }
}
