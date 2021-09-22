using System.Numerics;

namespace Infrastructure.Space
{
    public class ProjectionSpace
    {
        public float Width { get; }
        public float Height { get; }
        public float Zfar { get; }
        public float Znear { get; }

        public Matrix4x4 Matrix { get; }
        public Matrix4x4 TransposeMatrix { get; }

        public ProjectionSpace(float width, float height, float zfar, float znear)
        {
            Width = width;
            Height = height;
            Zfar = zfar;
            Znear = znear;

            Matrix = new Matrix4x4
            {
                M11 = 2  / width,
                M22 = 2 / height,
                M33 = 1 / (znear - zfar),
                M34 = znear / (znear - zfar),
                M44 = 1
            };

            TransposeMatrix = Matrix4x4.Transpose(Matrix);
        }
    }
}