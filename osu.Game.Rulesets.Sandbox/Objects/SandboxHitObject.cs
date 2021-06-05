using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Sandbox.Judgements;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Sandbox.Objects
{
    public class SandboxHitObject : HitObject
    {
        protected override HitWindows CreateHitWindows() => HitWindows.Empty;

        public override Judgement CreateJudgement() => new SandboxJudgement();
    }
}
