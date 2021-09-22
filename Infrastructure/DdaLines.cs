using System;
using System.Collections.Generic;
using System.Numerics;

namespace Infrastructure
{
    public static class Viewer
    {
        public static List<Vector2> DdaLines(Vector2 coordinate1, Vector2 coordinate2)
        {
            var l = (float)Math.Max(Math.Abs(Math.Round(coordinate1.X) - Math.Round(coordinate2.X)), Math.Abs(Math.Round(coordinate2.Y) - Math.Round(coordinate2.Y))) + 1;
            var dx = (coordinate2.X - coordinate1.X) / l;
            var dy = (coordinate2.Y - coordinate1.Y) / l;
            var x = coordinate1.X;
            var y = coordinate1.Y;

            var result = new List<Vector2>();
            for (var i = 0; i < l; i++)
            {
                result.Add(new Vector2(x, y));
                x += dx;
                y += dy;
            }

            return result;
        }
    }
}