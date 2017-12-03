using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace MinecraftServerList
{
    public static class ImageHelper
    {
        public static BitmapImage GetBitmapImageFromBase64(string base64)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64.Substring(base64.IndexOf(',') + 1));
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }
    }
}
