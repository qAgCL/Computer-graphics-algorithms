using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        private readonly WriteableBitmap _imageBitMap = new(PixelHeight, PixelWidth, DpiHeight, DpiWidth, PixelFormats.Bgr32, null);
        private readonly byte[] _whiteImage = new byte[PixelHeight * PixelWidth * RgbBytesPerPixel];
        private readonly byte[] _image = new byte[PixelHeight * PixelWidth * RgbBytesPerPixel];
        private readonly ObjModel _model;

        private const float ScaleSpeed = 1f;
        private const float TranslationSpeed = 10f;
        private const float AngleSpeed = 0.25f;


        public MainWindow()
        {
            InitializeComponent();
            
            for (int i = 0; i < _whiteImage.Length; i++)
            {
                _whiteImage[i] = byte.MaxValue;
                _image[i] = byte.MaxValue;
            }
            unsafe
            {
                fixed (byte* b = _whiteImage)
                {
                    CopyMemory(_imageBitMap.BackBuffer, (IntPtr)b, (uint)_whiteImage.Length);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _imageBitMap.Lock();
                        _imageBitMap.AddDirtyRect(new Int32Rect(0, 0, _imageBitMap.PixelWidth, _imageBitMap.PixelHeight));
                        _imageBitMap.Unlock();
                    });
                }
            }

            Image.Source = _imageBitMap;
            Image.Stretch = Stretch.None;
            Image.HorizontalAlignment = HorizontalAlignment.Left;
            Image.VerticalAlignment = VerticalAlignment.Top;

            var objReader = new ObjFileReader(@"D:\7 сем\АКГ\mustang.obj");
            _model = objReader.ReadObjModel();

            _model.Height = PixelHeight;
            _model.Width = PixelWidth;

            _model.ProjectionSpace = Matrix4x4.CreatePerspectiveFieldOfView((float)(Math.PI / 4), 1, 0.1f, 1);

            var camPos = new Vector3(0, 0, 1000);
            var camTarget = new Vector3(0, 0, 0);
            var up = new Vector3(0, 1, 0);
            var camDirection = Vector3.Normalize(Vector3.Subtract(camPos, camTarget));
            var camRight = Vector3.Normalize(Vector3.Cross(up, camDirection));
            var camUp = Vector3.Cross(camDirection, camRight);

            _model.ViewSpace = Matrix4x4.CreateLookAt(camPos, camTarget, camUp);


            _model.ViewPortSpace = new ViewPortSpace(PixelWidth, PixelHeight, 0, 0);

            ShowModel();
        }

        private void ShowModel()
        {
            _model.TransformHardVertices();
            var points = _model.CalculatePoints();
            var ptr = _imageBitMap.BackBuffer;
            unsafe 
            {
                fixed (byte* clear = _whiteImage)
                {
                    CopyMemory(ptr, (IntPtr)clear, (uint)_whiteImage.Length);
                }

                Parallel.ForEach(points, (point) =>
                {
                    var column = (int)point.X;
                    var row = (int)point.Y;

                    var localPtr = ptr;
                    localPtr += row * PixelWidth * RgbBytesPerPixel;
                    localPtr += column * RgbBytesPerPixel;
                    *((int*)localPtr) = 0;
                });

                Application.Current.Dispatcher.Invoke(() =>
                {
                    _imageBitMap.Lock();
                    _imageBitMap.AddDirtyRect(new Int32Rect(0, 0, _imageBitMap.PixelWidth, _imageBitMap.PixelHeight));
                    _imageBitMap.Unlock();
                });
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Add)
            { 
                _model.Scale += ScaleSpeed;
            }

            if (e.Key == Key.Subtract)
            {
                _model.Scale -= ScaleSpeed;
            }

            if (e.Key == Key.Up)
            {
                _model.TranslationY += TranslationSpeed;
            }

            if (e.Key == Key.Down)
            {
                _model.TranslationY -= TranslationSpeed;
            }

            if (e.Key == Key.Left)
            {
                _model.TranslationX -= TranslationSpeed;
            }

            if (e.Key == Key.Right)
            {
                _model.TranslationX += TranslationSpeed;
            }

            if (e.Key == Key.NumPad7)
            {
                _model.AngleX += AngleSpeed;
            }

            if (e.Key == Key.NumPad8)
            {
                _model.AngleX -= AngleSpeed;
            }

            if (e.Key == Key.NumPad4)
            {
                _model.AngleY += AngleSpeed;
            }

            if (e.Key == Key.NumPad5)
            {
                _model.AngleY -= AngleSpeed;
            }

            if (e.Key == Key.NumPad1)
            {
                _model.AngleZ += AngleSpeed;
            }

            if (e.Key == Key.NumPad2)
            {
                _model.AngleZ -= AngleSpeed;
            }

            ShowModel();
        }
    }
}
