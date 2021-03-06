﻿using Microsoft.Xna.Framework;
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
        internal Bar bar;

        Vector2 barMidPoint;
        public Vector2 BarMidPoint { get { return barMidPoint; } }
        public TriangleBar(Game game, Vector2[] vertexPositionVectors) : base(game, vertexPositionVectors)
        {
            this.bar = new Bar();
            this.BarState = BarSideState.Green;
        }

        protected BarSideState barState;
        public BarSideState BarState
        {
            get { return this.barState; }
            set
            {
                if (this.barState != value)
                {
                    GradualColorChange(GetColorFromState(value));
                    switch (value)
                    {
                        case BarSideState.Green:
                            this.barState = this.bar.State = value;
                            break;
                        case BarSideState.Red:
                            this.barState = this.bar.State = value;
                            break;
                    }
                }
            }
        }

        public override void SetVertexZeroToPosition(Vector3 newPosition)
        {
            base.SetVertexZeroToPosition(newPosition);
            Vector2 point1 = new Vector2(_pcVerticies[1].Position.X, _pcVerticies[1].Position.Y);
            Vector2 point2 = new Vector2(_pcVerticies[2].Position.X, _pcVerticies[2].Position.Y);
            barMidPoint = (point1 + point2) / 2;
        }

        public override void Initialize()
        {
            if (this.BarState == BarSideState.Green) this.DrawColor = Color.Green;
            base.Initialize();
        }
    }
}
