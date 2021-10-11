using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Infrastructure.Space;

namespace Infrastructure.Models
{
    public class ObjModel
    {
        public readonly Dictionary<uint, Vector3> SpaceVertices;
        public readonly Dictionary<uint, Vector3> TextureCoordinates;
        public readonly Dictionary<uint, Vector3> VertexNormals;
        public readonly Dictionary<uint, Vector4> GeometricVertices;
        public readonly List<PolygonalElement> PolygonalElements;

        public List<Vector2> Points;
        
        public readonly Dictionary<uint, Vector4> TransformGeometricVertices;
        public Matrix4x4 TransformMatrix;

        public Matrix4x4 ProjectionSpace { get; set; }
        public Matrix4x4 ViewSpace { get; set; }
        public ViewPortSpace ViewPortSpace { get; set; }

        public Matrix4x4 ScaleMatrix { get; set; }
        public Matrix4x4 RotationMatrixX { get; set; }
        public Matrix4x4 RotationMatrixY { get; set; }
        public Matrix4x4 RotationMatrixZ { get; set; }
        public Matrix4x4 TranslationMatrix { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public float TranslationX { get; set; }
        public float TranslationY { get; set; }
        public float TranslationZ { get; set; }

        public float Scale { get; set; }
        public double AngleX { get; set; }
        public double AngleZ { get; set; }
        public double AngleY { get; set; }

        public ObjModel()
        {
            SpaceVertices = new Dictionary<uint, Vector3>();
            TextureCoordinates = new Dictionary<uint, Vector3>();
            VertexNormals = new Dictionary<uint, Vector3>();
            GeometricVertices = new Dictionary<uint, Vector4>();
            PolygonalElements = new List<PolygonalElement>();
            TransformGeometricVertices = new Dictionary<uint, Vector4>();

            Scale = 1;
        }

        public void TransformHardVertices()
        {
            ScaleMatrix = Matrix4x4.CreateScale(Scale);
            TranslationMatrix = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, TranslationX, 0, 1, 0, TranslationY, 0, 0, 1, TranslationZ, 0, 0, 0, 1));
            RotationMatrixX = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, 0, 0, (float)Math.Cos(AngleX), -(float)Math.Sin(AngleX), 0, 0, (float)Math.Sin(AngleX), (float)Math.Cos(AngleX), 0, 0, 0, 0, 1));
            RotationMatrixY = Matrix4x4.Transpose(new Matrix4x4((float)Math.Cos(AngleY), 0, (float)Math.Sin(AngleY), 0, 0, 1, 0, 0, -(float)Math.Sin(AngleY), 0, (float)Math.Cos(AngleY), 0, 0, 0, 0, 1));
            RotationMatrixZ = Matrix4x4.Transpose(new Matrix4x4((float)Math.Cos(AngleZ), -(float)Math.Sin(AngleZ), 0, 0, (float)Math.Sin(AngleZ), (float)Math.Cos(AngleZ), 0, 0, 0, 0, 1, 0, 0, 0, 0, 1));

            TransformMatrix =  RotationMatrixX * RotationMatrixY * RotationMatrixZ * ScaleMatrix * TranslationMatrix * ViewSpace * ProjectionSpace;

            Parallel.ForEach(GeometricVertices, (keyValuePair) =>
            {
                var vector = Vector4.Transform(keyValuePair.Value, TransformMatrix);
                vector = Vector4.Divide(vector, vector.W);
                TransformGeometricVertices[keyValuePair.Key] = Vector4.Transform(vector, ViewPortSpace.TransposeMatrix);
            });

        }

        public List<Vector2> CalculatePoints()
        {
            Points.Clear();
            foreach (var polygonalElement in PolygonalElements)
            {
                for (var i = 0; i < polygonalElement.GeometricVertices.Count - 1; i++)
                {
                    Points.AddRange(Viewer.DdaLines(TransformGeometricVertices[polygonalElement.GeometricVertices[i]], TransformGeometricVertices[polygonalElement.GeometricVertices[i + 1]]));
                }

                Points.AddRange(Viewer.DdaLines(TransformGeometricVertices[polygonalElement.GeometricVertices[^1]], TransformGeometricVertices[polygonalElement.GeometricVertices[0]]));
            }

            return Points.ToList();
        }
    }
}