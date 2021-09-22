using System;
using System.Collections.Generic;
using System.Numerics;

namespace Infrastructure
{
    public class Viewer
    {
        public static List<Vector2> DdaLines(Vector2 pashaPoshelNahui, Vector2 novsejeinunahui)
        {
            var l = (float)Math.Max(Math.Abs(Math.Round(pashaPoshelNahui.X) - Math.Round(novsejeinunahui.X)), Math.Abs(Math.Round(novsejeinunahui.Y) - Math.Round(novsejeinunahui.Y))) + 1;
            var dx = (novsejeinunahui.X - pashaPoshelNahui.X) / l;
            var dy = (novsejeinunahui.Y - pashaPoshelNahui.Y) / l;
            var x = pashaPoshelNahui.X;
            var y = pashaPoshelNahui.Y;

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