using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MiTL
{
    internal class IconManager
    {
        public static void GrabAndSaveIcon(string quickLaunchTile, string executablePath)
        {
            string iconPath = Environment.CurrentDirectory + @"\ql_icons\";
            string iconFile = "ql" + quickLaunchTile + ".ico";

            if (!Directory.Exists(iconPath))
            {
                Directory.CreateDirectory(iconPath);
            }

            Icon exeIcon = ExtractIconFromFilePath(executablePath);

            if (exeIcon != null)
            {
                Bitmap icon = exeIcon.ToBitmap();

                IntPtr hBitmap = icon.GetHbitmap();
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                     hBitmap, IntPtr.Zero, Int32Rect.Empty,
                     BitmapSizeOptions.FromEmptyOptions());

                icon.Save(iconPath + iconFile);
                icon.Dispose();
                exeIcon.Dispose();
            }
        }

        private static Icon ExtractIconFromFilePath(string executablePath)
        {
            Icon result = null;

            try
            {
                result = Icon.ExtractAssociatedIcon(executablePath);
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to extract the icon from the binary");
            }

            return result;
        }
    }
}