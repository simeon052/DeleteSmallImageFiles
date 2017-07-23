using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteSmallImageFiles.Core
{
    public class ImageFileInfo
    {
        public int Height { get; }
        public int Width { get; }
        public long Size { get; }
        public string FullName { get; }

        public ImageFileInfo(int height, int width, long size, string fullname)
        {
            this.Height = height;
            this.Width = width;
            this.Size = size;
            this.FullName = fullname;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Name   : {this.FullName}");
            sb.AppendLine($"Size   : {this.Size}");
            sb.AppendLine($"Width  : {this.Width}");
            sb.AppendLine($"Height : {this.Height}");

            return sb.ToString();
        }
    }

    public class ImageFileLister
    {
        public static IEnumerable<string> FindAll(string parentPath, Predicate<ImageFileInfo> checker)
        {
            DirectoryInfo di = new DirectoryInfo(parentPath);
            FileInfo[] files =di.GetFiles("*.*", SearchOption.AllDirectories);

            foreach (System.IO.FileInfo f in files)
            {
                var (result, ifi) = ImageFileChecker(f);
                if (result)
                {
                    if (checker(ifi))
                    {
                        yield return f.FullName;
                    }
                }
            }
        }



        public static (bool, ImageFileInfo ) ImageFileChecker(FileInfo fi)
        {
            ImageFileInfo ifi = null;
            try
            {
                using (Image img = System.Drawing.Image.FromFile(fi.FullName))
                {
                    ifi = new ImageFileInfo(img.Height, img.Width, fi.Length, fi.FullName);
                }
            }
            catch (OutOfMemoryException ex)
            {
                System.Diagnostics.Debug.WriteLine($"{fi.FullName} is not an image file.");
                return (false , ifi);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{ex.ToString()}");
                return (false , ifi);
            }


            return (true, ifi);
        }

    }
}
