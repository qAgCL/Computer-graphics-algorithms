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
            var objReader = new ObjFileReader(@"D:\Untitled.obj");
            var objModel = objReader.ReadObjModel();

            var projectionSpace = new ProjectionSpace(300, 300, 100f, 0.1f);
            var viewPortSpace = new ViewPortSpace(300, 300, -0.1f, -0.1f);
            var viewSpace = new ViewSpace(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));


            var vectors = objModel.GeometricVertices.Select(vector => (Vector4) vector).ToArray();

            var coordinates = vectors.Select(x => Vector4.Transform(Vector4.Transform(Vector4.Transform(x, viewSpace.TransposeMatrix), projectionSpace.TransposeMatrix), viewPortSpace.TransposeMatrix)).ToArray();

            foreach (var cor in coordinates)
            {
                Console.WriteLine(cor.ToString());
            }

            var vectorPolygons = objModel.PolygonalElements.Where(plg => plg.TextureCoordinates.Count == 0 && plg.VertexNormals.Count == 0);
            var points = new List<List<Vector2>>();
            foreach (var vectorPolygon in vectorPolygons)
            {
                var test = new List<Vector2>();
                var pairs = new List<Pair>();
                for (var i = 0; i < vectorPolygon.GeometricVertices.Count - 1; i++)
                {
                    pairs.Add(new Pair()
                    {
                        First = coordinates[vectorPolygon.GeometricVertices[i] -1],
                        Second = coordinates[vectorPolygon.GeometricVertices[i] - 1]
                    });
                }

                pairs.Add(new Pair()
                {
                    First = coordinates[vectorPolygon.GeometricVertices[0] -1],
                    Second = coordinates[vectorPolygon.GeometricVertices[^1] -1]
                });

                foreach (var pair in pairs)
                {
                    test.AddRange(Viewer.DdaLines(new Vector2(pair.First.X, pair.First.Y), new Vector2(pair.Second.X, pair.Second.Y)));
                }

                points.Add(test);
            }
        }

        public class Pair
        {
            public Vector4 First { get; set; }
            public Vector4 Second { get; set; }
        }
    }
}
