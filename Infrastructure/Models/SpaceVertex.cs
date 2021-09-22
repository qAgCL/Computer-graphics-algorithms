using System;
using System.Globalization;
using Infrastructure.Models.Common;

namespace Infrastructure.Models
{
    public class SpaceVertex : IElement
    {
        public const string Name = "vp";
        public const int MinArrayLength = 2;

        public float U { get; set; }
        public float V { get; set; }
        public float W { get; set; }

        public SpaceVertex() { }
        public SpaceVertex(float u, float v = 0, float w = 0)
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

            if (!float.TryParse(elements[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var u))
            {
                throw new ArgumentException($"Couldn't convert u element to float");
            }

            U = u;

            if (elements.Length < MinArrayLength + 1)
            {
                return;
            }

            if (!float.TryParse(elements[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
            {
                throw new ArgumentException($"Couldn't convert v element to float");
            }

            V = v;

            if (elements.Length < MinArrayLength + 2)
            {
                return;
            }

            if (!float.TryParse(elements[3], NumberStyles.Any, CultureInfo.InvariantCulture, out var w))
            {
                throw new ArgumentException($"Couldn't convert w element to float");
            }

            W = w;
        }
    }
}