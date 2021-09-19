using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Models.Common;

namespace Infrastructure.Models
{
    public class PolygonalElement : IElement
    {
        public const string Name = "f";
        public const int MinArrayLength = 4;

        public readonly List<uint> TextureCoordinates;
        public readonly List<uint> GeometricVertices;
        public readonly List<uint> VertexNormals;

        public PolygonalElement()
        {
            TextureCoordinates = new List<uint>();
            GeometricVertices = new List<uint>();
            VertexNormals = new List<uint>();
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

            foreach (var element in elements[1..])
            {
                var numb = element.Count(x => x.Equals('/'));
                if (numb > 0)
                {
                    switch (numb)
                    {
                        case 1:
                        {
                            var localElements = element.Split('/');
                            if (localElements.Length < 2)
                            {
                                throw new ArgumentException($"Error in coordinates");
                            }

                            if (!uint.TryParse(localElements[0], out var v))
                            {
                                throw new ArgumentException($"Couldn't convert v element to uint");
                            }

                            if (!uint.TryParse(localElements[1], out var vt))
                            {
                                throw new ArgumentException($"Couldn't convert vt element to uint");
                            }

                            GeometricVertices.Add(v);
                            TextureCoordinates.Add(vt);
                            break;
                        }
                        case 2:
                            if (element.Contains("//"))
                            {
                                var localElements = element.Split("//");
                                if (localElements.Length < 2)
                                {
                                    throw new ArgumentException($"Error in coordinates");
                                }

                                if (!uint.TryParse(localElements[0], out var v))
                                {
                                    throw new ArgumentException($"Couldn't convert v element to uint");
                                }

                                if (!uint.TryParse(localElements[1], out var vn))
                                {
                                    throw new ArgumentException($"Couldn't convert vn element to uint");
                                }

                                GeometricVertices.Add(v);
                                VertexNormals.Add(vn);
                            }
                            else
                            {
                                var localElements = element.Split("/");
                                if (localElements.Length < 3)
                                {
                                    throw new ArgumentException($"Error in coordinates");
                                }

                                if (!uint.TryParse(localElements[0], out var v))
                                {
                                    throw new ArgumentException($"Couldn't convert v element to uint");
                                }

                                if (!uint.TryParse(localElements[1], out var vt))
                                {
                                    throw new ArgumentException($"Couldn't convert vt element to uint");
                                }

                                if (!uint.TryParse(localElements[2], out var vn))
                                {
                                    throw new ArgumentException($"Couldn't convert vn element to uint");
                                }

                                GeometricVertices.Add(v);
                                VertexNormals.Add(vn);
                                TextureCoordinates.Add(vt);
                            }
                            break;
                    }
                }
                else
                {
                    var localElements = element.Split(' ');

                    foreach (var localElement in localElements)
                    {
                        if (!uint.TryParse(localElement, out var v))
                        {
                            throw new ArgumentException($"Couldn't convert v element to uint");
                        }

                        GeometricVertices.Add(v);
                    }
                }
            }
        }
    }
}