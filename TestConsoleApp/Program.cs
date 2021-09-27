using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Reader;
using Infrastructure.Space;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var matrix = new Matrix4x4(1, 2, 3, 4, 8, 1, 7, 3, 1, 2, 3, 4, 8, 1, 7, 3);
            var vector = new Vector4(1, 2, 3, 4);

            var projectionSpace = new ProjectionSpace(600, 600, 100f, 0.1f);
            var viewPortSpace = new ViewPortSpace(600, 600, -0.1f, -0.1f);
            var viewSpace = new ViewSpace(new Vector3(1, 2, 1), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            Console.WriteLine(Vector4.Transform(Vector4.Transform(Vector4.Transform(vector, viewSpace.TransposeMatrix), projectionSpace.TransposeMatrix), viewPortSpace.TransposeMatrix));
            matrix = viewSpace.TransposeMatrix * projectionSpace.TransposeMatrix * viewPortSpace.TransposeMatrix;
            Console.WriteLine(matrix);


            var test = new Dictionary<uint, Vector3>() {{1, new Vector3(1, 2, 3)}};

            var casd = new Dictionary<uint, Vector3>(test);


            test[1] = new Vector3(3, 4, 6);

            Console.WriteLine(test[1]);
            Console.WriteLine(casd[1]);
        }

    }
}
