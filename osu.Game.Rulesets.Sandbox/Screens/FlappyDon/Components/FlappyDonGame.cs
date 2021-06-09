using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Sandbox.Screens.FlappyDon.Components
{
    public class FlappyDonGame : CompositeDrawable
    {
        private readonly Bindable<GameState> gameState = new Bindable<GameState>();

        private readonly Backdrop background;
        private readonly Backdrop ground;
        private readonly Bird bird;

        public FlappyDonGame()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                background = new Backdrop(() => new BackgroundSprite(), 20000),
                ground = new Backdrop(() => new GroundSprite(), 2250),
                bird = new Bird()
            };

            bird.GroundY = 525;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

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

            if (!bird.IsTouchingGround)
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

            bird.Reset();
        }

        private void play()
        {
            bird.FlyUp();
        }

        private void fail()
        {
            bird.FallDown();
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
