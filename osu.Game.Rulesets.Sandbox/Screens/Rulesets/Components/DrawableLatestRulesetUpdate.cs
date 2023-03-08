using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Sandbox.Online;

namespace osu.Game.Rulesets.Sandbox.Screens.Rulesets.Components
{
    public partial class DrawableLatestRulesetUpdate : CompositeDrawable
    {
        private readonly string url;

        private GetLatestReleaseRequest request;
        private readonly FillFlowContainer flow;

        public DrawableLatestRulesetUpdate(string url)
        {
            this.url = url;

            AutoSizeAxes = Axes.Both;
            AddInternal(flow = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
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
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            checkUpdate();
        }

        private void checkUpdate()
        {
            request?.Abort();

            request = new GetLatestReleaseRequest(url);
            request.Finished += () => Schedule(() =>
            {
                flow.Add(new DrawableDate(request.ResponseObject.PublishedAt)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = OsuFont.GetFont(weight: FontWeight.Bold)
                });
            });

            request.Failed += (_) => onFail();
            request.PerformAsync();
        }

        private void onFail()
        {
            Schedule(() =>
            {
                flow.Add(new OsuSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = OsuFont.GetFont(weight: FontWeight.Bold),
                    Text = "Check failed."
                });
            });
        }

        protected override void Dispose(bool isDisposing)
        {
            request?.Abort();
            base.Dispose(isDisposing);
        }
    }
}
