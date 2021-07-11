using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Sandbox.Configuration;
using osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.MusicHelpers;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components
{
    public class Particles : CurrentRateContainer
    {
        private readonly Bindable<string> colour = new Bindable<string>("#ffffff");

        private ParticlesDrawable particles;

        [Resolved(canBeNull: true)]
        private SandboxRulesetConfigManager config { get; set; }

        public Particles()
        {
            RelativeSizeAxes = Axes.Both;
            Add(particles = new ParticlesDrawable());
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            config?.BindWith(SandboxRulesetSetting.ParticlesColour, colour);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            IsKiai.BindValueChanged(kiai =>
            {
                if (kiai.NewValue)
                {
                    particles.SetRandomDirection();
                }
                else
                    particles.Direction.Value = MoveDirection.Forward;
            });

            colour.BindValueChanged(c => particles.Colour = Colour4.FromHex(c.NewValue), true);
        }
    }
}
