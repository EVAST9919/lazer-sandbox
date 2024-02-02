﻿using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Rendering.Vertices;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Game.Rulesets.Sandbox.Extensions;
using osuTK;

namespace osu.Game.Rulesets.Sandbox.Screens.Visualizer.Components.Visualizers
{
    public abstract partial class MusicVisualizerDrawable : Drawable
    {
        // Total amplitude count is 256, however in most cases some of them are empty, let's not use them.
        private const int used_amplitude_count = 160;

        public readonly Bindable<double> BarWidth = new Bindable<double>(5);
        public readonly Bindable<int> BarCount = new Bindable<int>(100);
        public readonly Bindable<int> Decay = new Bindable<int>(200);
        public readonly Bindable<int> HeightMultiplier = new Bindable<int>(400);
        public readonly Bindable<bool> Reversed = new Bindable<bool>();
        public readonly Bindable<int> Smoothness = new Bindable<int>();

        protected Texture Texture { get; private set; }

        protected IShader Shader { get; set; }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders, GameHost host, TextureStore textures)
        {
            Shader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
            Texture = GetTexture(host.Renderer, textures);
        }

        protected virtual Texture GetTexture(IRenderer renderer, TextureStore textures) => renderer.WhitePixel;

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Decay.BindValueChanged(_ => ResetArrays());
            Reversed.BindValueChanged(_ => ResetArrays());
            Smoothness.BindValueChanged(_ => ResetArrays());
            BarCount.BindValueChanged(c => ResetArrays(), true);
        }

        private float[] currentRawAudioData;
        private float[] maxBarValues;
        private float[] smoothAudioData;

        protected int AdjustedBarCount;
        private int barsToFill = 0; // How much bars are needed to fill the gap between real amplitudes

        protected virtual void ResetArrays()
        {
            if (BarCount.Value > used_amplitude_count)
            {
                barsToFill = (int)Math.Ceiling((float)BarCount.Value / used_amplitude_count);
                AdjustedBarCount = used_amplitude_count * barsToFill;
            }
            else
            {
                AdjustedBarCount = BarCount.Value;
            }

            currentRawAudioData = new float[AdjustedBarCount];
            maxBarValues = new float[AdjustedBarCount];
            smoothAudioData = new float[AdjustedBarCount];
        }

        public void SetAmplitudes(float[] amplitudes)
        {
            var newRawAudioData = getConvertedAmplitudes(amplitudes);

            for (int i = 0; i < AdjustedBarCount; i++)
                ApplyData(i, newRawAudioData[i]);
        }

        protected virtual void ApplyData(int index, float newRawAudioDataAtIndex)
        {
            if (newRawAudioDataAtIndex > currentRawAudioData[index])
            {
                currentRawAudioData[index] = newRawAudioDataAtIndex;
                maxBarValues[index] = currentRawAudioData[index];
            }
        }

        protected override void Update()
        {
            base.Update();

            var diff = (float)Clock.ElapsedFrameTime;

            for (int i = 0; i < AdjustedBarCount; i++)
                UpdateData(i, diff);

            PostUpdate();

            Invalidate(Invalidation.DrawNode);
        }

        protected virtual void UpdateData(int index, float timeDifference)
        {
            currentRawAudioData[index] -= maxBarValues[index] / Decay.Value * timeDifference;
            smoothAudioData[index] = currentRawAudioData[index] * HeightMultiplier.Value;
        }

        protected virtual void PostUpdate()
        {
            if (Smoothness.Value > 0)
                smoothAudioData.Smooth(Math.Min(Smoothness.Value, AdjustedBarCount / 2));
        }

        private float[] getConvertedAmplitudes(float[] amplitudes)
        {
            var amps = new float[AdjustedBarCount];

            var lerp = AdjustedBarCount != BarCount.Value;

            if (lerp)
            {
                for (int i = 0; i < used_amplitude_count; i++)
                {
                    for (int j = 0; j < barsToFill; j++)
                    {
                        var realValue = amplitudes[i];
                        var nextValue = amplitudes[i + 1];

                        amps[i * barsToFill + j] = MathExtensions.Map(j, 0, barsToFill, realValue, nextValue);
                    }
                }
            }
            else
            {
                for (int i = 0; i < AdjustedBarCount; i++)
                    amps[i] = amplitudes[(int)MathExtensions.Map(i, 0, AdjustedBarCount, 0, used_amplitude_count)];
            }

            return amps;
        }

        protected override DrawNode CreateDrawNode() => CreateVisualizerDrawNode();

        protected abstract VisualizerDrawNode CreateVisualizerDrawNode();

        protected abstract class VisualizerDrawNode : DrawNode
        {
            protected new MusicVisualizerDrawable Source => (MusicVisualizerDrawable)base.Source;

            protected IVertexBatch<TexturedVertex2D> VertexBatch;
            protected readonly List<float> AudioData = new List<float>();

            private IShader shader;
            protected Texture Texture;
            protected Vector2 Size;
            protected double BarWidth;
            protected bool Reversed;

            protected VisualizerDrawNode(MusicVisualizerDrawable source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                shader = Source.Shader;
                Texture = Source.Texture;
                Size = Source.DrawSize;
                BarWidth = Source.BarWidth.Value;
                Reversed = Source.Reversed.Value;

                AudioData.Clear();
                AudioData.AddRange(Source.smoothAudioData);
            }

            protected abstract float Spacing { get; }
            protected virtual Vector2 Inflation => Vector2.One;

            protected override void Draw(IRenderer renderer)
            {
                base.Draw(renderer);

                if (AudioData.Any())
                {
                    VertexBatch ??= renderer.CreateQuadBatch<TexturedVertex2D>(200, 5);
                    shader.Bind();

                    Vector2 inflation = Inflation;
                    var spacing = Spacing;
                    PreCompute();

                    for (int i = 0; i < AudioData.Count; i++)
                        DrawBar(Reversed ? AudioData.Count - 1 - i : i, AudioData[i], spacing, inflation, renderer);

                    shader.Unbind();
                }
            }

            protected virtual void PreCompute()
            {
            }

            protected abstract void DrawBar(int index, float data, float spacing, Vector2 inflation, IRenderer renderer);

            protected override void Dispose(bool isDisposing)
            {
                base.Dispose(isDisposing);
                VertexBatch?.Dispose();
            }
        }
    }
}
