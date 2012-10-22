#define FLOAT

#if DECIMAL
using FLOATINGTYPE = System.Decimal;
#endif
#if DOUBLE
using FLOATINGTYPE = System.Double;
#endif
#if FLOAT
using FLOATINGTYPE = System.Single;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Rendering.Services;

namespace System.Rendering.Forms
{
    public class TextureDescription 
    {
    }

    public class Win32TextureLoader : ILoader<TextureBuffer>
    {
        public bool Load(IO.Stream stream, out TextureBuffer texture)
        {
            try
            {
                texture = TextureLoader.FromBitmap((Bitmap)Bitmap.FromStream(stream));
                return true;
            }
            catch
            {
                texture = null;
                return false;
            }
        }

        public IEnumerable<string> Formats
        {
            get
            {
                yield return "jpg";
                yield return "jpeg";
                yield return "bmp";
                yield return "gif";
                yield return "tiff";
                yield return "png";
                yield return "wmf";
            }
        }

        public bool Save(IO.Stream stream, TextureBuffer texture, string format)
        {
            try
            {
                var bitmap = TextureLoader.ToBitmap(texture);
                System.Drawing.Imaging.ImageFormat f = System.Drawing.Imaging.ImageFormat.Bmp;
                switch (format)
                {
                    case "bmp": f = System.Drawing.Imaging.ImageFormat.Bmp; break;
                    case "gif": f = System.Drawing.Imaging.ImageFormat.Gif; break;
                    case "jpg": f = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                    case "jpeg": f = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                    case "png": f = System.Drawing.Imaging.ImageFormat.Png; break;
                    case "tiff": f = System.Drawing.Imaging.ImageFormat.Tiff; break;
                }
                bitmap.Save(stream, f);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    static class TextureLoader
    {
        public unsafe static void FastCopy(uint* source, uint* destination, int count)
        {
            while (count --> 0)
            {
                *destination = *source;
                source++;
                destination++;
            }
        }

        public static Bitmap ToBitmap(TextureBuffer texture)
        {
            Bitmap bitmap = new Bitmap(texture.Width, texture.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int rows = texture.Height;
            int cols = texture.Width;

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, cols, rows), Drawing.Imaging.ImageLockMode.ReadOnly, Drawing.Imaging.PixelFormat.Format32bppArgb);

            CustomPixels.ARGB[,] data = texture.GetData<CustomPixels.ARGB>(0, 0, rows, cols);

            IntPtr dst = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

            unsafe
            {
                FastCopy((uint*)dst, (uint*)bitmapData.Scan0, rows * cols);
            }

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        public static TextureBuffer FromBitmap(string path)
        {
            return FromBitmap((Bitmap)Bitmap.FromFile(path));
        }

        public static TextureBuffer FromBitmap(Bitmap bitmap)
        {
            return ((TextureBuffer)FastConvert(bitmap));
        }

        private static CustomPixels.ARGB[,] FastConvert(Bitmap bitmap)
        {
            CustomPixels.ARGB[,] data = new CustomPixels.ARGB[bitmap.Height, bitmap.Width];

            IntPtr dst = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

            FastCopy(bitmap, dst);

            return data;
        }

        private static void FastCopy(Bitmap bitmap, IntPtr pointer)
        {
            int rows = bitmap.Height;
            int cols = bitmap.Width;

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, cols, rows), Drawing.Imaging.ImageLockMode.ReadOnly, Drawing.Imaging.PixelFormat.Format32bppArgb);

            unsafe
            {
                FastCopy((uint*)bitmapData.Scan0, (uint*)pointer, rows * cols);
            }

            bitmap.UnlockBits(bitmapData);
        }

        public static CubeTextureBuffer FromBitmap(Bitmap positiveX, Bitmap negativeX, Bitmap positiveY, Bitmap negativeY, Bitmap positiveZ, Bitmap negativeZ)
        {
            CustomPixels.ARGB[,,] data = new CustomPixels.ARGB[6, positiveX.Height, positiveX.Width];

            IntPtr dst = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

            int countOfByteForAFace = positiveX.Width*positiveX.Height*4;

            FastCopy(positiveX, (IntPtr)((int)dst + 0 * countOfByteForAFace));
            FastCopy(negativeX, (IntPtr)((int)dst + 1 * countOfByteForAFace));
            FastCopy(positiveY, (IntPtr)((int)dst + 2 * countOfByteForAFace));
            FastCopy(negativeY, (IntPtr)((int)dst + 3 * countOfByteForAFace));
            FastCopy(positiveZ, (IntPtr)((int)dst + 4 * countOfByteForAFace));
            FastCopy(negativeZ, (IntPtr)((int)dst + 5 * countOfByteForAFace));

            return (CubeTextureBuffer)data;
        }

        public static TextureBuffer FromSize(int width, int height)
        {
            return TextureBuffer.Empty<CustomPixels.ARGB>(width, height);
        }

        public static TextureBuffer FromControl(Control control)
        {
            Bitmap b = new Bitmap(control.Width, control.Height);
            control.DrawToBitmap(b, new Rectangle(0, 0, control.Width, control.Height));

            return TextureLoader.FromBitmap(b);
        }
    }
}
