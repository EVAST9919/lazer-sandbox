using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Bindables;
using osuTK;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;

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
        private Sample pointSample;

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
        private void load(ISampleStore samples)
        {
            pointSample = samples.Get("point");
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
            background.Start();
            ground.Start();

            score.Value = 0;
            bird.Reset();
            obstacles.Reset();
        }

        private void play()
        {
            obstacles.Start();
            bird.FlyUp();
        }

        private void fail()
        {
            bird.FallDown();

            obstacles.Freeze();
            background.Freeze();
            ground.Freeze();
        }

        private enum GameState
        {
            Ready,
            Playing,
            GameOver
        }
    }
}
