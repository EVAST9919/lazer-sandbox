using osu.Framework.IO.Network;

namespace osu.Game.Rulesets.Sandbox.Online
{
    public class GetLatestReleaseRequest : JsonWebRequest<GitHubRelease>
    {
        public GetLatestReleaseRequest(string url)
            : base($"https://api.github.com/repos/{url}/releases/latest")
        {
        }
    }
}
