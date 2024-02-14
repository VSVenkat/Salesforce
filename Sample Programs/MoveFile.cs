using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Specify the source and target directories
        string sourceDirectory = @"C:\SourceDirectory";
        string targetDirectory = @"C:\TargetDirectory";

        // Check if the source directory exists
        if (Directory.Exists(sourceDirectory))
        {
            // Get all files in the source directory
            string[] files = Directory.GetFiles(sourceDirectory);

            // Check if any files exist in the source directory
            if (files.Length > 0)
            {
                try
                {
                    // Move each file to the target directory with new file name
                    foreach (string file in files)
                    {
                        // Get the file name and construct the target file path with new file name
                        string fileName = Path.GetFileName(file);
                        string newFileName = GetNewFileName(fileName); // Call a method to get new file name
                        string targetFilePath = Path.Combine(targetDirectory, newFileName);

                        // Move the file to the target directory with new file name
                        File.Move(file, targetFilePath);

                        Console.WriteLine($"File '{fileName}' moved and renamed to '{newFileName}' successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No files found in the source directory.");
            }
        }
        else
        {
            Console.WriteLine("Source directory does not exist.");
        }

        Console.ReadLine(); // Keep console open
    }

    // Method to generate new file name based on existing file name
    static string GetNewFileName(string fileName)
    {
        // You can implement your logic to generate new file names here
        // For simplicity, let's just add a timestamp to the existing file name
        return $"{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(fileName)}";
    }
}
