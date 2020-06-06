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
    class BarManager : DrawableGameComponent
    {
        List<TriangleBar> bars = new List<TriangleBar>();

        public Rectangle barRegion;

        SpriteBatch sb;

        public BarManager(Game game) : base(game)
        {
            TriangleBar temp = new TriangleBar(this.Game, 
                
                new Vector2[2]
            {
                new Vector2(0,-1/3),
                new Vector2(1/3, 0)
            }
            
            );
            bars.Add(temp);
        }

        public BarManager(Game game, Vector2[] basisVectors) : this(game, basisVectors, 1)
        {

            Vector2[] positions = new Vector2[3] {
                new Vector2(0,0),
                basisVectors[0] - basisVectors[1],
                basisVectors[0] + basisVectors[1]
                };

            TriangleBar temp = new TriangleBar(this.Game, positions);
            bars.Add(temp);
        }

        public BarManager(Game game, Vector2[] basisVectors, int numBars) : base(game)
        {

            for (int i = 0; i < numBars; i++) {
                Vector2 LeftPos = basisVectors[0] + (
                    -basisVectors[1] + i * ((2 * basisVectors[1])/numBars)
                    );
                Vector2[] positions = new Vector2[3] {
                new Vector2(0,0),
                LeftPos,
                LeftPos + ((2 * basisVectors[1])/numBars)
                };

                TriangleBar temp = new TriangleBar(this.Game, positions);
                temp.BarState = GetStateFromInt(i % BarSideCount);

                bars.Add(temp);
            }
        }
        public void FlipBars()
        {
            foreach(var b in bars)
            {
                b.BarState = GetNextBarState(b.BarState);
            }
        }

        public override void Initialize()
        {
            sb = new SpriteBatch(this.Game.GraphicsDevice);

            foreach(var b in bars)
            {
                b.Initialize();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach(var b in bars)
            {
                b.Update(gameTime);
            }
        }
        public void UpdateBarLocationAndRects()
        {
            foreach(var b in bars)
            {
                //b.Location = barRegion.Location.ToVector2();
                //b.Rectangle = barRegion;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();

            foreach(var b in bars)
            {
                if (b.Visible) b.Draw(gameTime);
            }
            sb.End();

            base.Draw(gameTime);
        }
    }
}
