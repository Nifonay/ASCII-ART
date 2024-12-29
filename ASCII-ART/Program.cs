using ImageMagick;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Loading Image ....");
        string imageFileName = "chitanda.jpg";
        // Read from file.
        using (var imageFromFile = new MagickImage(imageFileName))
        {
            Console.WriteLine("Image Loaded!");
            
            var height = imageFromFile.Height;
            var width = imageFromFile.Width;

            Console.WriteLine($"Image is {height}x{width}");

            // pixel array
            var pixels = imageFromFile.GetPixels();

            var imgInfo = new MagickImageInfo(imageFileName);
            // convert to brightness array

            // change the text color in the terminal
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            var brightness = brightnessArray(pixels, imgInfo);
            for (var j = 0; j < brightness.GetLength(1); j++)
            {
                for (var i = 0; i < brightness.GetLength(0); i++)
                {
                    Console.Write(brightnessToAscii(brightness[i, j]));
                    Console.Write(brightnessToAscii(brightness[i, j]));
                    Console.Write(brightnessToAscii(brightness[i, j]));
                }
                Console.WriteLine();
            }


            Console.WriteLine($"Depth: {imageFromFile.Depth} bits/channel");

        }
    }

    public static void printPixels(IPixelCollection<ushort> pixels)
    {
        int i = 0;
        foreach(var pixel in pixels) 
        {
            var pixelValues = pixel.ToArray();
            Console.WriteLine($"Red: {pixelValues[0]}, Green: {pixelValues[1]}, Blue: {pixelValues[2]}");

            i++;
        }
    }

    public static int[,] brightnessArray(IPixelCollection<ushort> pixels, MagickImageInfo info)
    {
        int[,] brightness = new int[info.Width, info.Height];
        
        // What Conversion method will we use?
        int method = -1;
        while (method < 1 || method > 3) { 
            Console.WriteLine("What method Would you like to use? Press 1 for Average, 2 for Lightness, or 3 for luminosity: ");
            method = Convert.ToInt32(Console.ReadLine());
        }

        // Process the pixels
        foreach (var pixel in pixels)
        {
            var pixelArray = pixel.ToArray();
           
            int brightValue;
            if (method == 1)
            {
                brightValue = (pixelArray[0] + pixelArray[1] + pixelArray[2]) / 3;
            }
            else if (method == 2)
            {
                var rgbMax = new[] { pixelArray[0], pixelArray[1], pixelArray[2] }.Max();
                var rgbMin = new[] { pixelArray[0], pixelArray[1], pixelArray[2] }.Min();
                brightValue = (rgbMax + rgbMin) / 2;
            }
            else 
            {
                brightValue = Convert.ToInt32((pixelArray[0] * 0.21) + (pixelArray[1] * 0.72) + (pixelArray[2] * 0.07));
            }
            
            brightness[pixel.X, pixel.Y] = brightValue;
        }
        return brightness;
    }


    public static char brightnessToAscii(int brightness)
    {
        
        brightness = brightness / 257;
        string asciiChars = "`^\",:;Il!i~+_-?][}{1)(|\\/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B@$";
        int index = brightness * (asciiChars.Length - 1) / 255;

        var invert = true;
        if (invert == true)
        {
            index = invertAscii(index, asciiChars.Length - 1);
        }
        return Convert.ToChar(asciiChars[index]);
    }


    public static int invertAscii(int Ascii, int characterslength)
    {
        return Math.Abs(Ascii - characterslength);
    }


}