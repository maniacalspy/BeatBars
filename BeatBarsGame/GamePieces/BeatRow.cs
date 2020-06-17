using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBarsGame
{
    public enum RowCompassLocation { North, East, South, West}
    class BeatRow : DrawableGameComponent
    {
        public readonly RowCompassLocation compassLocation;

        public bool BarsNeedFlipped;

        int laneCount;

        BeatManager beatM;
        BarManager barM;

        Vector2[] basis;

        public Vector2 RowLocation;

        Rectangle rowRectangle;
        public Rectangle RowRectangle
        {
            get { return rowRectangle; }

            set { 
                if (this.rowRectangle != value)
                {
                    rowRectangle = value;
                    int XPos = rowRectangle.Left, YPos = rowRectangle.Top, Width = 0, Height = 0;
                    
                    switch (compassLocation)
                    {
                        case RowCompassLocation.North:
                            YPos = rowRectangle.Bottom;
                            Width = rowRectangle.Width;
                            Height = rowRectangle.Height / 10;
                            break;
                        case RowCompassLocation.South:
                            Width = rowRectangle.Width;
                            Height = rowRectangle.Height / 10;
                            break;

                        case RowCompassLocation.East:
                            Width = rowRectangle.Width / 10;
                            Height = rowRectangle.Height;
                            break;

                        case RowCompassLocation.West:
                            XPos = rowRectangle.Right;
                            Width = rowRectangle.Width / 10;
                            Height = rowRectangle.Height;
                            break;
                    }


                    RowLocation = new Vector2(XPos, YPos);
                    if (barM != null)
                    {
                        barM.barRegion = new Rectangle(XPos, YPos, Width, Height);
                        barM.UpdateBarLocationAndRects();
                    }
                }
            }
        }

        public BeatRow(Game game, RowCompassLocation location, int numLanes) : base(game)
        {
            laneCount = numLanes;
            compassLocation = location;
            BarsNeedFlipped = false;
            rowRectangle = new Rectangle();
        }

        public override void Initialize()
        {
            Vector2[] basisVectors = new Vector2[2] { new Vector2(0, Game.GraphicsDevice.Viewport.Height / 6), new Vector2(GraphicsDevice.Viewport.Width/6, 0) };
            switch (compassLocation)
            {
                case RowCompassLocation.South:
                    basisVectors[0] = new Vector2(0, -Game.GraphicsDevice.Viewport.Height / 6);
                    break;

                case RowCompassLocation.East:
                    basisVectors = new Vector2[2] { new Vector2(GraphicsDevice.Viewport.Width / 6, 0), new Vector2(0, -Game.GraphicsDevice.Viewport.Height / 6) };
                    break;

                case RowCompassLocation.West:
                    basisVectors = new Vector2[2] { new Vector2(-GraphicsDevice.Viewport.Width / 6, 0), new Vector2(0, -Game.GraphicsDevice.Viewport.Height / 6) };
                    break;

            }

            barM = new BarManager(Game, basisVectors, laneCount);
            barM.Initialize();

            beatM = new BeatManager(Game, barM, rowRectangle, laneCount, compassLocation);
            beatM.Initialize();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            List<Beat> beatsToRemove = new List<Beat>();
            if (BarsNeedFlipped)
            {
                barM.FlipBars();
                BarsNeedFlipped = false;
            }
            beatM.Update(gameTime);
            barM.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            
            barM.Draw(gameTime);
            beatM.Draw(gameTime);
            base.Draw(gameTime);
        }

    }
}
