using System;
using System.Globalization;
using System.Numerics;

namespace Infrastructure.Models
{
    public static class GeometricVertex
    {
        public const string Name = "v";
        public const int MinArrayLength = 4;

        public static Vector4 FieldFromStringArray(string[] elements)
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

            if (elements.Length < MinArrayLength + 1)
            {
                return new Vector4(x, y, z, 1);
            }

            return !float.TryParse(elements[4], NumberStyles.Any, CultureInfo.InvariantCulture, out var w)
                ? throw new ArgumentException($"Couldn't convert w element to float")
                : new Vector4(x, y, z, w);
        }
    }
}