using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public bool Intersects(Rectangle otherRect)
        {
            Point[] Verticies = new Point[] {new Point((int)_vertexPositions[0].X, (int)_vertexPositions[0].Y),
                new Point((int)_vertexPositions[1].X, (int)_vertexPositions[1].Y),
                new Point((int)_vertexPositions[2].X, (int)_vertexPositions[2].Y)
                };
            foreach (var v in Verticies)
            {
                if (v.X > otherRect.Left && v.X < otherRect.Right &&
                   v.Y > otherRect.Top && v.Y < otherRect.Bottom) return true;
            }

            return hasLineCollision(Verticies[1], Verticies[2],
                new Point(otherRect.Left, otherRect.Top), new Point(otherRect.Right, otherRect.Bottom));
        }

        public bool Intersects(Triangle otherTriangle)
        {
            return false;
        }

        bool hasLineCollision(Point a, Point b, Point c, Point d)
        {
                float denominator = ((b.X - a.X) * (d.Y - c.Y)) - ((b.Y - a.Y) * (d.X - c.X));
                float numerator1 = ((a.Y - c.Y) * (d.X - c.X)) - ((a.X - c.X) * (d.Y - c.Y));
                float numerator2 = ((a.Y - c.Y) * (b.X - a.X)) - ((a.X - c.X) * (b.Y - a.Y));

                if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

                float r = numerator1 / denominator;
                float s = numerator2 / denominator;

                return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }

    }
}
