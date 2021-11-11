using osuTK;
using osuTK.Graphics;
using osuTK.Graphics.ES30;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Layout;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using System.Collections.Generic;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Buffers;
using osu.Framework.Graphics.Colour;

namespace osu.Game.Rulesets.Yoso.Graphics
{
    public class PostProcessingContainer : Container<Drawable>, IBufferedDrawable
    {
        private Color4 backgroundColour = new Color4(0, 0, 0, 0);

        /// <summary>
        /// The background colour of the framebuffer. Transparent black by default.
        /// </summary>
        public Color4 BackgroundColour
        {
            get => backgroundColour;
            set
            {
                if (backgroundColour == value)
                    return;

                backgroundColour = value;
                ForceRedraw();
            }
        }

        private Vector2 frameBufferScale = Vector2.One;

        public Vector2 FrameBufferScale
        {
            get => frameBufferScale;
            set
            {
                if (frameBufferScale == value)
                    return;

                frameBufferScale = value;
                ForceRedraw();
            }
        }

        /// <summary>
        /// Forces a redraw of the framebuffer before it is blitted the next time.
        /// Only relevant if <see cref="CacheDrawnFrameBuffer"/> is true.
        /// </summary>
        public void ForceRedraw() => Invalidate(Invalidation.DrawNode);

        /// <summary>
        /// In order to signal the draw thread to re-draw the buffered container we version it.
        /// Our own version (update) keeps track of which version we are on, whereas the
        /// drawVersion keeps track of the version the draw thread is on.
        /// When forcing a redraw we increment updateVersion, pass it into each new drawnode
        /// and the draw thread will realize its drawVersion is lagging behind, thus redrawing.
        /// </summary>
        private long updateVersion;

        public IShader TextureShader { get; private set; }

        public IShader RoundedTextureShader { get; private set; }

        private IShader shader;

        private readonly PostProcessingContainerDrawNodeDrawNodeSharedData sharedData;
        private readonly string shaderName;

        /// <summary>
        /// Constructs an empty buffered container.
        /// </summary>
        /// <param name="formats">The render buffer formats attached to the frame buffers of this <see cref="BufferedContainer"/>.</param>
        /// <param name="pixelSnapping">Whether the frame buffer position should be snapped to the nearest pixel when blitting.
        /// This amounts to setting the texture filtering mode to "nearest".</param>
        public PostProcessingContainer(string shaderName = "original", RenderbufferInternalFormat[] formats = null, bool pixelSnapping = false)
        {
            this.shaderName = shaderName;
            sharedData = new PostProcessingContainerDrawNodeDrawNodeSharedData(formats, pixelSnapping);

            AddLayout(screenSpaceSizeBacking);
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders)
        {
            TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
            RoundedTextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE_ROUNDED);
            shader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, shaderName);
        }

        protected override DrawNode CreateDrawNode() => CreatePostProcessingDrawNode(sharedData);

        protected virtual PostProcessingContainerDrawNode CreatePostProcessingDrawNode(PostProcessingContainerDrawNodeDrawNodeSharedData shared) => new PostProcessingContainerDrawNode(this, shared);

        public override bool UpdateSubTreeMasking(Drawable source, RectangleF maskingBounds)
        {
            var result = base.UpdateSubTreeMasking(source, maskingBounds);

            childrenUpdateVersion = updateVersion;

            return result;
        }

        protected override RectangleF ComputeChildMaskingBounds(RectangleF maskingBounds) => ScreenSpaceDrawQuad.AABBFloat; // Make sure children never get masked away

        // We actually only care about Invalidation.MiscGeometry | Invalidation.DrawInfo
        private readonly LayoutValue screenSpaceSizeBacking = new LayoutValue(Invalidation.Presence | Invalidation.RequiredParentSizeToFit | Invalidation.DrawInfo);

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {
            var result = base.OnInvalidate(invalidation, source);

            if ((invalidation & Invalidation.DrawNode) > 0)
            {
                ++updateVersion;
                result = true;
            }

            return result;
        }

        private long childrenUpdateVersion = -1;
        protected override bool RequiresChildrenUpdate => base.RequiresChildrenUpdate && childrenUpdateVersion != updateVersion;

        protected override void Update()
        {
            base.Update();
            ForceRedraw();
        }

        public DrawColourInfo? FrameBufferDrawColour => base.DrawColourInfo;

        // Children should not receive the true colour to avoid colour doubling when the frame-buffers are rendered to the back-buffer.
        public override DrawColourInfo DrawColourInfo
        {
            get
            {
                // Todo: This is incorrect.
                var blending = Blending;
                blending.ApplyDefaultToInherited();

                return new DrawColourInfo(Color4.White, blending);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            sharedData.Dispose();
        }

        protected class PostProcessingContainerDrawNode : BufferedDrawNode, ICompositeDrawNode
        {
            protected new PostProcessingContainer Source => (PostProcessingContainer)base.Source;

            protected new CompositeDrawableDrawNode Child => (CompositeDrawableDrawNode)base.Child;

            private long updateVersion;

            private IShader shader;

            public PostProcessingContainerDrawNode(PostProcessingContainer source, PostProcessingContainerDrawNodeDrawNodeSharedData sharedData)
                : base(source, new CompositeDrawableDrawNode(source), sharedData)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                updateVersion = Source.updateVersion;
                shader = Source.shader;
            }

            protected override long GetDrawVersion() => updateVersion;

            protected override void PopulateContents()
            {
                base.PopulateContents();
                drawFrameBuffer();
            }

            protected override void DrawContents()
            {
                DrawFrameBuffer(SharedData.CurrentEffectBuffer, DrawRectangle, DrawColourInfo.Colour);
            }

            private void drawFrameBuffer()
            {
                FrameBuffer current = SharedData.CurrentEffectBuffer;
                FrameBuffer target = SharedData.GetNextEffectBuffer();

                GLWrapper.SetBlend(BlendingParameters.None);

                using (BindFrameBuffer(target))
                {
                    UpdateUniforms(shader, current);

                    shader.Bind();
                    DrawFrameBuffer(current, new RectangleF(0, 0, current.Texture.Width, current.Texture.Height), ColourInfo.SingleColour(Color4.White));
                    shader.Unbind();
                }
            }

            protected virtual void UpdateUniforms(IShader targetShader, FrameBuffer current)
            {
            }

            public List<DrawNode> Children
            {
                get => Child.Children;
                set => Child.Children = value;
            }

            public bool AddChildDrawNodes => RequiresRedraw;
        }

        public class PostProcessingContainerDrawNodeDrawNodeSharedData : BufferedDrawNodeSharedData
        {
            public PostProcessingContainerDrawNodeDrawNodeSharedData(RenderbufferInternalFormat[] formats, bool pixelSnapping)
                : base(2, formats, pixelSnapping)
            {
            }
        }
    }
}
