using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using System;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Sandbox.UI;
using osu.Game.Rulesets.Sandbox.Beatmaps;
using osu.Game.Rulesets.Sandbox.Difficulty;
using osu.Game.Rulesets.Configuration;
using osu.Game.Configuration;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Sandbox
{
    public class SandboxRuleset : Ruleset
    {
        public static readonly string VERSION = "2021.1211.1";

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => new DrawableSandboxRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new SandboxBeatmapConverter(beatmap, this);

        public override IRulesetConfigManager CreateConfig(SettingsStore settings) => new SandboxRulesetConfigManager(settings, RulesetInfo);

        public override RulesetSettingsSubsection CreateSettings() => new SandboxSettingsSubsection(this);

        public override string Description => "Sandbox";

        public override string ShortName => "sandbox";

        public override string PlayingVerb => "Doing random stuff";

        public override Drawable CreateIcon() => new Sprite
        {
            Texture = new TextureStore(new TextureLoaderStore(CreateResourceStore()), false).Get("Textures/ruleset"),
        };

        public override IEnumerable<Mod> GetModsFor(ModType type) => Array.Empty<Mod>();

        protected override IEnumerable<HitResult> GetValidHitResults() => new[]
        {
            HitResult.Perfect
        };

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) => new SandboxDifficultyCalculator(RulesetInfo, beatmap);
    }
}
