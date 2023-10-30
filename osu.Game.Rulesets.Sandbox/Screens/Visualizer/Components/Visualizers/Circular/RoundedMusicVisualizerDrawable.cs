using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osuTK;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Visualizers.Circular
{
    public partial class RoundedMusicVisualizerDrawable : CircularMusicVisualizerDrawable
    {
        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders)
        {
            Shader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "RoundedBar");
        }

        protected override CircularVisualizerDrawNode CreateCircularVisualizerDrawNode() => new RoundedVisualizerDrawNode(this);

        private class RoundedVisualizerDrawNode : CircularVisualizerDrawNode
        {
            public RoundedVisualizerDrawNode(CircularMusicVisualizerDrawable source)
                : base(source)
            {
            }

            protected override void DrawBar(int index, float data, float spacing, Vector2 inflation, IRenderer renderer)
            {
                float rotation = MathHelper.DegreesToRadians(index * spacing - 90);
                float rotationCos = MathF.Cos(rotation);
                float rotationSin = MathF.Sin(rotation);

                var barPosition = new Vector2(rotationCos / 2 + 0.5f, rotationSin / 2 + 0.5f) * Size.X;
                var barSize = new Vector2((float)BarWidth, (float)BarWidth + data);

                var bottomOffset = new Vector2(-rotationSin * barSize.X / 2, rotationCos * barSize.X / 2);
                var amplitudeOffset = new Vector2(rotationCos * barSize.Y, rotationSin * barSize.Y);

                var drawQuad = new Quad(
                    Vector2Extensions.Transform(barPosition - bottomOffset, DrawInfo.Matrix),
                    Vector2Extensions.Transform(barPosition - bottomOffset + amplitudeOffset, DrawInfo.Matrix),
                    Vector2Extensions.Transform(barPosition + bottomOffset, DrawInfo.Matrix),
                    Vector2Extensions.Transform(barPosition + bottomOffset + amplitudeOffset, DrawInfo.Matrix)
                );

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
