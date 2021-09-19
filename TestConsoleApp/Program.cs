using System;
using System.IO;
using Infrastructure.Models;
using Infrastructure.Reader;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var objFileReader = new ObjFileReader(@"D:\7 сем\АКГ\test.obj");

            var objModel = objFileReader.ReadObjModel();

            foreach (var sp in objModel.SpaceVertices)
            {
                Console.WriteLine($" u - {sp.U}, v - {sp.V}, w - {sp.W}");
            }
            
            Console.WriteLine("--------");

            foreach (var sp in objModel.GeometricVertices)
            {
                Console.WriteLine($" x - {sp.X}, y - {sp.Y}, z - {sp.Z}, w - {sp.W}");
            }

            Console.WriteLine("--------");

            foreach (var sp in objModel.TextureCoordinates)
            {
                Console.WriteLine($" u - {sp.U}, v - {sp.V}, w - {sp.W}");
            }

            Console.WriteLine("--------");

            foreach (var sp in objModel.VertexNormals)
            {
                Console.WriteLine($" x - {sp.X}, y - {sp.Y}, z - {sp.Z}");
            }


            Console.WriteLine("--------");

            foreach (var sp in objModel.PolygonalElements)
            {
                for (var i = 0; i < sp.GeometricVertices.Count; i++)
                {
                    Console.WriteLine($"v - {sp.GeometricVertices[i]}  - vn - {sp.VertexNormals[i]}");
                }
            }
        }
    }
}
