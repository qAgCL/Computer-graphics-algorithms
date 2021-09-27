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

            var objReader = new ObjFileReader(@"D:\7 сем\АКГ\test.obj");
            var objModel = objReader.ReadObjModel();
            objModel.TransformVertices();
            var points = objModel.CalculatePoints();

            foreach (var po in points)
            {
                for (var i = 0; i < po.Count - 1; i++)
                {
                    var line = new Line
                    {
                        X1 = po[i].X,
                        X2 = po[i+1].X,
                        Y1 = po[i].Y,
                        Y2 = po[i+1].Y,
                        Stroke = Brushes.Black
                    };
                    mainGrid.Children.Add(line);
                }
            }
        }
    }
}
