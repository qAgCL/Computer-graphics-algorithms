using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Reader;
using Infrastructure.Space;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                var numb = float.Parse(Console.ReadLine());

                Console.WriteLine((int)numb);
            }

        }

    }
}
