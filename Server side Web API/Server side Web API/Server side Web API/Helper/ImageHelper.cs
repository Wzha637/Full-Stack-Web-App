using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace A1.Helper
{
    public class ImageHelper
    {
        public static Image Resize(Image image, Size newSize, out string imageType)
        {
            if (image.RawFormat.Equals(ImageFormat.Jpeg))
                imageType = "JPEG";
            else if (image.RawFormat.Equals(ImageFormat.Png))
                imageType = "PNG";
            else
                imageType = "GIF";
            Bitmap b = new Bitmap(newSize.Width, newSize.Height);//hold the resized image
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);// graphics object
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, newSize.Width, newSize.Height);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        public static string ImageToString(Image image, ImageFormat imageFormat)
        {
            string photoString;
            using (var ms = new MemoryStream())//connect to memeory
            {
                image.Save(ms, imageFormat);//Save image in memory
                byte[] photoBytes = ms.ToArray();//copy the image in the memory to a byte array
                photoString = Convert.ToBase64String(photoBytes);//Encode the byte array into a base 64 string
            }
            return photoString;//return the base 64 string, ie the image to the caller
        }
    }
}
