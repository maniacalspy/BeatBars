using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BeatBarsGame.BarGlobals;

namespace BeatBarsGame
{
    class TriangleBar : DrawableTriangle
    {
        internal GameConsoleBar bar;

        public TriangleBar(Game game, Vector2[] vertexPositionVectors) : base(game, vertexPositionVectors)
        {
            this.bar = new GameConsoleBar((GameConsole)game.Services.GetService<IGameConsole>());
            this.BarState = BarSideState.Green;
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
                            GradualColorChange(Color.Green);
                            this.barState = this.bar.State = value;
                            break;
                        case BarSideState.Red:
                            GradualColorChange(Color.Red);
                            this.barState = this.bar.State = value;
                            break;
                    }
            }
        }

        public override void Initialize()
        {
           
            //this.Location = new Vector2(100, 100);
            if (this.BarState == BarSideState.Green) this.DrawColor = Color.Green;
            base.Initialize();
        }
    }
}
