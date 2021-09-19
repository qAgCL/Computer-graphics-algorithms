using System;
using System.IO;
using Infrastructure.Models;

namespace Infrastructure.Reader
{
    public class ObjFileReader : ObjReader
    {
        private readonly string _filePath;
        public ObjFileReader(string filePath)
        {
            _filePath = filePath;
        }

        public override ObjModel ReadObjModel()
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                throw new ArgumentException("File path is null or empty");
            }

            if (!File.Exists(_filePath))
            {
                throw new ArgumentException("File doesn't exist");
            }

            return GetObjModel(File.ReadAllLines(_filePath));
        }
    }
}