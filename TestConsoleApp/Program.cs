using System;
using System.IO;
using System.Linq;
using System.Numerics;
using Infrastructure.Models;
using Infrastructure.Reader;
using Infrastructure.Space;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var objReader = new ObjFileReader(@"D:\7 сем\АКГ\chair.obj");
            var objModel = objReader.ReadObjModel();

            
            var projectionSpace = new ProjectionSpace(1000, 600, 100f, 0.1f);
            var viewPortSpace = new ViewPortSpace(1000, 600, -15, -15);
            var viewSpace = new ViewSpace(new Vector3(5, 5, 5), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            var vectors = objModel.GeometricVertices.Select(vector => (Vector4) vector).ToArray();

            var coordinates = vectors.Select(x => Vector4.Transform(Vector4.Transform(Vector4.Transform(x, viewSpace.TransposeMatrix), projectionSpace.TransposeMatrix), viewPortSpace.TransposeMatrix));

            foreach (var cor in coordinates)
            {
                Console.WriteLine(cor.ToString());
            }
            var vectorPolygons = objModel.PolygonalElements.Where(plg => plg.TextureCoordinates.Count == 0 && plg.VertexNormals.Count == 0);
        }
    }
}
