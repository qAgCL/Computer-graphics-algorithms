using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Infrastructure.Models
{
    public class ObjModel
    {
        public readonly List<SpaceVertex> SpaceVertices;
        public readonly List<TextureCoordinate> TextureCoordinates;
        public readonly List<VertexNormal> VertexNormals;
        public readonly List<GeometricVertex> GeometricVertices;
        public readonly List<PolygonalElement> PolygonalElements;


        public ObjModel()
        {
            SpaceVertices = new List<SpaceVertex>();
            TextureCoordinates = new List<TextureCoordinate>();
            VertexNormals = new List<VertexNormal>();
            GeometricVertices = new List<GeometricVertex>();
            PolygonalElements = new List<PolygonalElement>();
        }
 

    }
}