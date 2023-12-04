using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Test.Conbination
{
    public class TxtConbinator : IConbinator
    {
        public int combineAllFiles(string inputFolderPath, string outputPath, string patternToRemove = " ")
        {
            try
            {
                // list of files in folder
                string[] files = Directory.GetFiles(inputFolderPath);

                StringBuilder combinedContent = new StringBuilder();

                int deletedLinesCount = 0;

                foreach (var filePath in files)
                {
                    // Read the content of the file
                    string fileContent = File.ReadAllText(filePath);

                    // 
                    string[] lines = fileContent.Split('\n');
                    foreach (var line in lines)
                    {
                        // If the line does not contain the specified fragment
                        if (!line.Contains(patternToRemove))
                        {
                            combinedContent.AppendLine(line);
                        }
                        else
                        {
                            // Increase the counter for deleted lines
                            deletedLinesCount++;
                        }
                    }
                }

                // Write the combined content to the output file
                File.WriteAllText(outputPath, combinedContent.ToString());
                return deletedLinesCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }

        public int combineSpecificNumberOfFiles(string folderPath, string outputPath, int[] selectedFileNumbers, string patternToRemove = " ")
        {
            StringBuilder combinedContent = new StringBuilder();
            int deletedLinesCount = 0;

            // Iterate through selected file numbers
            foreach (int fileNumber in selectedFileNumbers)
            {
                // Get the path of the current file
                string filePath = getFilePath(folderPath, fileNumber);

                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Read the content of the file
                    string fileContent = File.ReadAllText(filePath);

                    // Split the content into lines
                    string[] lines = fileContent.Split('\n');

                    // Iterate through each line
                    foreach (var line in lines)
                    {
                        // If the line does not contain the specified fragment
                        if (!line.Contains(patternToRemove))
                        {
                            // Append the line to the combined content
                            combinedContent.AppendLine(line);
                        }
                        else
                        {
                            // Increase the counter for deleted lines
                            deletedLinesCount++;
                        }
                    }
                }
                else
                {
                    // Display a message if the file does not exist
                    Console.WriteLine($"File with number {fileNumber} does not exist.");
                }
            }

            // Write the combined content to the output file
            File.WriteAllText(outputPath, combinedContent.ToString());

            // Return the count of deleted lines
            return deletedLinesCount;
        }


        // Constructs the full file path based on the folder path and the file number
        private string getFilePath(string folderPath, int fileNumber)
        {
            // Generate the file name using the file number
            string fileName = $"file{fileNumber}.txt";

            // Combine the folder path and the file name to create the full file path
            return Path.Combine(folderPath, fileName);
        }

    }
}
