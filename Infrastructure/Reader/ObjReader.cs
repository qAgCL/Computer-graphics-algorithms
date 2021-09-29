using System;
using System.Collections.Generic;
using System.Numerics;
using Infrastructure.Models;

namespace Infrastructure.Reader
{
    public abstract class ObjReader
    {
        public abstract ObjModel ReadObjModel();

        public ObjModel GetObjModel(string[] lines)
        {
            if (lines == null)
            {
                throw new ArgumentNullException();
            }

            var objModel = new ObjModel();
            foreach (var line in lines)
            {
                var lineElements = line.Replace("  ", " ").Split(' ');
                if (lineElements.Length == 0)
                {
                    continue;
                }

                switch (lineElements[0])
                {
                    case GeometricVertex.Name:
                        objModel.GeometricVertices.Add((uint)objModel.GeometricVertices.Count + 1, GeometricVertex.FieldFromStringArray(lineElements));
                        objModel.TransformGeometricVertices.Add((uint)objModel.GeometricVertices.Count, Vector4.One);
                        break;
                    case SpaceVertex.Name:
                        objModel.SpaceVertices.Add((uint)objModel.SpaceVertices.Count + 1, SpaceVertex.FieldFromStringArray(lineElements));
                        break;
                    case TextureCoordinate.Name:
                        objModel.TextureCoordinates.Add((uint)objModel.TextureCoordinates.Count + 1, TextureCoordinate.FieldFromStringArray(lineElements));
                        break;
                    case VertexNormal.Name:
                        objModel.VertexNormals.Add((uint)objModel.VertexNormals.Count + 1, VertexNormal.FieldFromStringArray(lineElements));
                        break;
                    case PolygonalElement.Name:
                        objModel.PolygonalElements.Add(PolygonalElement.FieldFromStringArray(lineElements));
                        break;
                }

            }

            objModel.Points = new List<Vector2>();

            return objModel;
        }
    }
}