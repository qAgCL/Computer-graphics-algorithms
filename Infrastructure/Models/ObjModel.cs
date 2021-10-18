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

        public List<List<Vector2>> PointsPerPolygon;
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
        public float AngleX { get; set; }
        public float AngleZ { get; set; }
        public float AngleY { get; set; }

        public ObjModel()
        {
            SpaceVertices = new Dictionary<uint, Vector3>();
            TextureCoordinates = new Dictionary<uint, Vector3>();
            VertexNormals = new Dictionary<uint, Vector3>();
            GeometricVertices = new Dictionary<uint, Vector4>();
            PolygonalElements = new List<PolygonalElement>();
            TransformGeometricVertices = new Dictionary<uint, Vector4>();

            Points = new List<Vector2>();
            Scale = 1;
        }

        public void TransformHardVertices()
        {
            ScaleMatrix = Matrix4x4.CreateScale(Scale);
            TranslationMatrix = Matrix4x4.CreateTranslation(TranslationX, TranslationY, TranslationZ);
            RotationMatrixX = Matrix4x4.CreateRotationX(AngleX);
            RotationMatrixY = Matrix4x4.CreateRotationY(AngleY);
            RotationMatrixZ = Matrix4x4.CreateRotationZ(AngleZ);

            TransformMatrix =  RotationMatrixX * RotationMatrixY * RotationMatrixZ * ScaleMatrix * TranslationMatrix * ViewSpace * ProjectionSpace;
            Parallel.ForEach(Partitioner.Create(GeometricVertices.Keys), key =>
            {
                var vector = Vector4.Transform(GeometricVertices[key], TransformMatrix);
                vector = Vector4.Divide(vector, vector.W);
                TransformGeometricVertices[key] = Vector4.Transform(vector, ViewPortSpace.TransposeMatrix);
            });

        }

        public List<Vector2> CalculatePoints()
        {
            
            Parallel.ForEach(Partitioner.Create(0, PolygonalElements.Count),  range =>
            {
                for (var i = range.Item1; i < range.Item2; i++)
                {
                    PointsPerPolygon[i].Clear();

                    for (var j = 0; j < PolygonalElements[i].GeometricVertices.Count - 1; j++)
                    {
                        PointsPerPolygon[i].AddRange(Viewer.DdaLines(TransformGeometricVertices[PolygonalElements[i].GeometricVertices[j]],
                            TransformGeometricVertices[PolygonalElements[i].GeometricVertices[j + 1]], Width, Height));
                    }

                    PointsPerPolygon[i].AddRange(Viewer.DdaLines(TransformGeometricVertices[PolygonalElements[i].GeometricVertices[^1]],
                        TransformGeometricVertices[PolygonalElements[i].GeometricVertices[0]], Width, Height));
                }
            });

            Points.Clear();
            Points.AddRange(PointsPerPolygon.SelectMany(x => x));

            return Points;
        }
    }
}