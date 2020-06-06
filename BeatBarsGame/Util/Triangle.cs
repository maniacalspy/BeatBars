using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBarsGame
{
    class Triangle
    {
        Vector2[] _vertexPositions;
        public Vector2[] Verticies { get { return _vertexPositions; } }

        Vector2 _center;

        public Vector2 Center
        {
            get
            {
                return _center;
            }

            set
            {
                for(int i = 0; i < 3; i++)
                {
                    _vertexPositions[i] -= (value - _center);
                }
                _center = value;
            }
        }

        public Triangle(Vector2[] verticies)
        {
            _vertexPositions = verticies;
            CalcCenter();
        }

        public Vector2 CalcCenter()
        {
            float XPos = 0, YPos = 0;

            for (int i = 0; i < 3; i++)
            {
                XPos += _vertexPositions[i].X;
                YPos += _vertexPositions[i].Y;
            }

            XPos /= 3;
            YPos /= 3;
            return new Vector2(XPos, YPos);
        }

    }
}
