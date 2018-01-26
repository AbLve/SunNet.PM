using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SF.Framework.Helpers
{
    public enum ThumnailMode
    {
        EqualW,
        H,
        W,
        HW,
        Cut
    }

    public static class ThumbnailHelper
    {
        public static void Create(string inputFileName, string outputFileName, int width, int height, ThumnailMode mode, string fileType)
        {
            Image originalImage = Image.FromFile(inputFileName);
            Create(originalImage, outputFileName, width, height, mode, fileType);
        }

        public static void Create(Image originalImage, string outputFileName, int width, int height, ThumnailMode mode, string fileType)
        {
            Image bitmap = null;
            Graphics g = null;

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case ThumnailMode.EqualW:
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumnailMode.HW:
                    break;
                case ThumnailMode.W:
                    if (ow > towidth)
                    {
                        toheight = originalImage.Height * width / originalImage.Width;
                    }
                    else
                    {
                        towidth = ow;
                        toheight = oh;
                    }
                    break;
                case ThumnailMode.H:
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ThumnailMode.Cut:
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            bitmap = new Bitmap(towidth, toheight);
            g = Graphics.FromImage(bitmap);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.Transparent);
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

            fileType = fileType.ToLower();
            ImageFormat imageFormat = ImageFormat.Jpeg;
            if (fileType == "gif")
            {
                imageFormat = ImageFormat.Gif;
            }

            if (fileType == "bmp")
            {
                imageFormat = ImageFormat.Bmp;
            }

            if (fileType == "png")
            {
                imageFormat = ImageFormat.Png;
            }

            if (fileType == "jpg" || fileType == "jpeg")
            {
                imageFormat = ImageFormat.Jpeg;
            }

            bitmap.Save(outputFileName, imageFormat);

            if (originalImage != null)
                originalImage.Dispose();

            if (bitmap != null)
                bitmap.Dispose();

            if (g != null)
                g.Dispose();
        }

        public static void CreateOutWAndH(Image originalImage, string outputFileName, int width, int height, ThumnailMode mode, string fileType, out int outWidth, out int outHeight)
        {
            Image bitmap = null;
            Graphics g = null;

            outWidth = 0;
            outHeight = 0;

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case ThumnailMode.EqualW:
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumnailMode.HW:
                    break;
                case ThumnailMode.W:
                    if (ow > towidth)
                    {
                        toheight = originalImage.Height * width / originalImage.Width;
                    }
                    else
                    {
                        towidth = ow;
                        toheight = oh;
                    }
                    break;
                case ThumnailMode.H:
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ThumnailMode.Cut:
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            bitmap = new Bitmap(towidth, toheight);
            g = Graphics.FromImage(bitmap);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.Transparent);
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

            fileType = fileType.ToLower();
            ImageFormat imageFormat = ImageFormat.Jpeg;
            if (fileType == "gif")
            {
                imageFormat = ImageFormat.Gif;
            }

            if (fileType == "bmp")
            {
                imageFormat = ImageFormat.Bmp;
            }

            if (fileType == "png")
            {
                imageFormat = ImageFormat.Png;
            }

            if (fileType == "jpg" || fileType == "jpeg")
            {
                imageFormat = ImageFormat.Jpeg;
            }

            bitmap.Save(outputFileName, imageFormat);

            outWidth = bitmap.Width;
            outHeight = bitmap.Height;

            if (originalImage != null)
                originalImage.Dispose();

            if (bitmap != null)
                bitmap.Dispose();

            if (g != null)
                g.Dispose();
        }
        
        public static void Cut(string inputFileName, string outputFileName, int toWidth, int toHeight, int cropWidth, int cropHeight, int x, int y, string fileType)
        {
            Image originalImage = Image.FromFile(inputFileName);
            Cut(originalImage, outputFileName, toWidth, toWidth, cropWidth, cropHeight, x, y, fileType);
        }

        public static void Cut(Image originalImage, string outputFileName, int toWidth, int toHeight, int cropWidth, int cropHeight, int x, int y, string fileType)
        {
            Image bitmap = originalImage;
            Graphics g = null;

            int ow = originalImage.Width;
            int oh = originalImage.Height;

            bitmap = new Bitmap(toWidth, toHeight);
            g = Graphics.FromImage(bitmap);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.Transparent);
            g.DrawImage(originalImage, new Rectangle(0, 0, toWidth, toHeight), new Rectangle(x, y, cropWidth, cropHeight), GraphicsUnit.Pixel);

            fileType = fileType.ToLower();
            ImageFormat imageFormat = ImageFormat.Jpeg;
            if (fileType == "gif")
            {
                imageFormat = ImageFormat.Gif;
            }

            if (fileType == "bmp")
            {
                imageFormat = ImageFormat.Bmp;
            }

            if (fileType == "png")
            {
                imageFormat = ImageFormat.Png;
            }

            if (fileType == "jpg" || fileType == "jpeg")
            {
                imageFormat = ImageFormat.Jpeg;
            }

            bitmap.Save(outputFileName, imageFormat);

            if (originalImage != null)
                originalImage.Dispose();

            if (bitmap != null)
                bitmap.Dispose();

            if (g != null)
                g.Dispose();
        }
    }
}