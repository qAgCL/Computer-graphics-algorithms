using System;
using System.Numerics;

namespace Infrastructure.Space
{
    public class ViewSpace
    {
        public Vector3 Eye { get; }
        public Vector3 Target { get; }
        public Vector3 Up { get; }
        public Matrix4x4 Matrix { get; }
        public Matrix4x4 TransposeMatrix { get; }


        public ViewSpace(Vector3 eye, Vector3 target, Vector3 up)
        {
            Eye = eye;
            Target = target;
            Up = up;

            Matrix = new Matrix4x4();

            var zAxis = Vector3.Normalize(eye - target);
            var xAxis = Vector3.Normalize(Vector3.Cross(up, zAxis));
            var yAxis = Vector3.Cross(zAxis, xAxis);

            var matrix1 = new Matrix4x4(xAxis.X, xAxis.Y, xAxis.Z, 0, yAxis.X, yAxis.Y, yAxis.Z, 0, zAxis.X, zAxis.Y, zAxis.Z, 0, 0, 0, 0, 1);
            var matrix2 = new Matrix4x4(1, 0, 0, -eye.X, 0, 1, 0, -eye.Y, 0, 0, 1, -eye.Z, 0, 0, 0, 1);
            Console.WriteLine(matrix1);
            Console.WriteLine(matrix2);
            Matrix = matrix1 * matrix2;
            TransposeMatrix = Matrix4x4.Transpose(Matrix);
        }
    }
}