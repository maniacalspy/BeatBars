using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBarsGame
{
    class BeatManager : DrawableGameComponent
    {
        List<Beat> beats = new List<Beat>();
        List<Rectangle> Lanes;
        Vector2 progressionDirection;

        float speed;

        public BeatManager(Game game, Vector2 direction, Rectangle region, int laneCount, RowCompassLocation compassLocation) : base(game)
        {

            Lanes = new List<Rectangle>();
            switch (compassLocation)
            {
                case RowCompassLocation.North:
                case RowCompassLocation.South:
                    speed = 30f;
                    for (int i = 0; i < laneCount; i++)
                    {
                        Lanes.Add(new Rectangle(region.Left + i * region.Width / laneCount, region.Top, region.Width / laneCount, region.Height));
                    }
                    break;
                case RowCompassLocation.East:
                case RowCompassLocation.West:
                    speed = 50f;
                    for (int i = 0; i < laneCount; i++)
                    {
                        Lanes.Add(new Rectangle(region.Left, region.Top + i * region.Height / laneCount, region.Width, region.Height / laneCount));
                    }
                    break;
            }

            progressionDirection = -direction;
            
        }

        public override void Initialize()
        {
            for(int i = 0; i < Lanes.Count; i++)
            {
                Beat temp = new Beat(Game, Lanes[i].Center.ToVector2());
                beats.Add(temp);
                temp.Initialize();
            }
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            foreach(var b in beats)
            {
                b.Location += progressionDirection * speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                b.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            sb.Begin();
            foreach(var b in beats)
            {
                if (b.Visible) b.Draw(sb);
            }
            sb.End();
            base.Draw(gameTime);
        }
    }
}
