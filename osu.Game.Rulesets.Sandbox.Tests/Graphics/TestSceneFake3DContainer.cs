using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Tests.Graphics
{
    public partial class TestSceneFake3DContainer : RulesetTestScene
    {
        public TestSceneFake3DContainer()
        {
            Fake3DContainer container;

            Add(container = new Fake3DContainer
            {
                RelativeSizeAxes = Axes.Both
            });

            for (int i = 0; i < 500; i++)
            {
                float depth = RNG.Next(-100, 1000);
                container.Add(new Fake3DEntity
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Size = new Vector2(RNG.Next(10, 100)),
                    PositionInVolume = new Vector3(RNG.Next(-2000, 2000), RNG.Next(-2000, 2000), depth),
                    Depth = depth,
                    Colour = getRandomColour()
                });
            }
        }

        private Color4 getRandomColour() => new Color4(RNG.NextSingle(0.5f, 1f), RNG.NextSingle(0.5f, 1f), RNG.NextSingle(0.5f, 1f), 1f);

        private partial class Fake3DContainer : CompositeDrawable
        {
            public readonly Bindable<Vector3> CameraPosition = new Bindable<Vector3>(new Vector3(0, 0, -100));

            protected override void LoadComplete()
            {
                base.LoadComplete();
                CameraPosition.BindValueChanged(c => updateChildren(c.NewValue), true);
            }

            private void updateChildren(Vector3 cameraPosition)
            {
                foreach (var c in InternalChildren)
                {
                    if (c is not Fake3DEntity e)
                        continue;

                    e.UpdateWithCamera(cameraPosition);
                }
            }

            public void Add(Fake3DEntity e)
            {
                e.UpdateWithCamera(CameraPosition.Value);
                AddInternal(e);
            }

            protected override bool OnScroll(ScrollEvent e)
            {
                CameraPosition.Value = new Vector3(CameraPosition.Value.X, CameraPosition.Value.Y, CameraPosition.Value.Z + e.ScrollDelta.Y * 10);
                return true;
            }

            protected override bool OnDragStart(DragStartEvent e) => true;

            protected override void OnDrag(DragEvent e)
            {
                base.OnDrag(e);

                CameraPosition.Value = new Vector3(CameraPosition.Value.X - e.Delta.X, CameraPosition.Value.Y - e.Delta.Y, CameraPosition.Value.Z);
            }
        }

        private partial class Fake3DEntity : Container
        {
            private Vector3 positionInVolume;

            public Vector3 PositionInVolume
            {
                get => positionInVolume;
                set
                {
                    if (positionInVolume == value)
                        return;

                    positionInVolume = value;

                    if (lastCameraPosition.HasValue)
                        UpdateWithCamera(lastCameraPosition.Value);
                }
            }

            public Fake3DEntity()
            {
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both
                };
            }

            private Vector3? lastCameraPosition;

            public void UpdateWithCamera(Vector3 cameraPosition)
            {
                lastCameraPosition = cameraPosition;

                if (positionInVolume.Z <= cameraPosition.Z)
                {
                    Alpha = 0;
                    return;
                }

                var distance = positionInVolume.Z - cameraPosition.Z;
                var mappedDepth = 2f / (distance / 100);

                Scale = new Vector2(mappedDepth);
                Position = (positionInVolume.Xy - cameraPosition.Xy) * mappedDepth;
                Alpha = 1;
            }
        }
    }
}
