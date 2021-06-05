using osu.Game.Beatmaps;
using osu.Game.Replays;
using osu.Game.Rulesets.Sandbox.Beatmaps;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Sandbox.Replays
{
    internal class SandboxAutoGenerator : AutoGenerator
    {
        public new SandboxBeatmap Beatmap => (SandboxBeatmap)base.Beatmap;

        public SandboxAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
            Replay = new Replay();
        }

        protected Replay Replay;

        public override Replay Generate()
        {
            Replay.Frames.Add(new SandboxReplayFrame(0));
            return Replay;
        }
    }
}
