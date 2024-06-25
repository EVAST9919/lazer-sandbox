using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Sandbox.Configuration;
using osuTK;

namespace osu.Game.Rulesets.Sandbox.UI.Settings
{
    public partial class ColourPickerDropdown : FillFlowContainer
    {
        private readonly Bindable<string> hexColour = new Bindable<string>();

        private OsuColourPicker picker;
        private readonly SandboxRulesetSetting lookup;

        public ColourPickerDropdown(string name, SandboxRulesetSetting lookup)
        {
            this.lookup = lookup;

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Direction = FillDirection.Vertical;
            Spacing = new Vector2(5);
            Children = new Drawable[]
            {
                new OsuSpriteText
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Text = name
                },
                picker = new OsuColourPicker
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(SandboxRulesetConfigManager rulesetConfig)
        {
            rulesetConfig.BindWith(lookup, hexColour);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            picker.Current.Value = Colour4.FromHex(hexColour.Value);
            picker.Current.BindValueChanged(c =>
            {
                hexColour.Value = c.NewValue.ToHex();
            });
        }
    }
}
