using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Infrastructure.Models.Common;

namespace Infrastructure.Models
{
    public class GeometricVertex : IElement
    {
        public const string Name = "v";
        public const int MinArrayLength = 4;

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; } = 1;

        public GeometricVertex(){}
        public GeometricVertex(float x, float y, float z, float w = 1)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public void FieldFromStringArray(string[] elements)
        {
            if (elements.Length < MinArrayLength)
            {
                throw new ArgumentException($"Elements array must be more or equal {MinArrayLength}");
            }

            if (!elements[0].Equals(Name, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ArgumentException($"Fist array must be equal {Name}");
            }

            if (!float.TryParse(elements[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var x))
            {
                throw new ArgumentException($"Couldn't convert x element to float");
            }

            if (!float.TryParse(elements[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var y))
            {
                throw new ArgumentException($"Couldn't convert y element to float");
            }

            if (!float.TryParse(elements[3], NumberStyles.Any, CultureInfo.InvariantCulture, out var z))
            {
                throw new ArgumentException($"Couldn't convert z element to float");
            }

            X = x;
            Y = y;
            Z = z;

            if (elements.Length < MinArrayLength + 1)
            {
                return;
            }

            if (!float.TryParse(elements[4], NumberStyles.Any, CultureInfo.InvariantCulture, out var w))
            {
                throw new ArgumentException($"Couldn't convert w element to float");
            }

            W = w;
        }

        public static explicit operator Vector4(GeometricVertex vector) => new (vector.X, vector.Y , vector.Z, vector.W);
    }
}