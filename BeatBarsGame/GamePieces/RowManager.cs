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

        double timeSinceLastSpawn;
        double spawnRate;

        public RowManager(Game game) : base(game)
        {
            timeSinceLastSpawn = 0;
            spawnRate = 2;
            this.controller = new BBPlayerController(game);
            Rows = new Dictionary<RowCompassLocation, BeatRow> {
                { RowCompassLocation.North, new BeatRow(this.Game, RowCompassLocation.North, 3)},
                { RowCompassLocation.South, new BeatRow(this.Game, RowCompassLocation.South, 3)},
                { RowCompassLocation.East, new BeatRow(this.Game, RowCompassLocation.East, 3)},
                { RowCompassLocation.West, new BeatRow(this.Game, RowCompassLocation.West, 3)}
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
            timeSinceLastSpawn += gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            if (timeSinceLastSpawn >= spawnRate)
            {
                ChooseRowToSpawn();
                timeSinceLastSpawn = 0;
            }
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

        void ChooseRowToSpawn()
        {
            int choice = RandomQueue.GetRandomInt(Rows.Count);
            Rows.ElementAt(choice).Value.RequestSpawn();
        }

        Rectangle CalculateRowRectangle(BeatRow row)
        {
            int smallerdimension = GraphicsDevice.Viewport.Height;
            int largerdimension = GraphicsDevice.Viewport.Width;
            if (GraphicsDevice.Viewport.Width < GraphicsDevice.Viewport.Height) {
                smallerdimension = GraphicsDevice.Viewport.Width;
                largerdimension = GraphicsDevice.Viewport.Height;
            }

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
