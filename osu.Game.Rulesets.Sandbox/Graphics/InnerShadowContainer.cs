using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Sandbox.Graphics
{
    public abstract partial class InnerShadowContainer : Container
    {
        protected override Container<Drawable> Content => content;

        private readonly Container<Drawable> content;

        protected InnerShadowContainer()
        {
            Masking = true;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.1f
                },
                content = CreateContent(),
                new InnerShadow()
            };
        }

        protected abstract Container<Drawable> CreateContent();

        private partial class InnerShadow : Box
        {
            [BackgroundDependencyLoader]
            private void load(ShaderManager shaders)
            {
                RelativeSizeAxes = Axes.Both;
                Colour = Color4.Black;
                TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "InnerShadow");
            }
        }
    }
}
