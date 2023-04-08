using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Sandbox.Screens.Rulesets.Components;
using osuTK;

namespace osu.Game.Rulesets.Sandbox.Screens.Rulesets
{
    public partial class RulesetsScreen : SandboxScreen
    {
        public RulesetsScreen()
        {
            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding { Vertical = 10 },
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Width = 0.5f,
                Child = new OsuScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 10),
                        Children = new Drawable[]
                        {
                            new RulesetPanel("Tau", "taulazer/tau"),
                            new RulesetPanel("Yoso", "EVAST9919/yoso-version-tracker", "https://www.patreon.com/evast", RulesetDownloadType.External),
                            new RulesetPanel("Bosu", "EVAST9919/bosu"),
                            new RulesetPanel("Swing", "EVAST9919/lazer-swing"),
                            new RulesetPanel("Touhosu", "EVAST9919/touhosu"),
                            new RulesetPanel("Sandbox", "EVAST9919/lazer-sandbox"),
                            new RulesetPanel("Karaoke", "karaoke-dev/karaoke"),
                            new RulesetPanel("soyokaze!", "goodtrailer/soyokaze"),
                            new RulesetPanel("sentakki", "LumpBloom7/sentakki"),
                            new RulesetPanel("osu!DIVA", "Artemis-chan/osu-DIVA"),
                            new RulesetPanel("Hitokori", "Flutterish/Hitokori"),
                            new RulesetPanel("gamebosu", "Game4all/gamebosu"),
                            new RulesetPanel("Hishigata", "LumpBloom7/hishigata")
                        }
                    }
                }
            });
        }
    }
}
