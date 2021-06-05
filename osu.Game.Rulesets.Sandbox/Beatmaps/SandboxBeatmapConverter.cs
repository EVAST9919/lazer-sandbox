using System.Collections.Generic;
using System.Threading;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Sandbox.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Sandbox.Beatmaps
{
    public class SandboxBeatmapConverter : BeatmapConverter<SandboxHitObject>
    {
        public SandboxBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
        }

        public override bool CanConvert() => true;

        protected override Beatmap<SandboxHitObject> CreateBeatmap() => new SandboxBeatmap();

        protected override IEnumerable<SandboxHitObject> ConvertHitObject(HitObject obj, IBeatmap beatmap, CancellationToken token)
            => new SandboxHitObject
            {
                StartTime = obj.StartTime,
                Samples = obj.Samples
            }.Yield();
    }
}
