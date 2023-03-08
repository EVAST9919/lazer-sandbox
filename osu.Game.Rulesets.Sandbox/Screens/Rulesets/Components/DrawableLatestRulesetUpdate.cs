using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Utils;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Sandbox.Online;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Screens.Rulesets.Components
{
    public partial class DrawableLatestRulesetUpdate : CompositeDrawable
    {
        private readonly string url;

        private GetLatestReleaseRequest request;
        private readonly FillFlowContainer flow;
        private readonly UpdateButton button;

        public DrawableLatestRulesetUpdate(string url)
        {
            this.url = url;

            AutoSizeAxes = Axes.Y;
            RelativeSizeAxes = Axes.X;
            AddRangeInternal(new Drawable[]
            {
                button = new UpdateButton
                {
                    Clicked = checkUpdate,
                    Text = "Check latest update"
                },
                flow = new FillFlowContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    Alpha = 0f,
                    Children = new Drawable[]
                    {
                        new OsuSpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Font = OsuFont.GetFont(weight: FontWeight.Bold),
                            Text = "Latest update: "
                        }
                    }
                }
            });
        }

        private void checkUpdate()
        {
            button.Enabled.Value = false;
            button.Text = "Checking...";

            request?.Abort();

            request = new GetLatestReleaseRequest(url);
            request.Finished += () => Schedule(() =>
            {
                button.Hide();

                TimeSpan timeSpan = DateTimeOffset.Now.Subtract(request.ResponseObject.PublishedAt);

                int days = Math.Clamp(timeSpan.Days, 0, 7);
                Color4 color = Interpolation.ValueAt(days, Color4.Red, Color4.DeepSkyBlue, 0, 7);

                flow.Add(new DrawableDate(request.ResponseObject.PublishedAt)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = OsuFont.GetFont(weight: FontWeight.Bold),
                    Colour = color
                });

                flow.Show();
            });

            request.Failed += (_) => onFail();
            request.PerformAsync();
        }

        private void onFail()
        {
            Schedule(() =>
            {
                button.Hide();

                flow.Add(new OsuSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = OsuFont.GetFont(weight: FontWeight.Bold),
                    Text = "Check failed."
                });

                flow.Show();
            });
        }

        protected override void Dispose(bool isDisposing)
        {
            request?.Abort();
            base.Dispose(isDisposing);
        }

        private partial class UpdateButton : SettingsButton
        {
            public Action Clicked;

            public UpdateButton()
            {
                Padding = new MarginPadding(0);
                Action = () => Clicked?.Invoke();
            }
        }
    }
}
