using System;
using System.Globalization;
using Infrastructure.Models.Common;

namespace Infrastructure.Models
{
    public class VertexNormal : IElement
    {
        public const string Name = "vn";
        public const int MinArrayLength = 4;

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public VertexNormal() { }
        public VertexNormal(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
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
        }
    }
}