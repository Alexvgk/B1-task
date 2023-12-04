using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Test.Generation
{
    public class TxtGenerator : IGenerator
    {
        public bool generateFile()
        {
            try
            {
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "files");

                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }

                //For generating random numbers
                Random random = new Random();

                // Create 100 files
                for (int i = 1; i <= 100; i++)
                {
                    string fileName = $"file{i}.txt";
                    string filePath = Path.Combine(outputPath, fileName);
                    string fileContent = generateFileContent();

                    // Write to the file
                    File.WriteAllText(filePath, fileContent.ToString());

                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                return false;
            }
        }

        public string generateFileContent()
        {
            Random random = new Random();

            // Use StringBuilder for efficient string manipulation
            StringBuilder fileContent = new StringBuilder();

            for (int j = 0; j < 100000; j++)// Generate 100,000 lines for file
            {
                // Generate a random date from the last 5 years
                DateTime randomDate = DateTime.Now.AddDays(-random.Next(365 * 5));

                // Generate a random set of Latin characters
                string latinChars = Guid.NewGuid().ToString().Substring(0, 10);

                // Generate a random set of Russian characters
                string russianChars = generateRussianString(random);

                // Generate a random positive even integer in the range from 1 to 100,000,000
                int randomInt = random.Next(1, 50000000) * 2;

                // Generate a random positive number with 8 decimal places in the range from 1 to 20
                double randomDouble = Math.Round(random.NextDouble() * (20 - 1) + 1, 8);

                // Form the string and append it to the file content
                fileContent.Append($"{randomDate:dd.MM.yyyy}||{latinChars}||{russianChars}||{randomInt}||{randomDouble}||\n");
            }
            return fileContent.ToString();
        }


        private string generateRussianString(Random random)
        {
            StringBuilder russianChars = new StringBuilder();

            for (int i = 0; i < 10; i++)
            {
                char randomChar = (char)random.Next('А', 'я' + 1);
                russianChars.Append(randomChar);
            }

            return russianChars.ToString();
        }
    }
}
