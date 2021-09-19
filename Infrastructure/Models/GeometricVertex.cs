using System;
using System.Globalization;
using Infrastructure.Models.Common;

namespace Infrastructure.Models
{
    public class GeometricVertex : IElement
    {
        public const string Name = "v";
        public const int MinArrayLength = 4;

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; } = 1;

        public GeometricVertex(){}
        public GeometricVertex(double x, double y, double z, double w = 1)
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

            if (!double.TryParse(elements[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var x))
            {
                throw new ArgumentException($"Couldn't convert x element to double");
            }

            if (!double.TryParse(elements[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var y))
            {
                throw new ArgumentException($"Couldn't convert y element to double");
            }

            if (!double.TryParse(elements[3], NumberStyles.Any, CultureInfo.InvariantCulture, out var z))
            {
                throw new ArgumentException($"Couldn't convert z element to double");
            }

            X = x;
            Y = y;
            Z = z;

            if (elements.Length < MinArrayLength + 1)
            {
                return;
            }

            if (!double.TryParse(elements[4], NumberStyles.Any, CultureInfo.InvariantCulture, out var w))
            {
                throw new ArgumentException($"Couldn't convert w element to double");
            }

            W = w;
        }
    }
}