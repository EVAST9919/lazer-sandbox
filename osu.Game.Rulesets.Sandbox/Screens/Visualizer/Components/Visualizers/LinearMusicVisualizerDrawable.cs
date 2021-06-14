using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osuTK;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Visualizers
{
    public class LinearMusicVisualizerDrawable : MusicVisualizerDrawable
    {
        public readonly Bindable<BarAnchor> BarAnchorBindable = new Bindable<BarAnchor>(BarAnchor.Centre);

        public LinearMusicVisualizerDrawable()
        {
            RelativeSizeAxes = Axes.Both;
        }

        protected override VisualizerDrawNode CreateVisualizerDrawNode() => new LinearVisualizerDrawNode(this);

        private class LinearVisualizerDrawNode : VisualizerDrawNode
        {
            public new LinearMusicVisualizerDrawable Source => (LinearMusicVisualizerDrawable)base.Source;

            private BarAnchor origin;

            public LinearVisualizerDrawNode(LinearMusicVisualizerDrawable source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();
                origin = Source.BarAnchorBindable.Value;
            }

            protected override float Spacing => Size.X / AudioData.Count;

            private float barY;

            protected override void PreCompute()
            {
                base.PreCompute();
                barY = getBarPositionY();
            }

            protected override void DrawBar(int index, float data, float spacing, Vector2 inflation)
            {
                var barPosition = new Vector2(index * spacing, barY);
                var barSize = new Vector2((float)BarWidth, 2 + data);

                DrawQuad(
                    Texture,
                    getQuad(barPosition, barSize),
                    DrawColourInfo.Colour,
                    null,
                    VertexBatch.AddAction,
                    new Vector2(1f / barSize.Y, 1f / barSize.X));
            }

            private float getBarPositionY()
            {
                switch (origin)
                {
                    default:
                    case BarAnchor.Bottom:
                        return Size.Y;

                    case BarAnchor.Centre:
                        return Size.Y / 2;

                    case BarAnchor.Top:
                        return 0;
                }
            }

            private Quad getQuad(Vector2 barPosition, Vector2 barSize)
            {
                switch (origin)
                {
                    default:
                    case BarAnchor.Bottom:
                        return new Quad(
                            Vector2Extensions.Transform(barPosition, DrawInfo.Matrix),
                            Vector2Extensions.Transform(barPosition + new Vector2(0, -barSize.Y), DrawInfo.Matrix),
                            Vector2Extensions.Transform(barPosition + new Vector2(barSize.X, 0), DrawInfo.Matrix),
                            Vector2Extensions.Transform(barPosition + new Vector2(barSize.X, -barSize.Y), DrawInfo.Matrix)
                            );

                    case BarAnchor.Centre:
                        return new Quad(
                            Vector2Extensions.Transform(barPosition + new Vector2(0, -barSize.Y / 2), DrawInfo.Matrix),
                            Vector2Extensions.Transform(barPosition + new Vector2(0, barSize.Y / 2), DrawInfo.Matrix),
                            Vector2Extensions.Transform(barPosition + new Vector2(barSize.X, -barSize.Y / 2), DrawInfo.Matrix),
                            Vector2Extensions.Transform(barPosition + new Vector2(barSize.X, barSize.Y / 2), DrawInfo.Matrix)
                            );

                    case BarAnchor.Top:
                        return new Quad(
                            Vector2Extensions.Transform(barPosition, DrawInfo.Matrix),
                            Vector2Extensions.Transform(barPosition + new Vector2(0, barSize.Y), DrawInfo.Matrix),
                            Vector2Extensions.Transform(barPosition + new Vector2(barSize.X, 0), DrawInfo.Matrix),
                            Vector2Extensions.Transform(barPosition + new Vector2(barSize.X, barSize.Y), DrawInfo.Matrix)
                            );
                }
            }
        }
    }

    public enum BarAnchor
    {
        Top,
        Centre,
        Bottom
    }
}
