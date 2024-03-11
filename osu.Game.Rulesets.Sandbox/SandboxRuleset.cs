﻿using System.Collections.Generic;
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
using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Framework.Input.Bindings;

namespace osu.Game.Rulesets.Sandbox
{
    public partial class SandboxRuleset : Ruleset
    {
        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => new DrawableSandboxRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new SandboxBeatmapConverter(beatmap, this);

        public override IRulesetConfigManager CreateConfig(SettingsStore settings) => new SandboxRulesetConfigManager(settings, RulesetInfo);

        public override RulesetSettingsSubsection CreateSettings() => new SandboxSettingsSubsection(this);

        public override string Description => "Sandbox";

        public override string ShortName => "sandbox";

        public override string PlayingVerb => "Sandboxing";

        public override Drawable CreateIcon() => new SandboxIcon(this);

        public override IEnumerable<Mod> GetModsFor(ModType type) => Array.Empty<Mod>();

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.Up, SandboxAction.NumbersUp),
            new KeyBinding(InputKey.Down, SandboxAction.NumbersDown),
            new KeyBinding(InputKey.Left, SandboxAction.NumbersLeft),
            new KeyBinding(InputKey.Right, SandboxAction.NumbersRight),
            new KeyBinding(InputKey.Z, SandboxAction.FlappyJump),
            new KeyBinding(InputKey.X, SandboxAction.FlappyJump)
        };

        protected override IEnumerable<HitResult> GetValidHitResults() => new[]
        {
            HitResult.Perfect
        };

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) => new SandboxDifficultyCalculator(RulesetInfo, beatmap);

        private partial class SandboxIcon : Sprite
        {
            private readonly SandboxRuleset ruleset;

            public SandboxIcon(SandboxRuleset ruleset)
            {
                this.ruleset = ruleset;
            }

            [BackgroundDependencyLoader]
            private void load(GameHost host)
            {
                Texture = new TextureStore(host.Renderer, new TextureLoaderStore(ruleset.CreateResourceStore()), false).Get("Textures/Icons/Sandbox");
            }
        }
    }
}
