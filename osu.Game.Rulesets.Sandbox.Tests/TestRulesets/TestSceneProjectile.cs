using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Sandbox.Extensions;
using osu.Game.Tests.Visual;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Tests.TestRulesets
{
    public class TestSceneProjectile : OsuTestScene
    {
        private readonly Container pathContainer;
        private readonly Box collisionSurface;
        private readonly Box drawableNormal;
        private ProjectileInfo infoAtStart;
        private float angle;
        private float surfaceNormal;
        private float speed;
        private double duration = 1.0;
        private Vector2 startPos = Vector2.Zero;

        public TestSceneProjectile()
        {
            AddRange(new Drawable[]
            {
                pathContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
                collisionSurface = new Box
                {
                    Origin = Anchor.Centre,
                    Height = 60,
                    Width = 1,
                    EdgeSmoothness = Vector2.One
                },
                drawableNormal = new Box
                {
                    Origin = Anchor.CentreLeft,
                    Height = 1,
                    Width = 30,
                    EdgeSmoothness = Vector2.One,
                    Colour = Color4.Red
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddSliderStep("Angle", -270f, 90f, 0f, a =>
            {
                angle = a;
                updatePath();
            });
            AddSliderStep("Speed", 0f, 20f, 10f, s =>
            {
                speed = s;
                updatePath();
            });
            AddSliderStep("Duration", 1.0, 10000.0, 5000.0, d =>
            {
                duration = d;
                updatePath();
            });

            AddSliderStep("Surface normal", -270f, 90f, -90f, sn =>
            {
                surfaceNormal = sn;
                collisionSurface.Rotation = drawableNormal.Rotation = sn;
                updatePath();
            });

            AddSliderStep("Start x", 100f, 1000f, 100f, x =>
            {
                startPos.X = x;
                updatePath();
            });
            AddSliderStep("Start y", 100f, 1000f, 200f, y =>
            {
                startPos.Y = y;
                updatePath();
            });
        }

        private void updatePath()
        {
            pathContainer.Clear();

            infoAtStart = new ProjectileInfo(startPos, speed, angle);

            for (int i = 0; i <= 100; i++)
            {
                pathContainer.Add(new Circle
                {
                    Size = new Vector2(5),
                    Colour = Color4.White,
                    Position = ProjectileExtensions.GetProjectileInfoAt(infoAtStart, (float)duration * i / 200f).Position,
                    Origin = Anchor.Centre
                });
            }

            var infoBeforeCollision = ProjectileExtensions.GetProjectileInfoAt(infoAtStart, (float)duration * 0.5f);

            collisionSurface.Position = drawableNormal.Position = infoBeforeCollision.Position;

            var (info, collided) = ProjectileExtensions.ProcessCollision(infoBeforeCollision, surfaceNormal);

            for (int i = 0; i <= 100; i++)
            {
                pathContainer.Add(new Circle
                {
                    Size = new Vector2(5),
                    Colour = collided ? Color4.White : Color4.Gray,
                    Position = ProjectileExtensions.GetProjectileInfoAt(info, (float)duration * i / 200f).Position,
                    Origin = Anchor.Centre
                });
            }
        }
    }
}
