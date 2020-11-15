using StarMathLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace AOIS3.Prep
{
    class Program
    {
        public static IEnumerable<double> Sigmoid(IEnumerable<double> x)
        {
            return x.Select(val => 1 / (1 + Math.Exp(-val)));
        }
        static void Main(string[] args)
        {
            /*
            double[,] x = new double[,]
            {
                { 0, 0, 1 },
                { 0, 1, 1 },
                { 1, 0, 1 },
                { 1, 1, 1 }
            };

            List<double> y = new List<double> { 0, 0, 1, 1 };

            List<double> syn0 = new List<double> { -0.5, 0, 0.3 };

            for (int i = 0; i < 10000; i++)
            {
                double[,] l0 = (double[,]) x.Clone();

            }*/

            /*Image image = new Bitmap(@"E:\AOIS\AOIS3.Prep\AOIS3.Prep\Untitled.bmp");
            image = ResizeImage(image, 8, 8);
            image.Save(@"E:\AOIS\AOIS3.Prep\AOIS3.Prep\Untitled1.bmp");*/


        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }


        public static void GenerateData()
        {
            foreach (var file in
                Directory.EnumerateFiles(@"E:\AOIS\AOIS3.Prep\AOIS3.Prep\Cyrillic", "*.bmp"))
            {
                List<int> pixels = new List<int>();
                Bitmap bmp = new Bitmap(file);
                for(int i = 0; i < bmp.Width; i ++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        if (bmp.GetPixel(i, j).ToArgb() == -1)
                        {
                            pixels.Add(0);
                        }
                        else 
                        {
                            pixels.Add(1);
                        }
                    }
                }

                
                foreach (var value in pixels)
                {
                     File.AppendAllText(@"E:\AOIS\AOIS3.Prep\AOIS3.Prep\Cyrillic\data.csv", value + ",");
                }
                File.AppendAllText(@"E:\AOIS\AOIS3.Prep\AOIS3.Prep\Cyrillic\data.csv", (int) Path.GetFileName(file)[0] + Environment.NewLine);
            }
        }

    }
}
