using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Drawing;
using System.Windows.Interop;
using System.Drawing.Imaging;
using Rectangle = System.Windows.Shapes.Rectangle;
using Size = System.Windows.Size;

namespace FishBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int myFlag;
        private int f1flag;
        private int f2flag;

        BitmapImage img = new BitmapImage();
        BitmapImage imgPopl = new BitmapImage();
        BitmapImage imgVosk = new BitmapImage();
        public MainWindow()
        {
            InitializeComponent();

            myFlag = 0;
          //  BitmapImage img = new BitmapImage();
         //   BitmapImage imgPopl = new BitmapImage();
          //  BitmapImage imgVosk = new BitmapImage();

            Rectangle rect = new Rectangle();
            System.Drawing.Rectangle myrect = new System.Drawing.Rectangle();
            System.Drawing.Size size = new System.Drawing.Size(200,200);

            this.Loaded += delegate
            {
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Elapsed += delegate
                {
                    if (myFlag == 1)
                    {
                        this.Dispatcher.Invoke(new Action(delegate
                        {

                            Mouse.Capture(this);
                            System.Windows.Point pointToWindow = Mouse.GetPosition(this);
                            System.Windows.Point pointToScreen = PointToScreen(pointToWindow);
                            LbPos.Content = pointToScreen.ToString();
                            Mouse.Capture(null);

                            rect.RadiusX = pointToScreen.X;
                            rect.RadiusY = pointToScreen.Y;
                            myrect.X = Convert.ToInt32(pointToScreen.X);
                            myrect.Y = Convert.ToInt32(pointToScreen.Y);
                            myrect.Height = 200;
                            myrect.Width = 200;
                            myrect.Size = size;

                            img = CaptureRect(myrect, ImageFormat.Png);
                            imgPlane.Source = img;
                            imgPlane.StretchDirection = StretchDirection.Both;

                            if (f1flag == 1)
                            {
                                imgPopl = img;
                             //   myFlag = 0;
                                imagePoplavok.Source = img;
                                f1flag = 0;
                            //    btnStart.Content = "Start";
                            }
                            if (f2flag == 1)
                            {
                                imgVosk = img;
                            //    myFlag = 0;
                                imageVoskl.Source = img;
                                f2flag = 0;
                            //    btnStart.Content = "Start";
                            }

                        }));
                    }

                    
                };
                timer.Interval = 100;
                timer.Start();
            };
        }

        public static BitmapImage CaptureRect(System.Drawing.Rectangle rect, ImageFormat format)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(rect.Width, rect.Height,
                    System.Drawing.Imaging.PixelFormat.Format32bppRgb))
                {
                    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        graphics.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, System.Drawing.CopyPixelOperation.SourceCopy);
                    }
                    bitmap.Save(ms, format);
                }
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private void manageKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                f1flag = 1;
            }

            if (e.Key == Key.F2)
            {
                f2flag = 1;
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (myFlag == 0)
            {
                btnStart.Content = "Stop";
                myFlag = 1;
            }
            else
            {
                btnStart.Content = "Start";
                myFlag = 0;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Save(imgPopl, "popl.png");
            Save(imgVosk, "vosk.png");
        }

        public static void Save(BitmapImage image, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
    }
    
}
