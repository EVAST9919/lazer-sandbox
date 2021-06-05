using osu.Game.Replays;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Sandbox.Replays
{
    public class SandboxFramedReplayInputHandler : FramedReplayInputHandler<SandboxReplayFrame>
    {
        public SandboxFramedReplayInputHandler(Replay replay)
            : base(replay)
        {
        }
    }
}
