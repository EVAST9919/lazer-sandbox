using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays.Settings;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.Rulesets.Components
{
    public partial class RulesetPanel : CompositeDrawable
    {
        public RulesetPanel(string name, string updateURL, string downloadURL = "", RulesetDownloadType type = RulesetDownloadType.Github)
        {
            RelativeSizeAxes = Axes.X;
            Height = 120;
            Masking = true;
            CornerRadius = 10;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black.Opacity(0.5f)
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Horizontal = 20 },
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Spacing = new Vector2(15),
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Children = new Drawable[]
                            {
                                new RulesetIcon(name)
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Size = new Vector2(50)
                                },
                                new OsuSpriteText
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Text = name,
                                    Font = OsuFont.GetFont(size: 30, weight: FontWeight.Bold)
                                }
                            }
                        },
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Y,
                            Width = 200,
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0, 10),
                            Children = new Drawable[]
                            {
                                new DrawableLatestRulesetUpdate(updateURL)
                                {
                                    Anchor = Anchor.CentreRight,
                                    Origin = Anchor.CentreRight
                                },
                                new RulesetDownloadButton(string.IsNullOrEmpty(downloadURL) ? updateURL : downloadURL, type)
                                {
                                    Anchor = Anchor.CentreRight,
                                    Origin = Anchor.CentreRight,
                                    Text = "Download"
                                }
                            }
                        }
                    }
                }
            };
        }

        private partial class RulesetDownloadButton : SettingsButton
        {
            private readonly string downloadURL;
            private readonly RulesetDownloadType type;

            public RulesetDownloadButton(string downloadURL, RulesetDownloadType type)
            {
                this.downloadURL = downloadURL;
                this.type = type;

                Padding = new MarginPadding(0);
            }

            [BackgroundDependencyLoader]
            private void load(GameHost host)
            {
                Action = () => host.OpenUrlExternally(type == RulesetDownloadType.Github ? $"https://github.com/{downloadURL}/releases/latest" : downloadURL);
            }
        }

        private partial class RulesetIcon : Sprite
        {
            private readonly string name;

            public RulesetIcon(string name)
            {
                this.name = name;
            }

            [BackgroundDependencyLoader]
            private void load(TextureStore textures)
            {
                Texture = textures.Get($"Icons/{name}");
                if (Texture == null)
                    Alpha = 0;
            }
        }
    }
}
