using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBarsGame
{

    public interface IScoreService
    {
        int CurrentScore { get; set; }
        void AddScore(int score);
    }

    class ScoreManager : DrawableGameComponent, IScoreService
    {
        int _score;
        public int CurrentScore
        {
            get { return _score; }
            set { 
                _score = value;
                if (_score > _highScore) _highScore = _score;
            }
        }
        int _highScore;

        Vector2 streakPosition;
        Vector2 HSPosition;

        SpriteFont font;
        public ScoreManager(Game game) : base(game)
        {
            CurrentScore = 0;
            _highScore = 0;

            game.Services.AddService(typeof(IScoreService), this);
        }

        public void AddScore(int score)
        {
            CurrentScore += score;
            if (CurrentScore >= _highScore) _highScore = CurrentScore;
        }

        public override void Initialize()
        {
            streakPosition = new Vector2(GraphicsDevice.Viewport.Width * .05f, GraphicsDevice.Viewport.Height * .05f);
            HSPosition = new Vector2(GraphicsDevice.Viewport.Width * .05f, GraphicsDevice.Viewport.Height * .1f);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            try
            {
                font = this.Game.Content.Load<SpriteFont>("content/Arial");
            }
            catch
            {
                try
                {
                    font = this.Game.Content.Load<SpriteFont>("Arial");
                }
                catch
                {
                    font = this.Game.Content.Load<SpriteFont>("SpriteFont1");
                }
            }
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            sb.Begin();
            sb.DrawString(font, $"Streak: {_score}", streakPosition, Color.White);
            sb.DrawString(font, $"Highest Streak: {_highScore}", HSPosition, Color.White);

            sb.End();
            base.Draw(gameTime);
        }
    }
}
