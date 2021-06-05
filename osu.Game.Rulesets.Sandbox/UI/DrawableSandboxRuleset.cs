using System.Collections.Generic;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Sandbox.Objects;
using osu.Game.Rulesets.Sandbox.Objects.Drawables;
using osu.Game.Rulesets.Sandbox.Replays;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osu.Game.Users;

namespace osu.Game.Rulesets.Sandbox.UI
{
    public class DrawableSandboxRuleset : DrawableRuleset<SandboxHitObject>
    {
        public DrawableSandboxRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
        }

        protected override PassThroughInputManager CreateInputManager() => new SandboxInputManager(Ruleset.RulesetInfo);

        protected override Playfield CreatePlayfield() => new SandboxPlayfield();

        protected override ReplayRecorder CreateReplayRecorder(Score score) => new SandboxReplayRecorder();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new SandboxFramedReplayInputHandler(replay);

        public override DrawableHitObject<SandboxHitObject> CreateDrawableRepresentation(SandboxHitObject h)
        {
            switch (h)
            {
                case SandboxHitObject hitObject:
                    return new DrawableSandboxHitObject(hitObject);
            }

            return null;
        }

        protected override void LoadComplete()
        {
            SetReplayScore(new Score
            {
                ScoreInfo = new ScoreInfo { User = new User { Username = "Sandbox" } },
                Replay = new SandboxAutoGenerator(Beatmap).Generate(),
            });

            base.LoadComplete();
        }
    }
}
