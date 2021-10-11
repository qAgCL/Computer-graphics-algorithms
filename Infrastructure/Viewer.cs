using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class Viewer
    {
        public static List<Vector2> DdaLines(Vector4 coordinate1, Vector4 coordinate2, int width, int height)
        {
            var result = new List<Vector2>();
   
            var length = Math.Max(Math.Abs(coordinate1.X - coordinate2.X), Math.Abs(coordinate1.Y - coordinate2.Y));
            var dx = (coordinate2.X - coordinate1.X) / length;
            var dy = (coordinate2.Y - coordinate1.Y) / length;

            var x = coordinate1.X;
            var y = coordinate1.Y;
            for (var i = 0; i <= length; i++)
            {
                if (x < 0 || y < 0 || x >= width || y >= height)
                {
                    continue;
                }

                result.Add(new Vector2(x, y));
                x += dx;
                y += dy;
            }

            return result;
        }
    }
}