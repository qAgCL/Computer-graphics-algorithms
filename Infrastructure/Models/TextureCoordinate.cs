using System;
using System.Globalization;
using Infrastructure.Models.Common;

namespace Infrastructure.Models
{
    public class TextureCoordinate : IElement
    {
        public const string Name = "vt";
        public const int MinArrayLength = 2;

        public double U { get; set; }
        public double V { get; set; }
        public double W { get; set; }

        public TextureCoordinate() { }
        public TextureCoordinate(double u, double v = 0, double w = 0)
        {
            U = u;
            V = v;
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

            if (!double.TryParse(elements[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var u))
            {
                throw new ArgumentException($"Couldn't convert u element to double");
            }

            U = u;

            if (elements.Length < MinArrayLength + 1)
            {
                return;
            }

            if (!double.TryParse(elements[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
            {
                throw new ArgumentException($"Couldn't convert v element to double");
            }

            V = v;

            if (elements.Length < MinArrayLength + 2)
            {
                return;
            }

            if (!double.TryParse(elements[3], NumberStyles.Any, CultureInfo.InvariantCulture, out var w))
            {
                throw new ArgumentException($"Couldn't convert w element to double");
            }

            W = w;
        }
    }
}