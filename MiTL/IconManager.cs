using System;
using System.Drawing;
using System.IO;

namespace MiTL
{
    internal class IconManager
    {
        public static void GrabAndSaveIcon(string quickLaunchTile, string executablePath)
        {
            string iconPath = Environment.CurrentDirectory + @"\ql_icons\";
            string iconFile = quickLaunchTile + ".ico";

            Icon exeIcon = ExtractIconFromFilePath(executablePath);

            if (exeIcon != null)
            {
                if (!Directory.Exists(iconPath))
                {
                    Directory.CreateDirectory(iconPath);
                }
                using (FileStream stream = new FileStream(iconPath + iconFile, FileMode.Create))
                {
                    exeIcon.Save(stream);
                }
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