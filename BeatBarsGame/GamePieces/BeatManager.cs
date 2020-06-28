using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        Vector2 progressionDirection;

        List<Vector2> LaneSpawnPoints;
        BarManager bM;

        float speed;

        public BeatManager(Game game, BarManager barManager, Rectangle region, int laneCount, RowCompassLocation compassLocation) : base(game)
        {
            bM = barManager;
            beatsToRemove = new List<Beat>();
            wrongbeats = new List<Beat>();
            beatsRowID = new Dictionary<Beat, int>();
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

            if (beatsToRemove.Count > 0)
            {
                RemoveBeats();
                beatsToRemove.Clear();
            }

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
                    beatsToRemove.Add(b);
                    wrongbeats.Add(b);
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

        public void SpawnBeat()
        {
            int beatCount = RandomQueue.GetRandomInt(LaneSpawnPoints.Count+1);
            int firstBarState = RandomQueue.GetRandomInt(BarSideCount);
            List<int> AvailableLanes = new List<int>(LaneSpawnPoints.Count);
            List<int> ChosenLanes = new List<int>();
            for (int i = 0; i < LaneSpawnPoints.Count; i++)
            {
                AvailableLanes.Add(i);
            }

            for (int a = beatCount; a > 0; a--)
            {
                int index = RandomQueue.GetRandomInt(AvailableLanes.Count);
                int laneChoice = AvailableLanes[index];
                ChosenLanes.Add(laneChoice);
                AvailableLanes.RemoveAt(index);
            }
            ChosenLanes.Sort();
            for (int x = 0; x < beatCount; x++)
            {
                Beat b = new Beat(Game, LaneSpawnPoints[ChosenLanes[x]]);
                b.Initialize();
                b.State = GetStateFromInt((firstBarState + (ChosenLanes[x] - ChosenLanes[0])) % BarSideCount);
                beatsRowID.Add(b, ChosenLanes[x]);
            }
            
        }

        void RemoveBeats()
        {
            foreach (var b in beatsToRemove)
            {
                if (beatsRowID.ContainsKey(b))
                {
                    if (!wrongbeats.Contains(b))
                    {
                        Game.Services.GetService<IScoreService>().AddScore(1);
                    }
                    else Game.Services.GetService<IScoreService>().CurrentScore = 0;
                    b.Dispose();
                    beatsRowID.Remove(b);
                }
            }
            beatsToRemove.Clear();
            wrongbeats.Clear();
        }

    }
}
