using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MetadataExtractor;

namespace MetadataExtractorTest
{
    //https://github.com/drewnoakes/metadata-extractor-dotnet
    class Program
    {
        static void Main(string[] args)
        {
            var imagePath = "../../../cat.png";

            if (File.Exists(imagePath))
            {
                var directories = ImageMetadataReader.ReadMetadata(imagePath);

                foreach (var directory in directories)
                foreach (var tag in directory.Tags)
                    Console.WriteLine($"{directory.Name} - {tag.Name} = {tag.Description}");
            }
            Console.ReadKey();

        }
    }
}
