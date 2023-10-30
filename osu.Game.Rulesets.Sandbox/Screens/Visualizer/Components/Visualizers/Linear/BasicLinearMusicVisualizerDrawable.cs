using osu.Framework.Graphics.Rendering;
using osuTK;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Visualizers.Linear
{
    public partial class BasicLinearMusicVisualizerDrawable : LinearMusicVisualizerDrawable
    {
        protected override LinearVisualizerDrawNode CreateLinearVisualizerDrawNode() => new BasicDrawNode(this);

        private class BasicDrawNode : LinearVisualizerDrawNode
        {
            public BasicDrawNode(LinearMusicVisualizerDrawable source)
                : base(source)
            {
            }

            protected override void DrawBar(Vector2 barPosition, Vector2 barSize, IRenderer renderer)
            {
                var adjustedSize = barSize + new Vector2(0, 2);

                renderer.DrawQuad(
                    Texture,
                    GetDrawQuad(barPosition, adjustedSize),
                    DrawColourInfo.Colour,
                    null,
                    VertexBatch.AddAction,
                    new Vector2(1f / barSize.Y, 1f / barSize.X));
            }
        }
    }
}
