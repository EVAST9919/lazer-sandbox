using osu.Framework.IO.Network;

namespace osu.Game.Rulesets.Sandbox.Online
{
    public class GetLatestReleaseRequest : JsonWebRequest<GitHubRelease>
    {
        public GetLatestReleaseRequest()
            : base("https://api.github.com/repos/evast9919/lazer-sandbox/releases/latest")
        {
        }
    }
}
