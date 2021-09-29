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

            var objReader = new ObjFileReader(@"D:\7 сем\АКГ\chair.obj");
            var objModel = objReader.ReadObjModel();
            objModel.TransformVertices();
            var points = objModel.CalculatePoints();


            RenderOptions.SetBitmapScalingMode(Image, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(Image, EdgeMode.Aliased);

            var writeableBitmap = new WriteableBitmap(
                (int)1000,
                (int)1000,
                96,
                96,
                PixelFormats.Bgr32,
                null);

            Image.Source = writeableBitmap;

            Image.Stretch = Stretch.None;
            Image.HorizontalAlignment = HorizontalAlignment.Left;
            Image.VerticalAlignment = VerticalAlignment.Top;


            foreach (var po in points)
            {
                foreach (var p in po)
                {
                    var column = (int)Math.Round(p.X);
                    var row = (int)Math.Round(p.Y);

                    if (column < 0 || row < 0 || column >= 1000 || row >= 1000)
                    {
                        continue;
                    }
                    try
                    {
                        writeableBitmap.Lock();

                        unsafe
                        {
                            IntPtr pBackBuffer = writeableBitmap.BackBuffer;

                            pBackBuffer += row * writeableBitmap.BackBufferStride;
                            pBackBuffer += column * 4;

                            int color_data = int.MaxValue;

                            *((int*)pBackBuffer) = color_data;
                        }

                        writeableBitmap.AddDirtyRect(new Int32Rect(column, row, 1, 1));
                    }
                    finally
                    {
                        writeableBitmap.Unlock();
                    }
                }
            }
        }
    }
}
