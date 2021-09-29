using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        public List<List<Vector2>> Points;
        
        public readonly Dictionary<uint, Vector4> TransformGeometricVertices;
        public Matrix4x4 TransformMatrix;

        public ProjectionSpace ProjectionSpace { get; set; }
        public ViewSpace ViewSpace { get; set; }
        public ViewPortSpace ViewPortSpace { get; set; }

        public Matrix4x4 ScaleMatrix { get; set; }
        public Matrix4x4 RotationMatrix { get; set; }
        public Matrix4x4 TranslationMatrix { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public float TranslationX { get; set; }
        public float TranslationY { get; set; }
        public float TranslationZ { get; set; }

        public float Scale { get; set; }
        public float Angle { get; set; }


        public ObjModel()
        {
            SpaceVertices = new Dictionary<uint, Vector3>();
            TextureCoordinates = new Dictionary<uint, Vector3>();
            VertexNormals = new Dictionary<uint, Vector3>();
            GeometricVertices = new Dictionary<uint, Vector4>();
            PolygonalElements = new List<PolygonalElement>();
            TransformGeometricVertices = new Dictionary<uint, Vector4>();

            Scale = 1;
            Angle = 0;

            ScaleMatrix = Matrix4x4.Transpose(new Matrix4x4(Scale, 0, 0, 0, 0, Scale, 0, 0, 0, 0, Scale, 0, 0, 0, 0, 1));
            TranslationMatrix = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, TranslationX, 0, 1, 0, TranslationY, 0, 0, 1, TranslationZ, 0, 0, 0, 1));
        }

        public void TransformHardVertices()
        {
            ScaleMatrix = Matrix4x4.Transpose(new Matrix4x4(Scale, 0, 0, 0, 0, Scale, 0, 0, 0, 0, Scale, 0, 0, 0, 0, 1));
            TranslationMatrix = Matrix4x4.Transpose(new Matrix4x4(1, 0, 0, TranslationX, 0, 1, 0, TranslationY, 0, 0, 1, TranslationZ, 0, 0, 0, 1));

            TransformMatrix = ViewSpace.TransposeMatrix * TranslationMatrix * ScaleMatrix * ProjectionSpace.TransposeMatrix;
            Parallel.ForEach(GeometricVertices, (keyValuePair) =>
            {
                TransformGeometricVertices[keyValuePair.Key] = Vector4.Transform(keyValuePair.Value, TransformMatrix);
            });

            var xMin = TransformGeometricVertices.AsParallel().Select(x => x.Value.X).Min();
            var yMin = TransformGeometricVertices.AsParallel().Select(x => x.Value.Y).Min();

            ViewPortSpace = new ViewPortSpace(Width, Height, xMin, yMin);

            TransformMatrix = ViewPortSpace.TransposeMatrix;
            Parallel.ForEach(TransformGeometricVertices, (keyValuePair) =>
            {
                TransformGeometricVertices[keyValuePair.Key] = Vector4.Transform(keyValuePair.Value, TransformMatrix);
            });
        }

        public List<List<Vector2>> CalculatePoints()
        {
            Parallel.ForEach(PolygonalElements, (polygonalElement, state, index) =>
            {
                Points[(int)index].Clear();
                
                for (var i = 0; i < polygonalElement.GeometricVertices.Count - 1; i++)
                {
                    Points[(int)index].AddRange(Viewer.DdaLines(TransformGeometricVertices[polygonalElement.GeometricVertices[i]], TransformGeometricVertices[polygonalElement.GeometricVertices[i + 1]]));
                }

                Points[(int)index].AddRange(Viewer.DdaLines(TransformGeometricVertices[polygonalElement.GeometricVertices[^1]], TransformGeometricVertices[polygonalElement.GeometricVertices[0]]));
            });

            return Points;
        }
    }
}