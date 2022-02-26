using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Utils;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Sandbox.Extensions;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Tests.TestRulesets
{
    public class TestSceneProjectileRuleset : TestRulesetTestScene
    {
        protected override TestPlayfield CreateTestPlayfield() => new ProjectilePlayfield();

        private class ProjectilePlayfield : TestPlayfield
        {
            private readonly Container content;
            private readonly List<ProjectileTimingPoint> hitObjects = new();

            public ProjectilePlayfield()
            {
                Children = new Drawable[]
                {
                    content = new Container
                    {
                        Masking = true,
                        BorderThickness = 5f,
                        BorderColour = Color4.White,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both
                    },
                    new Circle
                    {
                        Size = new Vector2(10),
                        Colour = Color4.Red,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    }
                };
            }

            protected override void OnBeatmapChanged(ValueChangedEvent<WorkingBeatmap> beatmap)
            {
                content.Clear();
                hitObjects.Clear();

                var ogHitObjects = beatmap.NewValue.Beatmap.HitObjects;

                if (!ogHitObjects.Any())
                    return;

                hitObjects.Add(new ProjectileTimingPoint
                {
                    StartTime = ogHitObjects[0].StartTime - 5000,
                    Info = new ProjectileInfo(Vector2.Zero, 0, 90)
                });

                foreach (var h in ogHitObjects)
                    hitObjects.Add(new ProjectileTimingPoint { StartTime = h.StartTime });

                if (hitObjects.Count < 2)
                    return;

                for (int i = 0; i < hitObjects.Count - 1; i++)
                {
                    hitObjects[i].Duration = hitObjects[i + 1].StartTime - hitObjects[i].StartTime;
                }

                for (int i = 1; i < hitObjects.Count; i++)
                {
                    var prev = hitObjects[i - 1];
                    var collisionInfo = ProjectileExtensions.GetProjectileInfoAt(prev.Info, (float)prev.Duration.Value);
                    var normal = ProjectileExtensions.GetAcceptableAngle(ProjectileExtensions.GetOppositeAngle(collisionInfo.Angle) + RNG.NextSingle(-60f, 60f));
                    var (info, collided) = ProjectileExtensions.ProcessCollision(collisionInfo, normal);
                    hitObjects[i].Info = info;

                    content.Add(new DHO
                    {
                        InitialPosition = collisionInfo.Position,
                        StartTime = hitObjects[i].StartTime,
                        Rotation = normal,
                        Colour = collided ? Color4.White : Color4.Gray
                    });
                }
            }

            protected override void Update()
            {
                base.Update();

                if (hitObjects.Count < 2)
                    return;

                double currentTime = Beatmap.Value.Track.CurrentTime;

                if (currentTime < hitObjects[0].StartTime)
                    return;

                ProjectileTimingPoint closest = null;
                double offset = double.PositiveInfinity;

                foreach (var tp in hitObjects)
                {
                    if (tp.StartTime > currentTime)
                        continue;

                    if (currentTime - tp.StartTime < offset)
                    {
                        closest = tp;
                        offset = currentTime - tp.StartTime;
                    }
                }

                if (closest == null)
                    return;

                if (!closest.Duration.HasValue)
                    return;

                float currentProgress = (float)Math.Clamp(offset % closest.Duration.Value / closest.Duration.Value, 0f, 1f);

                var cameraOffset = ProjectileExtensions.GetProjectileInfoAt(closest.Info, (float)closest.Duration.Value * currentProgress).Position;

                foreach (var h in content)
                {
                    var dho = (DHO)h;

                    h.Alpha = currentTime < dho.StartTime - 1000 ? 0f : (currentTime > dho.StartTime ? 0f : 1f);
                    h.Position = dho.InitialPosition - cameraOffset;
                }
            }

            private class DHO : Box
            {
                public Vector2 InitialPosition { get; set; }

                public double StartTime { get; set; }

                public DHO()
                {
                    Origin = Anchor.Centre;
                    Anchor = Anchor.Centre;
                    Height = 30;
                    Width = 2;
                    EdgeSmoothness = Vector2.One;
                }
            }
        }

        private class ProjectileTimingPoint
        {
            public double StartTime { get; set; }

            public double? Duration { get; set; }

            public ProjectileInfo Info { get; set; }
        }
    }
}
