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

                    barM.barRegion = new Rectangle(XPos, YPos, Width, Height);
                    RowLocation = new Vector2(XPos, YPos);
                    barM.UpdateBarLocationAndRects();
                }
            }
        }

        public BeatRow(Game game, RowCompassLocation location) : base(game)
        {
            Vector2[] basisVectors = new Vector2[2] { new Vector2(0, -1f / 3), new Vector2(1f / 3, 0) };

            switch (location)
            {
                case RowCompassLocation.South:
                    basisVectors[0] = new Vector2(0, 1f / 3);
                    break;

                case RowCompassLocation.East:
                    basisVectors = new Vector2[2] { new Vector2(1f / 3, 0), new Vector2(0, 1f / 3) };
                    break;

                case RowCompassLocation.West:
                    basisVectors = new Vector2[2] { new Vector2(-1f / 3, 0), new Vector2(0, -1f / 3) };
                    break;

            }
            basis = basisVectors;
            beatM = new BeatManager();
            barM = new BarManager(game, basis);
            compassLocation = location;
            BarsNeedFlipped = false;
            //RowLocation = new Vector2(100, 100);
            rowRectangle = new Rectangle();
        }

        public override void Initialize()
        {
            barM.Initialize();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (BarsNeedFlipped)
            {
                barM.FlipBars();
                BarsNeedFlipped = false;
            }

            barM.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            barM.Draw(gameTime);
            base.Draw(gameTime);
        }

    }
}
