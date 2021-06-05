using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Sandbox.Judgements
{
    public class SandboxJudgement : Judgement
    {
        public override HitResult MaxResult => HitResult.Perfect;
    }
}
