using System.Numerics;

namespace Infrastructure.Space
{
    public class ViewPortSpace
    {
        public float Width { get; }
        public float Height { get; }
        public float Xmin { get; }
        public float Ymin { get; }

        public Matrix4x4 Matrix { get; }
        public Matrix4x4 TransposeMatrix { get; }


        public ViewPortSpace(float width, float height, float xmin, float ymin)
        {
            Width = width;
            Height = height;
            Xmin = xmin;
            Ymin = ymin;

            Matrix = new Matrix4x4
            {
                M11 = width / 2,
                M22 = - height / 2,
                M33 = 1,
                M44 = 1,
                M14 = xmin + width / 2,
                M24 = ymin + height / 2

            };

            TransposeMatrix = Matrix4x4.Transpose(Matrix);
        }
    }
}