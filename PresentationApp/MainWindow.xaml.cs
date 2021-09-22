using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Infrastructure;
using Infrastructure.Reader;
using Infrastructure.Space;

namespace PresentationApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var objReader = new ObjFileReader(@"D:\chair.obj");
            var objModel = objReader.ReadObjModel();

            var projectionSpace = new ProjectionSpace(300, 300, 100f, 0.1f);
            var viewPortSpace = new ViewPortSpace(300, 300, -0.1f, -0.1f);
            var viewSpace = new ViewSpace(new Vector3(1, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            var vectors = objModel.GeometricVertices.Select(vector => (Vector4)vector).ToArray();

            var coordinates = vectors.Select(x => Vector4.Transform(Vector4.Transform(Vector4.Transform(x, viewSpace.TransposeMatrix), projectionSpace.TransposeMatrix), viewPortSpace.TransposeMatrix)).ToArray();
            foreach (var cor in coordinates)
            {
                Console.WriteLine(cor.ToString());
            }

            var vectorPolygons = objModel.PolygonalElements;
            foreach (var vectorPolygon in vectorPolygons)
            {
                for (var i = 0; i < vectorPolygon.GeometricVertices.Count - 1; i++)
                {
                    var horL = new Line
                    {
                        X1 = coordinates[vectorPolygon.GeometricVertices[i] - 1].X,
                        X2 = coordinates[vectorPolygon.GeometricVertices[i+1] - 1].X,
                        Y1 = coordinates[vectorPolygon.GeometricVertices[i] - 1].Y,
                        Y2 = coordinates[vectorPolygon.GeometricVertices[i+1] - 1].Y,
                        Stroke = Brushes.Black
                    };
                    mainGrid.Children.Add(horL);
                }
            }

            var test = new Line
            {
                X1 = 500,
                X2 = 500,
                Y1 = 400,
                Y2 = 500,
                Stroke = Brushes.Black
            };
            mainGrid.Children.Add(test);
        }

        public class Pair
        {
            public Vector4 First { get; set; }
            public Vector4 Second { get; set; }
        }
    }
}
