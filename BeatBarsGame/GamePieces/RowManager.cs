using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BeatBarsGame
{
    class RowManager : DrawableGameComponent
    {
        internal BBPlayerController controller { get; set; }
        Dictionary<RowCompassLocation, BeatRow> Rows = new Dictionary<RowCompassLocation, BeatRow>();

        public RowManager(Game game) : base(game)
        {
            this.controller = new BBPlayerController(game);
            Rows = new Dictionary<RowCompassLocation, BeatRow> {
                { RowCompassLocation.North, new BeatRow(this.Game, RowCompassLocation.North)},
                { RowCompassLocation.South, new BeatRow(this.Game, RowCompassLocation.South)},
                { RowCompassLocation.East, new BeatRow(this.Game, RowCompassLocation.East)},
                { RowCompassLocation.West, new BeatRow(this.Game, RowCompassLocation.West)}
            };
        }

        public override void Initialize()
        {
            foreach(var r in Rows)
            {

                r.Value.RowRectangle = CalculateRowRectangle(r.Value);
                r.Value.Initialize();
            }
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.controller.Update();
            if (controller.RowsNeedChanged)
            {
                foreach(var r in controller.ChangingRows)
                {
                    Rows[r].BarsNeedFlipped = true;
                }
            }

            foreach(var r in Rows)
            {
                r.Value.Update(gameTime);
            }

            base.Update(gameTime);
        }

        Rectangle CalculateRowRectangle(BeatRow row)
        {
            int X = 0, Y = 0, Width = this.GraphicsDevice.Viewport.Width / 3, Height = this.GraphicsDevice.Viewport.Height / 3;
            switch (row.compassLocation)
            {
                case RowCompassLocation.North:
                    X = this.Game.GraphicsDevice.Viewport.Width / 3;
                    Y = 0;
                    break;

                case RowCompassLocation.South:
                    X = this.Game.GraphicsDevice.Viewport.Width / 3;
                    Y = (int)(this.Game.GraphicsDevice.Viewport.Height * (2f / 3));
                    break;

                case RowCompassLocation.West:
                    X = 0;
                    Y = this.Game.GraphicsDevice.Viewport.Height / 3;
                    break;

                case RowCompassLocation.East:
                    X = (int)(this.Game.GraphicsDevice.Viewport.Width * (2f / 3));
                    Y = this.Game.GraphicsDevice.Viewport.Height / 3;
                    break;
            }
            return new Rectangle(X, Y, Width, Height);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach(var r in Rows)
            {
                r.Value.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
    }
}
