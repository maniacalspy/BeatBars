using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBarsGame
{
    class TriangleBar : DrawableTriangle
    {
        internal GameConsoleBar bar;

        public TriangleBar(Game game, VertexPositionColor[] vertexPositionColors) : base(game, vertexPositionColors)
        {
            this.bar = new GameConsoleBar((GameConsole)game.Services.GetService<IGameConsole>());
        }

        public TriangleBar(Game game, Vector2[] basisVectors) : base(game, basisVectors)
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
                            if (this.barState == BarSideState.Blue)
                            {
                                this.DrawColor = Color.Green;
                                this.barState = this.bar.State = value;
                            }
                            break;
                        case BarSideState.Red:
                            if (this.barState == BarSideState.Green)
                            {
                                this.DrawColor = Color.Red;
                                this.barState = this.bar.State = value;
                            }
                            break;
                        case BarSideState.Blue:
                            if (this.barState == BarSideState.Red)
                            {
                                this.DrawColor = Color.Blue;
                                this.barState = this.bar.State = value;
                            }
                            break;
                    }
                //this.barState = this.bar.State = value;
            }
        }


        public override void Initialize()
        {
            this.DrawColor = Color.Green;
            //this.Location = new Vector2(100, 100);
            this.BarState = BarSideState.Green;
            base.Initialize();
        }
    }
}
