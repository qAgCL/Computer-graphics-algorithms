using System;
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
        
        public Dictionary<uint, Vector4> TransformGeometricVertices;

        public ProjectionSpace ProjectionSpace { get; set; }
        public ViewSpace ViewSpace { get; set; }
        public ViewPortSpace ViewPortSpace { get; set; }

        public Matrix4x4 ScaleMatrix { get; set; }
        public Matrix4x4 RotationMatrix { get; set; }

        public ObjModel()
        {
            SpaceVertices = new Dictionary<uint, Vector3>();
            TextureCoordinates = new Dictionary<uint, Vector3>();
            VertexNormals = new Dictionary<uint, Vector3>();
            GeometricVertices = new Dictionary<uint, Vector4>();
            PolygonalElements = new List<PolygonalElement>();
            TransformGeometricVertices = new Dictionary<uint, Vector4>();
            ScaleMatrix = new Matrix4x4(100, 0, 0, 0, 0, 100, 0, 0, 0, 0, 100, 0, 0, 0, 0, 1);
            RotationMatrix = new Matrix4x4((float)Math.Cos(1), -(float)Math.Sin(1), 0, 0, (float)Math.Sin(1), (float)Math.Cos(1), 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

            ProjectionSpace = new ProjectionSpace(300, 300, 100f, 0.1f);
            ViewPortSpace = new ViewPortSpace(300, 300, -0.1f, -0.1f);
            ViewSpace = new ViewSpace(new Vector3(-100, 0, 0), new Vector3(0, 0, 1), new Vector3(-100, 1, 0));
        }

        public void TransformVertices()
        {
            var finalMatrix = ViewSpace.TransposeMatrix * Matrix4x4.Transpose(ScaleMatrix) * ProjectionSpace.TransposeMatrix * ViewPortSpace.TransposeMatrix;
            TransformGeometricVertices = new Dictionary<uint, Vector4>(GeometricVertices);

            Parallel.ForEach(GeometricVertices, (keyValuePair) =>
            {
                TransformGeometricVertices[keyValuePair.Key] = Vector4.Transform(keyValuePair.Value, finalMatrix);
            });
        }

        public List<List<Vector2>> CalculatePoints()
        {
            var polygons = new List<List<Vector2>>( new List<Vector2>[PolygonalElements.Count]);
            Parallel.ForEach(PolygonalElements, (polygonalElement, state, index) =>
            {
                var points = new List<Vector2>();
                for (var i = 0; i < polygonalElement.GeometricVertices.Count - 1; i++)
                {
                    points.AddRange(Viewer.DdaLines(TransformGeometricVertices[polygonalElement.GeometricVertices[i]], TransformGeometricVertices[polygonalElement.GeometricVertices[i + 1]]));
                }

                points.AddRange(Viewer.DdaLines(TransformGeometricVertices[polygonalElement.GeometricVertices[^1]], TransformGeometricVertices[polygonalElement.GeometricVertices[0]]));
                polygons[(int)index] = points;
            });

            return polygons;
        }
    }
}