using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FaceRecognitionTest.Utils
{
    public static class FileUtils
    {
        public static IEnumerable<(string imagePath, string label)> LoadImagesFromDirectory(string folder)
        {
            var imagePaths = Directory
                .GetFiles(folder, "*", SearchOption.AllDirectories)
                .Where(x => Path.GetExtension(x) == ".jpg" || Path.GetExtension(x) == ".png");
            return imagePaths.Select(imagePath => (imagePath, Directory.GetParent(imagePath).Name));
        }
    }
}
