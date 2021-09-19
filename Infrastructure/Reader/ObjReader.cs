using System;
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
                        var geometricVertex = new GeometricVertex();
                        geometricVertex.FieldFromStringArray(lineElements);
                        objModel.GeometricVertices.Add(geometricVertex);
                        break;
                    case SpaceVertex.Name:
                        var spaceVertex = new SpaceVertex();
                        spaceVertex.FieldFromStringArray(lineElements);
                        objModel.SpaceVertices.Add(spaceVertex);
                        break;
                    case TextureCoordinate.Name:
                        var textureCoordinate = new TextureCoordinate();
                        textureCoordinate.FieldFromStringArray(lineElements);
                        objModel.TextureCoordinates.Add(textureCoordinate);
                        break;
                    case VertexNormal.Name:
                        var vertexNormal = new VertexNormal();
                        vertexNormal.FieldFromStringArray(lineElements);
                        objModel.VertexNormals.Add(vertexNormal);
                        break;
                    case PolygonalElement.Name:
                        var polygonalElement = new PolygonalElement();
                        polygonalElement.FieldFromStringArray(lineElements);
                        objModel.PolygonalElements.Add(polygonalElement);
                        break;
                }
            }

            return objModel;
        }
    }
}