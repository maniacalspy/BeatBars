using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using static BeatBarsGame.BarGlobals;

namespace BeatBarsGame
{
    class MonoGameBar : Sprite
    {
        public MonoGameBar(Game game) : base(game)
        {
            this.bar = new GameConsoleBar((GameConsole)game.Services.GetService<IGameConsole>());
        }

        protected BarSideState barState;
        public BarSideState BarState
        {
            get { return this.barState; }
            set
            {
                if (this.barState != value)
                    switch (value)
                    {
                        case BarSideState.Green:
                                this.DrawColor = Color.Green;
                                this.barState = this.bar.State = value;
                            break;
                        case BarSideState.Red:
                            this.DrawColor = Color.Red;
                            this.barState = this.bar.State = value;
                            break;
                    }

            }
        }

        internal GameConsoleBar bar { get; private set; }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteTexture = this.Game.Content.Load<Texture2D>("square");
            this.Origin = new Vector2(this.spriteTexture.Width / 2, this.spriteTexture.Height / 2);
            this.DrawColor = Color.Green;
            //this.Location = new Vector2(100, 100);
            this.BarState = BarSideState.Green;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        //public override void Draw(SpriteBatch sb)
        //{


        //sb.Draw(spriteTexture,
        //  Rectangle,
        //      null,
        //      DrawColor,
        //      MathHelper.ToRadians(Rotation),
        //      this.Origin,
        //      SpriteEffects,
        //      0);
        //}

}
}
