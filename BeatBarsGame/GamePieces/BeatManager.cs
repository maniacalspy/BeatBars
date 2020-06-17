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
    class BeatManager : DrawableGameComponent
    {
        List<Beat> beatsToRemove;
        List<Beat> wrongbeats;
        Dictionary<Beat, int> beatsRowID;
        public List<Beat> Beats { get { return beatsRowID.Keys.ToList<Beat>(); } }
        List<Rectangle> Lanes;

        Vector2 progressionDirection;

        List<Vector2> LaneSpawnPoints;
        BarManager bM;

        float speed;

        //TESTING VARIABLE
        GameConsole console;

        public BeatManager(Game game, BarManager barManager, Rectangle region, int laneCount, RowCompassLocation compassLocation) : base(game)
        {
            console = (GameConsole)game.Services.GetService<IGameConsole>();
            bM = barManager;
            beatsToRemove = new List<Beat>();
            beatsRowID = new Dictionary<Beat, int>();
            Lanes = new List<Rectangle>();
            LaneSpawnPoints = new List<Vector2>();
            Vector2 SpawnPointOffSet = new Vector2(0,0);
            switch (compassLocation)
            {
                case RowCompassLocation.North:
                    speed = 30f;
                    SpawnPointOffSet = new Vector2(0, -region.Height);
                    break;
                case RowCompassLocation.South:
                    speed = 30f;
                    SpawnPointOffSet = new Vector2(0, region.Height);
                    break;
                case RowCompassLocation.East:
                    speed = 50f;
                    SpawnPointOffSet = new Vector2(region.Width, 0);
                    break;
                case RowCompassLocation.West:
                    speed = 50f;
                    SpawnPointOffSet = new Vector2(-region.Width, 0);
                    break;
            }
            progressionDirection = -SpawnPointOffSet;
            progressionDirection.Normalize();
            for (int i = 0; i < laneCount; i++)
            {
                Vector2 point = barManager.getBarMidPointByIndex(i) + SpawnPointOffSet;
                LaneSpawnPoints.Add(point);
            }
        }

        public override void Initialize()
        {
            for(int i = 0; i < LaneSpawnPoints.Count; i++)
            {
                Beat temp = new Beat(Game, LaneSpawnPoints[i]);
                beatsRowID.Add(temp, i);
                temp.Initialize();
            }
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var b in beatsRowID)
            {
                b.Key.Location += progressionDirection * speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                BarCollisionResult collisionResult = bM.CheckCollision(b.Key, b.Value);
                HandleCollision(b.Key, collisionResult);
                b.Key.Update(gameTime);
            }

            if (beatsToRemove.Count > 0) RemoveBeats(beatsToRemove);

            base.Update(gameTime);
        }

        void HandleCollision(Beat b, BarCollisionResult cr)
        {
            switch (cr)
            {
                case BarCollisionResult.RightSide:
                    beatsToRemove.Add(b);
                    break;
                case BarCollisionResult.WrongSide:
                    console.GameConsoleWrite("Wrong Side!");
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            sb.Begin();
            foreach(var b in beatsRowID)
            {
                if (b.Key.Visible) b.Key.Draw(sb);
            }
            sb.End();
            base.Draw(gameTime);
        }

        void RemoveBeats(List<Beat> beats)
        {
            foreach (var b in beats)
            {
                if (beatsRowID.ContainsKey(b))
                {
                    b.Dispose();
                    beatsRowID.Remove(b);
                }
            }
            beatsToRemove.Clear();
        }

    }
}
