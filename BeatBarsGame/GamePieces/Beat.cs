using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static BeatBarsGame.BarGlobals;

namespace BeatBarsGame
{
    class Beat : Sprite
    {

        BarSideState state;
        public BarSideState State { get { return state; } }
        public Beat(Game game, Vector2 position): base(game)
        {
            state = BarSideState.Green;
            this.Location = position;
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteTexture = this.Game.Content.Load<Texture2D>("square");
            this.Origin = new Vector2(this.spriteTexture.Width / 2, this.spriteTexture.Height / 2);
            this.DrawColor = Color.Green;
            this.scale = .01f;
            //this.Location = new Vector2(100, 100);
        }

        public override void Draw(SpriteBatch sb)
        {


            sb.Draw(spriteTexture,
              new Rectangle(
                  (int)Location.X,
                  (int)Location.Y,
                  (int)(spriteTexture.Width * this.Scale),
                  (int)(spriteTexture.Height * this.Scale)),
              null,
              DrawColor,
              MathHelper.ToRadians(Rotation),
              this.Origin,
              SpriteEffects,
              0);

        }
    }
}
