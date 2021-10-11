using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.ExceptionServices;

namespace Infrastructure
{
    public static class Viewer
    {
        public static List<Vector2> DdaLines(Vector4 coordinate1, Vector4 coordinate2)
        {
            var first = ((int)Math.Round(coordinate1.X), (int)Math.Round(coordinate1.Y));
            var second = ((int)Math.Round(coordinate2.X), (int)Math.Round(coordinate2.Y));

            var result = new List<Vector2>();
   
            var deltaX = Math.Abs(second.Item1 - first.Item1);
            var deltaY = Math.Abs(second.Item2 - first.Item2);

            var length = Math.Max(deltaX, deltaY);
            if (length == 0)
            {
                result.Add(new Vector2(first.Item1, first.Item2));
                return result;
            }

            var dx = (second.Item1 - first.Item1) / (float)length;
            var dy = (second.Item2 - first.Item2) / (float)length;

            var x = (float) first.Item1;
            var y = (float) first.Item2;

            for (var i = 0; i < length + 1; i++)
            {
                result.Add(new Vector2(x, y));
                x += dx;
                y += dy;
            }

            return result;
        }
    }
}