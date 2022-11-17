using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Tests.Visual;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Tests.Graphics
{
    public class TestSceneShape : OsuTestScene
    {
        private readonly Shape shape;
        private readonly ShapeAnchor topLeftAnchor;
        private readonly ShapeAnchor topRightAnchor;
        private readonly ShapeAnchor bottomLeftAnchor;
        private readonly ShapeAnchor bottomRightAnchor;

        public TestSceneShape()
        {
            AddRange(new Drawable[]
            {
                shape = new Shape(),
                topLeftAnchor = new ShapeAnchor { Position = new Vector2(100), Dragged = updateShape },
                topRightAnchor = new ShapeAnchor { Position = new Vector2(100, 200), Dragged = updateShape },
                bottomLeftAnchor = new ShapeAnchor { Position = new Vector2(200, 100), Dragged = updateShape },
                bottomRightAnchor = new ShapeAnchor { Position = new Vector2(200), Dragged = updateShape },
            });

            updateShape();
        }

        private void updateShape()
        {
            shape.UpdatePositions(topLeftAnchor.Position, bottomLeftAnchor.Position, topRightAnchor.Position, bottomRightAnchor.Position);
        }

        private class ShapeAnchor : Circle
        {
            public Action Dragged;

            public ShapeAnchor()
            {
                Origin = Anchor.Centre;
                Size = new Vector2(15);
                Colour = Color4.Red;
            }

            protected override bool OnDragStart(DragStartEvent e) => true;

            protected override void OnDrag(DragEvent e)
            {
                base.OnDrag(e);

                Position = e.MousePosition;
                Dragged?.Invoke();
            }
        }

        private class Shape : Drawable, ITexturedShaderDrawable
        {
            public IShader TextureShader { get; private set; }

            public Shape()
            {
                RelativeSizeAxes = Axes.Both;
            }

            [BackgroundDependencyLoader]
            private void load(ShaderManager shaders)
            {
                TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
            }

            private Vector2 topLeft, topRight, bottomLeft, bottomRight;

            public void UpdatePositions(Vector2 topLeft, Vector2 bottomLeft, Vector2 topRight, Vector2 bottomRight)
            {
                this.topLeft = topLeft;
                this.bottomLeft = bottomLeft;
                this.topRight = topRight;
                this.bottomRight = bottomRight;

                Invalidate(Invalidation.DrawNode);
            }

            protected override DrawNode CreateDrawNode() => new ShapeDrawNode(this);

            private class ShapeDrawNode : TexturedShaderDrawNode
            {
                protected new Shape Source => (Shape)base.Source;

                public ShapeDrawNode(Shape source)
                    : base(source)
                {
                }

                private Vector2 topLeft, topRight, bottomLeft, bottomRight;

                public override void ApplyState()
                {
                    base.ApplyState();

                    topLeft = Source.topLeft;
                    bottomLeft = Source.bottomLeft;
                    topRight = Source.topRight;
                    bottomRight = Source.bottomRight;
                }

                public override void Draw(IRenderer renderer)
                {
                    base.Draw(renderer);

                    var texture = renderer.WhitePixel;

                    TextureShader.Bind();

                    texture.Bind();

                    renderer.DrawQuad(texture, new Quad(
                        Vector2Extensions.Transform(topLeft, DrawInfo.Matrix),
                        Vector2Extensions.Transform(topRight, DrawInfo.Matrix),
                        Vector2Extensions.Transform(bottomLeft, DrawInfo.Matrix),
                        Vector2Extensions.Transform(bottomRight, DrawInfo.Matrix)), DrawColourInfo.Colour);

                    TextureShader.Unbind();
                }
            }
        }
    }
}
