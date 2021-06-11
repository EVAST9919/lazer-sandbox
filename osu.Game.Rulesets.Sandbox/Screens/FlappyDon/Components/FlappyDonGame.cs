using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Bindables;
using osuTK;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Sandbox.Screens.FlappyDon.Components
{
    public class FlappyDonGame : CompositeDrawable
    {
        public static readonly Vector2 SIZE = new Vector2(1920, 1080);
        public static readonly int GROUND_HEIGHT = 200;

        private readonly Bindable<GameState> gameState = new Bindable<GameState>();
        private readonly BindableInt score = new BindableInt();

        private readonly Backdrop background;
        private readonly Backdrop ground;
        private readonly Bird bird;
        private readonly Obstacles obstacles;
        private readonly OsuSpriteText drawableScore;
        private readonly Box flash;
        private readonly Sprite readySprite;
        private readonly Sprite gameOverSprite;

        private Sample pointSample;
        private Sample punchSample;
        private Sample deathSample;

        private bool clickIsBlocked;

        public FlappyDonGame()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChild = new FlappyDonScalingContainer(SIZE)
            {
                Children = new Drawable[]
                {
                    background = new Backdrop(() => new BackgroundSprite(), 20000),
                    obstacles = new Obstacles(),
                    bird = new Bird(),
                    ground = new Backdrop(() => new GroundSprite(), 2250),
                    drawableScore = new OsuSpriteText
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Y = 150,
                        Font = OsuFont.GetFont(size: 80, weight: FontWeight.SemiBold)
                    },
                    flash = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.White,
                        Alpha = 0
                    },
                    readySprite = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Alpha = 0
                    },
                    gameOverSprite = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Alpha = 0
                    }
                }
            };

            bird.GroundY = SIZE.Y - GROUND_HEIGHT;
            obstacles.BirdThreshold = bird.X;

            obstacles.ThresholdCrossed += _ =>
            {
                score.Value++;
                pointSample?.Play();
            };
        }

        [BackgroundDependencyLoader]
        private void load(ISampleStore samples, TextureStore textures)
        {
            pointSample = samples.Get("point");
            punchSample = samples.Get("hit");
            deathSample = samples.Get("die");

            var readyTexture = textures.Get("FlappyDon/message");
            var gameOverTexture = textures.Get("FlappyDon/gameover");

            readySprite.Texture = readyTexture;
            readySprite.Size = readyTexture.Size;
            gameOverSprite.Texture = gameOverTexture;
            gameOverSprite.Size = gameOverTexture.Size;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            score.BindValueChanged(score => drawableScore.Text = score.NewValue.ToString(), true);

            gameState.BindValueChanged(state =>
            {
                switch (state.NewValue)
                {
                    case GameState.Ready:
                        ready();
                        return;

                    case GameState.Playing:
                        play();
                        return;

                    case GameState.GameOver:
                        fail();
                        return;
                }
            }, true);
        }

        protected override void Update()
        {
            base.Update();

            if (gameState.Value != GameState.Playing)
                return;

            if (!bird.IsTouchingGround && !obstacles.CheckForCollision(bird.CollisionQuad))
                return;

            gameState.Value = GameState.GameOver;
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (clickIsBlocked)
                return true;

            switch(gameState.Value)
            {
                case GameState.GameOver:
                    gameState.Value = GameState.Ready;
                    return true;

                case GameState.Playing:
                    bird.FlyUp();
                    return true;

                case GameState.Ready:
                    gameState.Value = GameState.Playing;
                    return true;
            };

            return true;
        }

        private void ready()
        {
            gameOverSprite.ClearTransforms();
            gameOverSprite.Hide();

            readySprite.Show();
            Scheduler.CancelDelayedTasks();

            flash.FinishTransforms();
            background.Start();
            ground.Start();

            score.Value = 0;
            bird.Reset();
            obstacles.Reset();
        }

        private void play()
        {
            readySprite.Hide();
            obstacles.Start();
            bird.FlyUp();
        }

        private void fail()
        {
            clickIsBlocked = true;
            Scheduler.AddDelayed(() => clickIsBlocked = false, 300);

            bird.FallDown();
            gameOverSprite.FadeIn(250, Easing.OutQuint);

            // Play the punch sound, and then the 'fall' sound slightly after
            punchSample.Play();
            Scheduler.AddDelayed(() => deathSample.Play(), 100);

            obstacles.Freeze();
            background.Freeze();
            ground.Freeze();

            flash.FadeIn(20, Easing.OutQuint).Then().FadeOut(750, Easing.Out);
        }

        private enum GameState
        {
            Ready,
            Playing,
            GameOver
        }
    }
}
