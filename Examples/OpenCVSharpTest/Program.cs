using System;
using OpenCvSharp;
namespace OpenCVSharpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string imagePath = "Lenna.png";
            Mat src = new Mat(imagePath, ImreadModes.Grayscale);
            Mat dst = new Mat();
            // Edge detection by Canny algorithm
            Cv2.Canny(src, dst, 50, 200);
            using (new Window("src image", src)) 
            using (new Window("dst image", dst)) 
            {
                Cv2.WaitKey();
            }
        }
    }
}
