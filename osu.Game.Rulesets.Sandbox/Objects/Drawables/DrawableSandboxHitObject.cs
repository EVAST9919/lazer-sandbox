using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Sandbox.Objects.Drawables
{
    public class DrawableSandboxHitObject : DrawableHitObject<SandboxHitObject>
    {
        protected override double InitialLifetimeOffset => 500;

        public DrawableSandboxHitObject(SandboxHitObject h)
            : base(h)
        {
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset > 0)
                ApplyResult(r => r.Type = r.Judgement.MaxResult);
        }
    }
}
