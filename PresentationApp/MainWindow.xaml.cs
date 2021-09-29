using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
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
using Infrastructure.Models;
using Infrastructure.Reader;
using Infrastructure.Space;

namespace PresentationApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region External
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);
        #endregion
        private const int PixelHeight = 1000;
        private const int PixelWidth = 1000;
        private const int DpiHeight = 96;
        private const int DpiWidth = 96;
        private const int RgbBytesPerPixel = 4;
        private WriteableBitmap _bitMap = new(PixelHeight, PixelWidth, DpiHeight, DpiWidth, PixelFormats.Bgr32, null);
        private readonly byte[] _blankImage = new byte[PixelHeight * PixelWidth * RgbBytesPerPixel];
        private ObjModel _objModel;

        private unsafe void FastClear()
        {
            fixed (byte* b = _blankImage)
            {
                CopyMemory(_bitMap.BackBuffer, (IntPtr)b, (uint)_blankImage.Length);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _bitMap.Lock();
                    _bitMap.AddDirtyRect(new Int32Rect(0, 0, _bitMap.PixelWidth, _bitMap.PixelHeight));
                    _bitMap.Unlock();
                });
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            
            for (int i = 0; i < _blankImage.Length; i++)
            {
                _blankImage[i] = byte.MaxValue;
            }

            FastClear();

            Image.Source = _bitMap;
            Image.Stretch = Stretch.None;
            Image.HorizontalAlignment = HorizontalAlignment.Left;
            Image.VerticalAlignment = VerticalAlignment.Top;
            RenderOptions.SetBitmapScalingMode(Image, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(Image, EdgeMode.Aliased);

            var objReader = new ObjFileReader(@"D:\chair.obj");
            _objModel = objReader.ReadObjModel();

            _objModel.Height = PixelHeight;
            _objModel.Width = PixelWidth;

            _objModel.ProjectionSpace = new ProjectionSpace(PixelWidth, PixelHeight, 100f, 0.1f);
            _objModel.ViewSpace = new ViewSpace(new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        }

        private void ShowModel()
        {
            _objModel.TransformHardVertices();
            FastClear();

            var points = _objModel.CalculatePoints();
            foreach (var po in points)
            {
                foreach (var p in po)
                {
                    var column = (int)Math.Round(p.X);
                    var row = (int)Math.Round(p.Y);

                    if (column < 0 || row < 0 || column >= PixelWidth || row >= PixelHeight)
                    {
                        continue;
                    }
                    try
                    {
                        _bitMap.Lock();

                        unsafe
                        {
                            var pBackBuffer = _bitMap.BackBuffer;
                            pBackBuffer += row * _bitMap.BackBufferStride;
                            pBackBuffer += column * 4;
                            *((int*)pBackBuffer) = 0;
                        }

                        _bitMap.AddDirtyRect(new Int32Rect(column, row, 1, 1));
                    }
                    finally
                    {
                        _bitMap.Unlock();
                    }
                }
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Add)
            { 
                _objModel.Scale += 0.5f;
                ShowModel();
            }

            if (e.Key == Key.Subtract)
            {
                _objModel.Scale -= 0.5f;
                ShowModel();
            }

            if (e.Key == Key.Up)
            {
                _objModel.TranslationY += 1f;
                ShowModel();
            }

            if (e.Key == Key.Down)
            {
                _objModel.TranslationY -= 1f;
                ShowModel();
            }

            if (e.Key == Key.Left)
            {
                _objModel.TranslationX -= 1f;
                ShowModel();
            }

            if (e.Key == Key.Right)
            {
                _objModel.TranslationX += 1f;
                ShowModel();
            }
        }
    }
}
