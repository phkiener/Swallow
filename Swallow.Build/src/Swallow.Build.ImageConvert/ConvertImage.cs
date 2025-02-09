using System.IO;
using Microsoft.Build.Framework;
using SkiaSharp;
using Task = Microsoft.Build.Utilities.Task;

namespace Swallow.Build.ImageConvert
{
    public sealed class ConvertImage : Task
    {
        [Required]
        public ITaskItem[] Images { get; set; }

        [Required]
        public string OutputDirectory { get; set; }

        [Required]
        public string Width { get; set; }

        [Required]
        public string Height { get; set; }

        public override bool Execute()
        {
            Directory.CreateDirectory(OutputDirectory);

            var width = int.Parse(Width);
            var height = int.Parse(Height);

            foreach (var file in Images)
            {
                SKCanvas canvas;
                using (var stream = File.OpenRead(file.ItemSpec))
                {
                    canvas = SKSvgCanvas.Create(new SKRect(0, 0, width, height), stream);
                }

                var surface = SKSurface.CreateNull(width, height);
                surface.Draw(canvas, 0, 0, new SKPaint());
                var image = surface.Snapshot();

                var data = image.Encode(SKEncodedImageFormat.Png, 100);

                using (var stream = File.OpenWrite(Path.Combine(OutputDirectory, Path.ChangeExtension(file.ItemSpec, ".png"))))
                {
                    data.SaveTo(stream);
                }
            }

            return true;
        }
    }
}
